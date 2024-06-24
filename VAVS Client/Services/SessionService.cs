using VAVS_Client.Classes;

namespace VAVS_Client.Services
{
    public interface SessionService
    {
        public void SetLoginUserInfo(HttpContext httpContext, TaxpayerInfo userInfo);
        public TaxpayerInfo GetLoginUserInfo(HttpContext httpContext);
        public bool IsActiveSession(HttpContext httpContext);
    }
}
