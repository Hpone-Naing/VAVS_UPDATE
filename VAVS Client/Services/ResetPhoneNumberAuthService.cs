using VAVS_Client.Classes;

namespace VAVS_Client.Services
{
    public interface ResetPhoneNumberAuthService
    {
        bool CreateResetPhoneNumberAuthInfo(string token, ResetPhoneNumberAuth resetPhonenumberAuth);
        ResetPhoneNumberAuth GetResetPhoneNumberAuthByHashedToken(string token);
        void UpdateOtp(string token, string hashedOtp = null);
        void UpdateResendCodeTime(string token, string hashedOtp = null);
    }
}
