using System.Net;
using VAVS_Client.Factories;
using VAVS_Client.Models;
using VAVS_Client.Util;

namespace VAVS_Client.Classes
{
    public class DeviceInfo
    {
        public string IpAddress { get; set; }
        public string PublicIpAddress { get; set; }
        public int RegistrationCount { get; set; }
        public int ResendCodeTime { get; set; }
        public string ReRegistrationTime { get; set; }
        public string ReResendCodeTime { get; set; }
        public string OTP { get; set; }

        public bool IsSameIp(string ipAddress) => (this.IpAddress == ipAddress);

        public bool IsExceedMaximunRegistration() => (this.RegistrationCount >= Utility.MAXIMUM_REGISTRATION_TIME);
        public bool IsExceedMaximunResendCode() => (this.ResendCodeTime >= Utility.MAXIMUM_RESEND_CODE_TIME);
        //public bool AllowNextTimeRegister() => (DateTime.Now >= DateTime.Parse(this.ReRegistrationTime));
        //public bool AllowNextTimeResendOTP() => (DateTime.Now >= DateTime.Parse(ReResendCodeTime));
        public bool AllowNextTimeRegister()
        {
            return (DateTime.Now >= DateTime.Parse(this.ReRegistrationTime));
        }

        public bool AllowNextTimeResendOTP()
        {
            return (DateTime.Now >= DateTime.Parse(this.ReResendCodeTime));
        }
    }
}
