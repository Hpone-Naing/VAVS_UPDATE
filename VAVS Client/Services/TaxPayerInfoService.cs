using VAVS_Client.Classes;
using LoginUserInfo = VAVS_Client.Classes.LoginUserInfo;

namespace VAVS_Client.Services
{
    public interface TaxPayerInfoService
    {
        bool CreateLoginUserInfo(string token, LoginUserInfo loginUserInfo);
        LoginUserInfo GetLoginUserByHashedToken(string token);
        void UpdateTaxedPayerInfo(string token, TaxpayerInfo taxPayerInfo);
        void UpdateTaxVehicleInfo(string token, TaxVehicleInfo taxVehicleInfo);

    }
}
