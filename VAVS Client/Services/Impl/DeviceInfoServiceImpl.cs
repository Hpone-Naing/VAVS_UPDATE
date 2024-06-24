using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using IPinfo;
using IPinfo.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using VAVS_Client.Classes;
using VAVS_Client.Util;
using DeviceInfo = VAVS_Client.Classes.DeviceInfo;

namespace VAVS_Client.Services.Impl
{
    public class DeviceInfoServiceImpl : DeviceInfoService
    {
        private IFirebaseConfig _firebaseConfig;
        public DeviceInfoServiceImpl(IFirebaseConfig firebaseConfig)
        {
            _firebaseConfig = firebaseConfig;
        }

        //IFirebaseConfig config = Utility.GetFirebaseConfig();
        IFirebaseClient client;
        public bool CreateDeviceInfo(DeviceInfo deviceInfo)
        {
            client = new FireSharp.FirebaseClient(_firebaseConfig/*config*/);
            var data = deviceInfo;
            SetResponse setResponse = client.Set(Utility.REGISTRATION_AUTH_FIREBASE_PATH/*"DeviceInfos/"*/ + HashUtil.ComputeSHA256Hash(deviceInfo.IpAddress), data);

            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(string.Empty, "Added Succesfully");
                return true;
            }
            else
            {
                Console.WriteLine(string.Empty, "Something went wrong!!");
                return false;
            }
        }

        public DeviceInfo GetDeviceInfoByIPAddress(string ipAddress)
        {
            client = new FireSharp.FirebaseClient(_firebaseConfig/*config*/);
            FirebaseResponse response = client.Get(Utility.REGISTRATION_AUTH_FIREBASE_PATH /*"DeviceInfos/"*/ + HashUtil.ComputeSHA256Hash(ipAddress));
            DeviceInfo deviceInfo = JsonConvert.DeserializeObject<DeviceInfo>(response.Body);
            Console.WriteLine("fb DeviceInfo by ip: " + JsonConvert.SerializeObject(deviceInfo));
            return deviceInfo;
        }
        public void UpdateRegistrationTime(string ipAddress)
        {
            DeviceInfo deviceInfo = GetDeviceInfoByIPAddress(ipAddress);
            if (deviceInfo != null)
            {
                if (deviceInfo.IsExceedMaximunRegistration() || (deviceInfo.ReRegistrationTime != null && !deviceInfo.AllowNextTimeRegister()))
                {
                    if (deviceInfo.ReRegistrationTime == null)
                    {
                        deviceInfo.RegistrationCount = 0;
                        deviceInfo.ResendCodeTime = 0;
                        deviceInfo.ReRegistrationTime = DateTime.Now.AddMinutes(Utility.NEXT_REGISTER_TIME_IN_MINUTE).ToString();
                    }
                    else
                    {
                        if (deviceInfo.AllowNextTimeRegister())
                        {
                            //deviceInfo.RegistrationCount = 0;
                            //deviceInfo.ResendCodeTime = 0;
                            deviceInfo.ReRegistrationTime = null;
                            deviceInfo.ReResendCodeTime = null;
                        }
                    }
                }
                else
                {
                    deviceInfo.ReRegistrationTime = null;
                    deviceInfo.ReResendCodeTime = null;
                    deviceInfo.RegistrationCount++;
                    deviceInfo.ResendCodeTime = 0;
                }
                CreateDeviceInfo(deviceInfo);
            }
        }

        public void UpdateOtp(string ipAddress, string hashedOtp = null)
        {
            DeviceInfo deviceInfo = GetDeviceInfoByIPAddress(ipAddress);
            if (deviceInfo != null)
            {
                deviceInfo.OTP = hashedOtp;
            }
            CreateDeviceInfo(deviceInfo);
        }
        public void UpdateResendCodeTime(string ipAddress, string hashedOtp = null)
        {
            DeviceInfo deviceInfo = GetDeviceInfoByIPAddress(ipAddress);
            if (deviceInfo != null)
            {
                if (deviceInfo.IsExceedMaximunResendCode() || (deviceInfo.ReResendCodeTime != null && !deviceInfo.AllowNextTimeResendOTP()))
                {
                    if (deviceInfo.ReResendCodeTime == null)
                    {
                        deviceInfo.ResendCodeTime = 0;
                        deviceInfo.ReResendCodeTime = DateTime.Now.AddMinutes(Utility.NEXT_RESENDCODE_TIME_IN_MINUTE).ToString();
                    }
                    else
                    {
                        if (deviceInfo.AllowNextTimeResendOTP())
                        {
                            deviceInfo.OTP = hashedOtp;
                            deviceInfo.ReResendCodeTime = null;
                        }
                    }
                }
                else
                {
                    deviceInfo.OTP = hashedOtp;
                    deviceInfo.ReResendCodeTime = null;
                    deviceInfo.ResendCodeTime++;
                }
                CreateDeviceInfo(deviceInfo);
            } 
        }

        public async Task<string> GetPublicIPAddress()
        {
            string apiUrl = "https://ipinfo.io/ip";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> VpnTurnOn()
        {
            string token = "ce0f6c22b96675";
            IPinfoClient client = new IPinfoClient.Builder()
                .AccessToken(token)
                .Build();
            string publicIp = await GetPublicIPAddress();
            Console.WriteLine("Public Ip is: " + publicIp);
            IPResponse ipResponse = await client.IPApi.GetDetailsAsync(publicIp);
            Console.WriteLine("VPN IP? " + JsonConvert.SerializeObject(ipResponse));
            if(ipResponse.Privacy != null)
                return ipResponse != null && ipResponse.Privacy.Vpn;
            Console.WriteLine((ipResponse.Country != "MM") + " / " + (ipResponse.CountryName != "Myanmar"));
            return ipResponse.Country != "MM" || ipResponse.CountryName != "Myanmar";
        }
    }
}
