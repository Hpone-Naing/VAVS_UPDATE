using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using VAVS_Client.Classes;
using VAVS_Client.Util;
using LoginAuth = VAVS_Client.Classes.LoginAuth;

namespace VAVS_Client.Services.Impl
{
    public class LoginAuthServiceImpl : LoginAuthService
    {
        private IFirebaseConfig _firebaseConfig;
        public LoginAuthServiceImpl(IFirebaseConfig firebaseConfig)
        {
            _firebaseConfig = firebaseConfig;
        }

        //IFirebaseConfig config = Utility.GetFirebaseConfig();
        IFirebaseClient client;
        public bool CreateLoginAuth(LoginAuth loginAuth)
        {
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            var data = loginAuth;
            SetResponse setResponse = client.Set(Utility.LOGIN_AUTH_FIREBASE_PATH + HashUtil.ComputeSHA256Hash(loginAuth.Nrc), data);

            if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(string.Empty, "Added Succesfully");
                return true;
            }
            else
            {
                Console.WriteLine(string.Empty, "Something went wrong!!");
                return false;
            }
        }

        public LoginAuth GetLoginAuthByPhoneNumber(string phoneNumber)
        {
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            FirebaseResponse response = client.Get(Utility.LOGIN_AUTH_FIREBASE_PATH + HashUtil.ComputeSHA256Hash(phoneNumber));
            LoginAuth loginAuth = JsonConvert.DeserializeObject<LoginAuth>(response.Body);
            Console.WriteLine("fb LoginAuth by ip: " + JsonConvert.SerializeObject(loginAuth));
            return loginAuth;
        }

        public LoginAuth GetLoginAuthByNrc(string nrc)
        {
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            FirebaseResponse response = client.Get(Utility.LOGIN_AUTH_FIREBASE_PATH + HashUtil.ComputeSHA256Hash(nrc));
            LoginAuth loginAuth = JsonConvert.DeserializeObject<LoginAuth>(response.Body);
            Console.WriteLine("fb LoginAuth by ip: " + JsonConvert.SerializeObject(loginAuth));
            return loginAuth;
        }

        public void UpdateOtp(string phoneNumber, string hashedOtp = null)
        {
            LoginAuth loginAuth = GetLoginAuthByPhoneNumber(phoneNumber);
            if (loginAuth != null)
            {
                loginAuth.OTP = hashedOtp;
            }
            CreateLoginAuth(loginAuth);
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
                        loginAuth.ReResendCodeTime = DateTime.Now.AddMinutes(Utility.NEXT_RESENDCODE_TIME_IN_MINUTE).ToString();
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
                CreateLoginAuth(loginAuth);
            }
        }
     }
}
