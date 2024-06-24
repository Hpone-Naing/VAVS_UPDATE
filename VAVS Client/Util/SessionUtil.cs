using Newtonsoft.Json;
using VAVS_Client.Classes;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Util
{
    public class SessionUtil
    {
        public static void SetLoginUserInfo(HttpContext httpContext, LoginUserInfo userInfo)
        {
            if (httpContext == null || userInfo == null)
            {
                throw new ArgumentNullException();
            }

            string userInfoJson = JsonConvert.SerializeObject(userInfo);
            httpContext.Session.SetString("LoginUserInfo", userInfoJson);
        }

        public static LoginUserInfo GetLoginUserInfo(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException();
            }

            string userInfoJson = httpContext.Session.GetString("LoginUserInfo");

            if (userInfoJson != null)
            {
                return JsonConvert.DeserializeObject<LoginUserInfo>(userInfoJson);
            }

            return null;
        }
        
        public static bool IsRememberMe(HttpContext httpContext)
        {
            LoginUserInfo loginUserInfo = GetLoginUserInfo(httpContext);
            return ( (loginUserInfo != null && loginUserInfo.RememberMe == true));

        }

        public static bool IsActiveSession(HttpContext httpContext)
        {
            LoginUserInfo loginUserInfo = GetLoginUserInfo(httpContext);
            return loginUserInfo != null;
        }

        public static string GetToken(HttpContext httpContext)
        {
            return httpContext.Session.GetString(HashUtil.ComputeSHA256Hash(Utility.TOKEN));
        }
        public static string GetResetPhoneNumberToken(HttpContext httpContext)
        {
            return httpContext.Session.GetString(HashUtil.ComputeSHA256Hash(Utility.RESET_PHONE_TOKEN));
        }
    }
}
