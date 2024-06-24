using VAVS_Client.Classes;
using LoginAuth = VAVS_Client.Classes.LoginAuth;

namespace VAVS_Client.Services
{
    public interface LoginAuthService
    {
        bool CreateLoginAuth(LoginAuth loginAuth);
        LoginAuth GetLoginAuthByPhoneNumber(string phoneNumber);
        LoginAuth GetLoginAuthByNrc(string nrc);
        void UpdateOtp(string phoneNumber, string hashedOtp = null);
        void UpdateResendCodeTime(string nrc, string hashedOtp = null);
    }
}
