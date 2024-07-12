using FireSharp.Response;
using Newtonsoft.Json;
using VAVS_Client.Data;

namespace VAVS_Client.Services.Impl
{
    public class LoginUserInfoDBServiceImpl : AbstractServiceImpl<LoginUserInfo>, LoginUserInfoDBService
    {
        private readonly ILogger<LoginUserInfoDBServiceImpl> _logger;

        public LoginUserInfoDBServiceImpl(VAVSClientDBContext context, ILogger<LoginUserInfoDBServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool CreateLoginUserInfo(string token, LoginUserInfo loginUserInfo)
        {
            loginUserInfo.Token = token;
            return Create(loginUserInfo);
        }

        public LoginUserInfo GetLoginUserByHashedToken(string token)
        {
            return FindByString("Token", token);
        }

        public void UpdateTaxedPayerInfo(string token, LoginUserInfo taxPayerInfo)
        {
            LoginUserInfo loginUserInfo = GetLoginUserByHashedToken(token);
            if (loginUserInfo != null)
            {
                //loginUserInfo.TaxpayerInfo = taxPayerInfo;
            }
            Update(taxPayerInfo);
        }

        public bool HardDeleteTaxedPayerInfo(LoginUserInfo taxPayerInfo)
        {
            try
            {
                return HardDelete(taxPayerInfo);
            }
            catch(Exception ex)
            {
                _logger.LogError("Exception occur when hard delete: " + ex);
                throw;
            }
            
        }

        public void UpdateLoginUserInfo(LoginUserInfo taxVehicleInfo)
        {
            Update(taxVehicleInfo);
        }
    }
}
