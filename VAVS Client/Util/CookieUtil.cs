using Newtonsoft.Json;
using VAVS_Client.Classes;
using DeviceInfo = VAVS_Client.Classes.DeviceInfo;

namespace VAVS_Client.Util
{
    public class CookieUtil
    {
        public static void SetDeviceInfo(HttpContext httpContext, DeviceInfo deviceInfo)
        {
            string deviceInfoJson = JsonConvert.SerializeObject(deviceInfo);

            httpContext.Response.Cookies.Append(deviceInfo.IpAddress, deviceInfoJson, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMonths(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
        }

        public static DeviceInfo GetDeviceInfo(HttpContext httpContext, string ipAddress)
        {
            string deviceInfoJson = httpContext.Request.Cookies[ipAddress];

            if (!string.IsNullOrEmpty(deviceInfoJson))
            {
                return JsonConvert.DeserializeObject<DeviceInfo>(deviceInfoJson);
            }

            return null;
        }

        public static void UpdateResendCodeTime(HttpContext httpContext, string ipAddress)
        {
            DeviceInfo deviceInfo = GetDeviceInfo(httpContext, ipAddress);
            if(deviceInfo != null)
            {
                if(deviceInfo.IsExceedMaximunResendCode() || (deviceInfo.ReResendCodeTime!=null && !deviceInfo.AllowNextTimeResendOTP()))
                {
                    if(deviceInfo.ReResendCodeTime == null)
                    {
                        deviceInfo.ResendCodeTime = 0;
                        deviceInfo.ReResendCodeTime = DateTime.Now.AddSeconds(30).ToString();
                    }
                    else
                    {
                        if (deviceInfo.AllowNextTimeResendOTP())
                        {
                            deviceInfo.ReResendCodeTime = null;
                        }
                    }
                }
                else
                {
                    deviceInfo.ReResendCodeTime = null;
                    deviceInfo.ResendCodeTime++;
                    
                }
                SetDeviceInfo(httpContext, deviceInfo);
                Console.WriteLine("Reg cookies: " + JsonConvert.SerializeObject(GetDeviceInfo(httpContext, ipAddress)));

            }
        }

        public static void UpdateRegistrationTime(HttpContext httpContext, string ipAddress)
        {
            DeviceInfo deviceInfo = GetDeviceInfo(httpContext, ipAddress);
            if(deviceInfo != null)
            {
                Console.WriteLine("deviceInfo != null");
                if(deviceInfo.IsExceedMaximunRegistration() || (deviceInfo.ReRegistrationTime != null && !deviceInfo.AllowNextTimeRegister()))
                {
                    Console.WriteLine("exceed max reg / not allow nexttimereg");

                    if (deviceInfo.ReRegistrationTime == null)
                    {
                        Console.WriteLine("reregtime null");

                        deviceInfo.RegistrationCount = 0;
                        deviceInfo.ResendCodeTime = 0;
                        deviceInfo.ReRegistrationTime = DateTime.Now.AddSeconds(15).ToString();
                    }
                    else
                    {
                        Console.WriteLine("reregtime not null");

                        if (deviceInfo.AllowNextTimeRegister())
                        {
                            Console.WriteLine("allow next time reregtime");

                            //deviceInfo.RegistrationCount = 0;
                            //deviceInfo.ResendCodeTime = 0;
                            deviceInfo.ReRegistrationTime = null;
                            deviceInfo.ReResendCodeTime = null;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("not exceed max reg /  allow nexttimereg");

                    deviceInfo.ReRegistrationTime = null;
                    deviceInfo.ReResendCodeTime = null;
                    deviceInfo.RegistrationCount++;
                    deviceInfo.ResendCodeTime = 0;
                }
                SetDeviceInfo(httpContext, deviceInfo);
                Console.WriteLine("Reg cookies: " + JsonConvert.SerializeObject(GetDeviceInfo(httpContext, ipAddress)));
            }
        }

        public static void SetExpireTime(HttpContext httpContext, string expireTime)
        {

            httpContext.Response.Cookies.Append("ExpireTime", expireTime, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMonths(1),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

        }



        public static string GetExpireTime(HttpContext httpContext)
        {
            string expireTime = httpContext.Request.Cookies["ExpireTime"];
            if (!string.IsNullOrEmpty(expireTime))
            {
                return expireTime;

            }

            return null;
        }
    }
}
