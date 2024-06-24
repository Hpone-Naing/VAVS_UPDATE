using VAVS_Client.Util;

namespace VAVS_Client.Classes
{
    public class LoginAuth
    {
        public string Nrc { get; set; }
        public string PhoneNumber { get; set; }
        public int ResendOTPCount { get; set; }
        public string ReResendCodeTime { get; set; }
        public string OTP { get; set; }

        public bool IsExceedMaximunResendCode() => (this.ResendOTPCount >= Utility.MAXIMUM_RESEND_CODE_TIME);
        public bool AllowNextTimeResendOTP()
        {
            return (DateTime.Now >= DateTime.Parse(this.ReResendCodeTime));
        }
    }
}
