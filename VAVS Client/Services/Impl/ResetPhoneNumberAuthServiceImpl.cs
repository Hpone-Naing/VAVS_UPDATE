using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using VAVS_Client.Classes;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class ResetPhoneNumberAuthServiceImpl : ResetPhoneNumberAuthService
    {
        private IFirebaseConfig _firebaseConfig;
        public ResetPhoneNumberAuthServiceImpl(IFirebaseConfig firebaseConfig)
        {
            _firebaseConfig = firebaseConfig;
        }

        IFirebaseClient client;
        public bool CreateResetPhoneNumberAuthInfo(string token, ResetPhoneNumberAuth resetPhonenumberAuth)
        {
            Console.WriteLine("Here crate login user info in fb.................");
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            var data = resetPhonenumberAuth;
            SetResponse setResponse = client.Set(Utility.ResetPhonenumberAuth_FIREBASE_PATH + token, data);

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

        public ResetPhoneNumberAuth GetResetPhoneNumberAuthByHashedToken(string token)
        {
            Console.WriteLine("GetResetPhoneNumberAuthByHashedToken " + token);
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            FirebaseResponse response = client.Get(Utility.ResetPhonenumberAuth_FIREBASE_PATH + token);
            ResetPhoneNumberAuth resetPhonenumberAuth = JsonConvert.DeserializeObject<ResetPhoneNumberAuth>(response.Body);
            Console.WriteLine("firebase ResetPhoneNumber by token: " + JsonConvert.SerializeObject(resetPhonenumberAuth));
            return resetPhonenumberAuth;
        }

        public void UpdateOtp(string token, string hashedOtp = null)
        {
            ResetPhoneNumberAuth resetPhonenumberAuth = GetResetPhoneNumberAuthByHashedToken(token);
            if (resetPhonenumberAuth != null)
            {
                resetPhonenumberAuth.OTP = hashedOtp;
            }
            CreateResetPhoneNumberAuthInfo(token, resetPhonenumberAuth);
        }
        public void UpdateResendCodeTime(string token, string hashedOtp = null)
        {
            ResetPhoneNumberAuth resetPhonenumberAuth = GetResetPhoneNumberAuthByHashedToken(token);
            if (resetPhonenumberAuth != null)
            {
                if (resetPhonenumberAuth.IsExceedMaximunResendCode() || (resetPhonenumberAuth.ReResendCodeTime != null && !resetPhonenumberAuth.AllowNextTimeResendOTP()))
                {
                    if (resetPhonenumberAuth.ReResendCodeTime == null)
                    {
                        resetPhonenumberAuth.ResendOTPCount = 0;
                        resetPhonenumberAuth.ReResendCodeTime = DateTime.Now.AddMinutes(Utility.NEXT_RESENDCODE_TIME_IN_MINUTE).ToString();
                    }
                    else
                    {
                        if (resetPhonenumberAuth.AllowNextTimeResendOTP())
                        {
                            resetPhonenumberAuth.OTP = hashedOtp;
                            resetPhonenumberAuth.ReResendCodeTime = null;
                        }
                    }
                }
                else
                {
                    resetPhonenumberAuth.OTP = hashedOtp;
                    resetPhonenumberAuth.ReResendCodeTime = null;
                    resetPhonenumberAuth.ResendOTPCount++;
                }
                CreateResetPhoneNumberAuthInfo(token, resetPhonenumberAuth);
            }
        }
    }
}
