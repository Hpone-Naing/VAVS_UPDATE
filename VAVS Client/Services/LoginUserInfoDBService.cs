namespace VAVS_Client.Services
{
    public interface LoginUserInfoDBService
    {
        public bool CreateLoginUserInfo(string token, LoginUserInfo loginUserInfo);
        public LoginUserInfo GetLoginUserByHashedToken(string token);
        public void UpdateTaxedPayerInfo(string token, LoginUserInfo taxPayerInfo);
        public void UpdateLoginUserInfo(LoginUserInfo taxVehicleInfo);
    }
}
