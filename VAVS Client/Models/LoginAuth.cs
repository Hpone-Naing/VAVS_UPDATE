using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_LoginAuth")]
    public class LoginAuth
    {

        [Key]
        public int LoginAuthId { get; set; }

        [StringLength(200)]
        public string? Nrc { get; set; }

        [StringLength(200)]
        public string? PhoneNumber { get; set; }
        public int ResendOTPCount { get; set; }

        [StringLength(200)]
        public string? ReResendCodeTime { get; set; }

        [StringLength(200)]
        public string? OTP { get; set; }

        public bool IsExceedMaximunResendCode() => (this.ResendOTPCount >= Utility.MAXIMUM_RESEND_CODE_TIME);
        public bool AllowNextTimeResendOTP()
        {
            return (DateTime.Now >= DateTime.Parse(this.ReResendCodeTime));
        }
    }
}
