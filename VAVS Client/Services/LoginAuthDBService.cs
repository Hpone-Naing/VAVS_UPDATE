namespace VAVS_Client.Services
{
    public interface LoginAuthDBService
    {
        public bool CreateLoginAuth(LoginAuth loginAuth);
        public bool UpdateLoginAuth(LoginAuth loginAuth);
        public LoginAuth GetLoginAuthByPhoneNumber(string phoneNumber);
        public LoginAuth GetLoginAuthByNrc(string nrc);
        public void UpdateOtp(string phoneNumber, string hashedOtp = null);
        public void UpdateResendCodeTime(string nrc, string hashedOtp = null);
        public bool HardDeleteLoginAuth(LoginAuth loginAuth);
    }
}
