using VAVS_Client.Util;

namespace VAVS_Client.Classes
{
    public class Otp
    {
        public string Digit1 { get; set; }
        public string Digit2 { get; set; }
        public string Digit3 { get; set; }
        public string Digit4 { get; set; }
        public string Digit5 { get; set; }
        public string Digit6 { get; set; }
        public bool IsValidOtp(string otp) => (HashUtil.ComputeSHA256Hash(string.Concat(Digit1, Digit2, Digit3, Digit4, Digit5, Digit6)) == otp);
        public void ConvertOtpToHash() => HashUtil.ComputeSHA256Hash(string.Concat(Digit1, Digit2, Digit3, Digit4, Digit5, Digit6));
    }
}
