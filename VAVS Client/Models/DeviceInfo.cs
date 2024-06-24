using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_DeviceInfo")]
    public class DeviceInfo
    {
        [Key]
        public int DeviceInfoId { get; set; }

        [StringLength(200)]
        public string? IpAddress { get; set; }

        [StringLength(200)]
        public string? PublicIpAddress { get; set; }
        public int RegistrationCount { get; set; }
        public int ResendCodeTime { get; set; }

        [StringLength(200)]
        public string? ReRegistrationTime { get; set; }

        [StringLength(200)]
        public string? ReResendCodeTime { get; set; }

        [StringLength(200)]
        public string? OTP { get; set; }

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
