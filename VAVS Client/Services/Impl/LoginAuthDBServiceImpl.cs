using FireSharp.Response;
using Newtonsoft.Json;
using VAVS_Client.Data;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class LoginAuthDBServiceImpl : AbstractServiceImpl<LoginAuth>, LoginAuthDBService
    {
        private readonly ILogger<LoginAuthDBServiceImpl> _logger;

        public LoginAuthDBServiceImpl(VAVSClientDBContext context, ILogger<LoginAuthDBServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool CreateLoginAuth(LoginAuth loginAuth)
        {
            return Create(loginAuth);
        }

        public bool UpdateLoginAuth(LoginAuth loginAuth)
        {
            return Update(loginAuth);
        }

        public LoginAuth GetLoginAuthByPhoneNumber(string phoneNumber)
        {
            return FindByString("PhoneNumber", phoneNumber);
        }

        public LoginAuth GetLoginAuthByNrc(string nrc)
        {
            return FindByString("Nrc", nrc);
        }

        public void UpdateOtp(string nrc, string hashedOtp = null)
        {
            LoginAuth loginAuth = GetLoginAuthByNrc(nrc);
            if (loginAuth != null)
            {
                loginAuth.OTP = hashedOtp;
            }
            Update(loginAuth);
        }
        public void UpdateResendCodeTime(string nrc, string hashedOtp = null)
        {
            LoginAuth loginAuth = GetLoginAuthByNrc(nrc);
            if (loginAuth != null)
            {
                if (loginAuth.IsExceedMaximunResendCode() || (loginAuth.ReResendCodeTime != null && !loginAuth.AllowNextTimeResendOTP()))
                {
                    if (loginAuth.ReResendCodeTime == null)
                    {
                        loginAuth.ResendOTPCount = 0;
                        loginAuth.ReResendCodeTime = DateTime.Now.AddMinutes(Utility.NEXT_RESENDCODE_TIME_IN_MINUTE).ToString("dd/MM/yyyy hh:mm:ss tt");
                    }
                    else
                    {
                        if (loginAuth.AllowNextTimeResendOTP())
                        {
                            loginAuth.OTP = hashedOtp;
                            loginAuth.ReResendCodeTime = null;
                        }
                    }
                }
                else
                {
                    loginAuth.OTP = hashedOtp;
                    loginAuth.ReResendCodeTime = null;
                    loginAuth.ResendOTPCount++;
                }
                Update(loginAuth);
            }
        }

        public bool HardDeleteLoginAuth(LoginAuth loginAuth)
        {
            try
            {
                return HardDelete(loginAuth);
            }
            catch(Exception e) 
            {
                throw;
            }


        }
    }
}
