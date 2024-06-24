using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using VAVS_Client.Classes;
using VAVS_Client.Util;
using LoginUserInfo = VAVS_Client.Classes.LoginUserInfo;

namespace VAVS_Client.Services.Impl
{
    public class TaxPayerInfoServiceImpl : TaxPayerInfoService
    {
        private IFirebaseConfig _firebaseConfig;
        public TaxPayerInfoServiceImpl(IFirebaseConfig firebaseConfig)
        {
            _firebaseConfig = firebaseConfig;
        }

        IFirebaseClient client;
        public bool CreateLoginUserInfo(string token, LoginUserInfo loginUserInfo)
        {
            Console.WriteLine("Here crate login user info in fb.................");
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            var data = loginUserInfo;
            SetResponse setResponse = client.Set(Utility.LoginUserInfo_FIREBASE_PATH + token, data);

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

        public LoginUserInfo GetLoginUserByHashedToken(string token)
        {
            Console.WriteLine("GetLoginUserByHashedToken " + token);
            client = new FireSharp.FirebaseClient(_firebaseConfig);
            FirebaseResponse response = client.Get(Utility.LoginUserInfo_FIREBASE_PATH + token);
            LoginUserInfo loginUserInfo = JsonConvert.DeserializeObject<LoginUserInfo>(response.Body);
            Console.WriteLine("firebase LoginUserInfo by token: " + JsonConvert.SerializeObject(loginUserInfo));
            return loginUserInfo;
        }

        public void UpdateTaxedPayerInfo(string token, TaxpayerInfo taxPayerInfo)
        {
            LoginUserInfo loginUserInfo = GetLoginUserByHashedToken(token);
            if (loginUserInfo != null)
            {
                //loginUserInfo.TaxpayerInfo = taxPayerInfo;
            }
            CreateLoginUserInfo(token, loginUserInfo);
        }

        public void UpdateTaxVehicleInfo(string token, TaxVehicleInfo taxVehicleInfo)
        {
            LoginUserInfo loginUserInfo = GetLoginUserByHashedToken(token);
            if (loginUserInfo != null)
            {
                loginUserInfo.TaxVehicleInfo = taxVehicleInfo;
            }
            CreateLoginUserInfo(token, loginUserInfo);
        }
    }
}
