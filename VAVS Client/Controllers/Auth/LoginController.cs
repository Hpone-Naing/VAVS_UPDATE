using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Util;
using Microsoft.AspNetCore.Mvc;
using VAVS_Client.ViewModels;
using System.Net;
using VAVS_Client.Models;
using System.Text.Json;
using LoginAuth = VAVS_Client.Models.LoginAuth;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Controllers.Auth
{
    public class LoginController : Controller
    {
        public ServiceFactory factoryBuilder;

        public LoginController(ServiceFactory serviceFactory)
        {
            factoryBuilder = serviceFactory;
        }

        private LoginView MakeLoginView()
        {
            LoginView viewModel = new LoginView
            {
                User = new User(),
                PersonalDetail = new PersonalDetail()
            };
            return viewModel;
        }

        private void MakeViewBag()
        {
            ViewBag.IsRememberMe = SessionUtil.IsRememberMe(HttpContext);
            ViewBag.StateDivisions = factoryBuilder.CreateStateDivisionService().GetSelectListStateDivisions();
            ViewBag.Townships = factoryBuilder.CreateTownshipService().GetSelectListTownshipsByStateDivision();//factoryBuilder.CreateTownshipService().GetSelectListTownships();
        }

        public IActionResult Index()
        {
            return View("ChooseUser");
        }

        public async Task<IActionResult> LoginUser()
        {
            
            /*bool isUseVpn = await factoryBuilder.CreateDeviceInfoService().VpnTurnOn();
            if (isUseVpn)
            {
                MakeViewBag();
                Utility.AlertMessage(this, "Please turn off vpn.", "alert-danger", "true");
                return View("Login", MakeLoginView());
            }*/

            MakeViewBag();
            return View("Login", MakeLoginView());
        }

        [HttpPost]
        public IActionResult Login(string phoneNumber)
        {
            try
            {
                var loginUser = factoryBuilder.CreatePersonalDetailService().FindPersonalDetailByPhoneNumber(Utility.MakePhoneNumberWithCountryCode(phoneNumber));
                if (loginUser != null)
                {
                    return RedirectToAction("CheckLoginAuthentication", "Login");
                }
                MakeLoginView();
                MakeViewBag();
                Utility.AlertMessage(this, "Phone number haven't registered yet.", "alert-danger");
                return View("Login");
            }
            catch (Exception e)
            {
                Utility.AlertMessage(this, "SQl Connection Error.Please refresh browser.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
        }

        private LoginAuth InitializeLoginAuth(string nrc, string phoneNumber, string hashedOtp)
        {
            return new LoginAuth()
            {
                Nrc = nrc,
                PhoneNumber = phoneNumber,
                ResendOTPCount = 0,
                OTP = hashedOtp
            };
        }

        private Otp MakeOtp(HttpContext httpContext)
        {
            string digit1 = httpContext.Request.Form["digit1"];
            string digit2 = httpContext.Request.Form["digit2"];
            string digit3 = httpContext.Request.Form["digit3"];
            string digit4 = httpContext.Request.Form["digit4"];
            string digit5 = httpContext.Request.Form["digit5"];
            string digit6 = httpContext.Request.Form["digit6"];
            return new Otp()
            {
                Digit1 = digit1,
                Digit2 = digit2,
                Digit3 = digit3,
                Digit4 = digit4,
                Digit5 = digit5,
                Digit6 = digit6
            };
        }

        [HttpPost]
        public async Task<IActionResult> CheckLoginAuthentication()
        {
            try
            {
                string nrcTownshipNumber = Request.Form["NRCTownshipNumber"];
                string nrcTownshipInitial = Request.Form["NRCTownshipInitial"];
                string nrcType = Request.Form["NRCType"];
                string nrcNumber = Request.Form["NRCNumber"];
                string nrc = Utility.MakeNRC(string.Concat(nrcTownshipNumber , "/"), nrcTownshipInitial, nrcType, nrcNumber);
                PersonalDetail personalInformation = await factoryBuilder.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(nrc);
                if (personalInformation != null)
                {
                    /*LoginUserInfo loginUserInfo = new LoginUserInfo
                    {
                        TaxpayerInfo = new TaxpayerInfo
                        {
                            Name = personalInformation.Name,
                            NRC = Utility.MakeNRC(nrcTownshipNumber, nrcTownshipInitial, nrcType, nrcNumber),
                           // PhoneNumber = personalInformation.PhoneNumber,
                        },
                        LoggedInTime = DateTime.Now,
                    };
                    factoryBuilder.CreateTaxPayerInfoService().CreateLoginUserInfo(SessionUtil.GetToken(HttpContext), loginUserInfo);*/
                    HttpContext.Session.SetString(HashUtil.ComputeSHA256Hash(Utility.TOKEN), HashUtil.ComputeSHA256Hash(string.Concat(nrc, personalInformation.PhoneNumber)));
                    factoryBuilder.CreateSessionServiceService().SetLoginUserInfo(HttpContext, new TaxpayerInfo()
                    {
                        Name = personalInformation.Name,
                        NRC = nrc
                    }
                    );
                    return RedirectToAction("CheckLoginOTPCode", "Login");
                }
                MakeViewBag();
                Utility.AlertMessage(this, "You haven't registered yet!. Please register", "alert-danger", "true");
                return RedirectToAction("LoginUser", "Login");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MakeViewBag();
                Utility.AlertMessage(this, "Internal Server error.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
        }
        
        public async Task<IActionResult> CheckLoginOTPCode(bool resend)
        {
            try
            {
                /*
                 * Check vpn turn on or off
                 */
                /*bool isUseVpn = await factoryBuilder.CreateDeviceInfoService().VpnTurnOn();
                if (isUseVpn)
                {
                    MakeViewBag();
                    Utility.AlertMessage(this, "Please turn off vpn.", "alert-danger", "true");
                    return RedirectToAction("Index", "Login");
                }*/

                //LoginUserInfo loginUserInfo = factoryBuilder.CreateTaxPayerInfoService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
                
                SessionService sessionService = factoryBuilder.CreateSessionServiceService();
                if (!sessionService.IsActiveSession(HttpContext))
                {
                    MakeViewBag();
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(HttpContext);
                Console.WriteLine("1 login userinfo null........................" + (loginUserInfo == null));
                PersonalDetail personalInformation = await factoryBuilder.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);//await factoryBuilder.CreatePersonalDetailService().GetPersonalInformationByNRC(loginUserInfo.NRC);
                

                DateTime currentTime = DateTime.Now;
                DateTime expireTime = currentTime.AddSeconds(Utility.OTP_EXPIRE_SECOND);
                string storedExpireTime = HttpContext.Session.GetString("ExpireTime");

                /*
                 * Check Login User's  null or not
                 */
                LoginAuth existingUser = factoryBuilder.CreateLoginAuthDBService().GetLoginAuthByNrc(loginUserInfo.NRC);
                if (existingUser == null)
                {
                    string otp = Utility.GenerateOtp();
                    Console.WriteLine("Otp 1 is: " + otp);
                    /*
                     * Send otp code via sms
                     */
                    await factoryBuilder.CreateSMSVerificationService().SendSMSOTP(personalInformation.PhoneNumber, Utility.MakeMessage("Your OTP code is: ", otp));
                    factoryBuilder.CreateLoginAuthDBService().CreateLoginAuth(InitializeLoginAuth(loginUserInfo.NRC, personalInformation.PhoneNumber, HashUtil.ComputeSHA256Hash(otp)));
                    HttpContext.Session.SetString("ExpireTime", expireTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                    ViewData["ExpireTime"] = HttpContext.Session.GetString("ExpireTime");
                    ViewData["phoneNumber"] = personalInformation.PhoneNumber;
                    return View("LoginAuthCode");
                }
                /*
                 * Check user can request re resend code or not
                 */
                if (existingUser.ReResendCodeTime != null)
                {
                    if (existingUser.IsExceedMaximunResendCode() || !existingUser.AllowNextTimeResendOTP())
                    {
                        factoryBuilder.CreateLoginAuthDBService().UpdateResendCodeTime(loginUserInfo.NRC);
                        Utility.AlertMessage(this, "Try resend code after " + DateTime.Parse(existingUser.ReResendCodeTime).ToString(), "alert-danger");
                        return RedirectToAction("Index", "Login");
                    }
                    if (existingUser.AllowNextTimeResendOTP())
                    {
                        string otp = Utility.GenerateOtp();
                        Console.WriteLine("Otp 2 is: " + otp);
                        /*
                         * Send otp code via sms
                         */
                        await factoryBuilder.CreateSMSVerificationService().SendSMSOTP(personalInformation.PhoneNumber, Utility.MakeMessage("Your OTP code is: ", otp));

                        factoryBuilder.CreateLoginAuthDBService().UpdateResendCodeTime(loginUserInfo.NRC, HashUtil.ComputeSHA256Hash(otp));
                        HttpContext.Session.SetString("ExpireTime", expireTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                        ViewData["phoneNumber"] = personalInformation.PhoneNumber;
                        ViewData["ExpireTime"] = HttpContext.Session.GetString("ExpireTime");
                        return View("LoginAuthCode");
                    }
                }
                /*
                 * OTP code expire and user can request re resend code
                 */
                if ((string.IsNullOrEmpty(storedExpireTime) || (!string.IsNullOrEmpty(storedExpireTime) && currentTime > DateTime.Parse(storedExpireTime))))
                {
                    string otp = Utility.GenerateOtp();
                    //string otp = "111111";
                    Console.WriteLine("Otp 3 is: " + otp);
                    /*
                     * Send otp code via sms
                     */
                    await factoryBuilder.CreateSMSVerificationService().SendSMSOTP(personalInformation.PhoneNumber, Utility.MakeMessage("Your OTP code is: ", otp));

                    if (resend)
                    {
                        factoryBuilder.CreateLoginAuthDBService().UpdateResendCodeTime(loginUserInfo.NRC, HashUtil.ComputeSHA256Hash(otp));
                    }
                    else
                    {
                        Console.WriteLine("1 login userinfo null........................" + (loginUserInfo == null));
                        factoryBuilder.CreateLoginAuthDBService().UpdateOtp(loginUserInfo.NRC, HashUtil.ComputeSHA256Hash(otp));
                    }
                    HttpContext.Session.SetString("ExpireTime", expireTime.ToString("yyyy-MM-ddTHH:mm:ss"));
                    ViewData["ExpireTime"] = HttpContext.Session.GetString("ExpireTime");
                    ViewData["phoneNumber"] = personalInformation.PhoneNumber;
                    return View("LoginAuthCode");
                }
                /*
                 * OTP code not expire and check valid OTP or not. If valid save PersonalDetail
                 */
                if (currentTime < DateTime.Parse(storedExpireTime))
                {
                    if (MakeOtp(HttpContext).IsValidOtp(existingUser.OTP))
                    {
                        /*
                         * Valid OTP and set session for loginUser and redirect vehicle search page
                         */
                        //await _serviceFactory.CreateSMSVerificationService().SendSMSOTP(personalDetail.PhoneNumber, Utility.MakeMessage("Your Username", "mgmg", " Your Password", "mgmg123++", "for Login"));
                        existingUser.Nrc = loginUserInfo.NRC;
                        existingUser.PhoneNumber = personalInformation.PhoneNumber;
                        existingUser.OTP = null;
                        existingUser.ReResendCodeTime = null;
                        existingUser.ResendOTPCount = 0;
                        factoryBuilder.CreateLoginAuthDBService().UpdateLoginAuth(existingUser);
                        HttpContext.Session.Remove("ExpireTime");
                        return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
                    }
                    ViewData["ExpireTime"] = HttpContext.Session.GetString("ExpireTime");
                    Utility.AlertMessage(this, "Incorrect OTP code.", "alert-danger");
                    ViewData["phoneNumber"] = personalInformation.PhoneNumber;
                    return View("LoginAuthCode");
                }

                ViewData["ExpireTime"] = HttpContext.Session.GetString("ExpireTime");
                ViewData["phoneNumber"] = personalInformation.PhoneNumber;
                Utility.AlertMessage(this, "OTP code expire.", "alert-danger");
                return View("LoginAuthCode");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MakeViewBag();
                Utility.AlertMessage(this, "Internal Server error.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult RemoveOneTapLogin()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }

        public IActionResult ResetPhonenumber()
        {
            return View(new ResetPhonenumber());
        }

        public async Task<IActionResult> CheckTaxedUser(ResetPhonenumber resetPhonenumber)
        {
            try
            {
                bool result = await factoryBuilder.CreatePersonalDetailService().ResetPhoneNumber(resetPhonenumber);
                if (result)
                {
                    MakeViewBag();
                    Utility.AlertMessage(this, "Your phonenumber has been updated", "alert-success", "true");
                    return RedirectToAction("LoginUser", "Login");
                }
                MakeViewBag();
                Utility.AlertMessage(this, "Sorry you cannot update phonenumber due to new_phonenumber already exit, incorrect nrc or old_phonenumber, you haven't taxed vehicle yet. Please register again.", "alert-danger", "true");
                return RedirectToAction("LoginUser", "Login");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                MakeViewBag();
                Utility.AlertMessage(this, "Internal Server error.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
        }
        public IActionResult Logout()
        {
            try
            {
                LoginUserInfo loginUserInfo = SessionUtil.GetLoginUserInfo(HttpContext);
                var loginUser = new User();//factoryBuilder.CreateUserService().FindUserByUserName(loginUserInfo.PhoneNumber);

                if (loginUserInfo.RememberMe == true)
                {
                    //HttpContext.Session.SetString(HashUtil.ComputeSHA256Hash("token"), string.Concat(loginUserInfo.TaxpayerInfo.NRC, loginUserInfo.TaxpayerInfo.PhoneNumber));
                    SessionUtil.SetLoginUserInfo(HttpContext, loginUserInfo);
                    return RedirectToAction("Index", "Login");
                }
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Login");
            }
            catch (Exception e)
            {
                Utility.AlertMessage(this, "Logout Fail. Internal Server Error.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
        }

        public JsonResult GetTownshipByStateDivision(int stateDivisionPkId)
        {
            return Json(factoryBuilder.CreateTownshipService().GetSelectListTownshipsByStateDivision(stateDivisionPkId));
        }

        public JsonResult GetNRCTownshipInitials(string nrcTownshipNumber)
        {
            List<string> NRCTownshipInitials = factoryBuilder.CreatePersonalDetailService().GetNRCTownshipInitials(nrcTownshipNumber);
            return Json(NRCTownshipInitials);
        }
    }
}
