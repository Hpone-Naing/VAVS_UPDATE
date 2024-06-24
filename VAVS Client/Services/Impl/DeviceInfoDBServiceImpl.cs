using FireSharp.Response;
using IPinfo.Models;
using IPinfo;
using Newtonsoft.Json;
using VAVS_Client.Data;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class DeviceInfoDBServiceImpl : AbstractServiceImpl<DeviceInfo>, DeviceInfoDBService
    {
        private readonly ILogger<DeviceInfoDBServiceImpl> _logger;

        public DeviceInfoDBServiceImpl(VAVSClientDBContext context, ILogger<DeviceInfoDBServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool CreateDeviceInfo(DeviceInfo deviceInfo)
        {
            deviceInfo.IpAddress = HashUtil.ComputeSHA256Hash(deviceInfo.IpAddress);
            return Create(deviceInfo);
        }

        public DeviceInfo GetDeviceInfoByIPAddress(string ipAddress)
        {
            return FindByString("IpAddress", HashUtil.ComputeSHA256Hash(ipAddress));
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
                Update(deviceInfo);
            }
        }

        public void UpdateOtp(string ipAddress, string hashedOtp = null)
        {
            DeviceInfo deviceInfo = GetDeviceInfoByIPAddress(ipAddress);
            if (deviceInfo != null)
            {
                deviceInfo.OTP = hashedOtp;
            }
            Update(deviceInfo);
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
                Update(deviceInfo);
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
            if (ipResponse.Privacy != null)
                return ipResponse != null && ipResponse.Privacy.Vpn;
            Console.WriteLine((ipResponse.Country != "MM") + " / " + (ipResponse.CountryName != "Myanmar"));
            return ipResponse.Country != "MM" || ipResponse.CountryName != "Myanmar";
        }
    }
}
