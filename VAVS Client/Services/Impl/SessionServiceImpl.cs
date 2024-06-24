using Newtonsoft.Json;
using VAVS_Client.Classes;

namespace VAVS_Client.Services.Impl
{
    public class SessionServiceImpl : SessionService
    {
        public void SetLoginUserInfo(HttpContext httpContext, TaxpayerInfo userInfo)
        {
            if (httpContext == null || userInfo == null)
            {
                throw new ArgumentNullException();
            }

            string userInfoJson = JsonConvert.SerializeObject(userInfo);
            httpContext.Session.SetString("LoginUserInfo", userInfoJson);
        }

        public TaxpayerInfo GetLoginUserInfo(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException();
            }

            string userInfoJson = httpContext.Session.GetString("LoginUserInfo");

            if (userInfoJson != null)
            {
                return JsonConvert.DeserializeObject<TaxpayerInfo>(userInfoJson);
            }

            return null;
        }

        public bool IsActiveSession(HttpContext httpContext)
        {
            TaxpayerInfo loginUserInfo = GetLoginUserInfo(httpContext);
            return loginUserInfo != null;
        }
    }
}
