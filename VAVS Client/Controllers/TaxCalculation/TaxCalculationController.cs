using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;
using VAVS_Client.Classes;
using VAVS_Client.Classes.TaxCalculation;
using VAVS_Client.Factories;
using VAVS_Client.Models;
using VAVS_Client.Util;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Controllers.TaxCalculation
{
    public class TaxCalculationController : Controller
    {
        public ServiceFactory _serviceFactory;

        public TaxCalculationController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        [HttpPost]
        public IActionResult ShowCalculateTaxForm(VehicleStandardValue vehicleStandardValue)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            TaxpayerInfo taxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            if (vehicleStandardValue.VehicleNumber != null)
            {
                bool IsTaxed = _serviceFactory.CreateTaxValidationService().IsTaxedVehicle(taxPayerInfo.NRC, vehicleStandardValue.VehicleNumber);
                if(IsTaxed)
                {
                    Utility.AlertMessage(this, "This vehicle has already taxed.", "alert-info");
                    return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
                }
            }
            LoginUserInfo loginUserInfo = new LoginUserInfo()
            {
                StandardValue = vehicleStandardValue?.StandardValue,
                VehicleNumber = vehicleStandardValue?.VehicleNumber,
                Manufacturer = vehicleStandardValue?.Manufacturer,
                BuildType = vehicleStandardValue?.BuildType,
                FuelType = vehicleStandardValue?.Fuel?.FuelType,
                ModelYear = vehicleStandardValue?.ModelYear,
                CountryOfMade = vehicleStandardValue?.CountryOfMade,
                EnginePower = vehicleStandardValue?.EnginePower,
                VehicleBrand = vehicleStandardValue?.VehicleBrand,
                Grade = vehicleStandardValue?.Grade,
                ChessisNumber = vehicleStandardValue?.ChessisNumber
            };
            LoginUserInfo existingLoginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
            if (existingLoginUserInfo != null)
            {
                existingLoginUserInfo.StandardValue = vehicleStandardValue?.StandardValue;
                existingLoginUserInfo.VehicleNumber = vehicleStandardValue?.VehicleNumber;
                existingLoginUserInfo.Manufacturer = vehicleStandardValue?.Manufacturer;
                existingLoginUserInfo.BuildType = vehicleStandardValue?.BuildType;
                existingLoginUserInfo.FuelType = vehicleStandardValue?.Fuel?.FuelType;
                existingLoginUserInfo.ModelYear = vehicleStandardValue?.ModelYear;
                existingLoginUserInfo.CountryOfMade = vehicleStandardValue?.CountryOfMade;
                existingLoginUserInfo.EnginePower = vehicleStandardValue?.EnginePower;
                existingLoginUserInfo.VehicleBrand = vehicleStandardValue?.VehicleBrand;
                existingLoginUserInfo.Grade = vehicleStandardValue?.Grade;
                existingLoginUserInfo.ChessisNumber = vehicleStandardValue?.ChessisNumber;
                _serviceFactory.CreateLoginUserInfoDBService().UpdateLoginUserInfo(existingLoginUserInfo);

            }
            else
            {
                _serviceFactory.CreateLoginUserInfoDBService().CreateLoginUserInfo(SessionUtil.GetToken(HttpContext), loginUserInfo);
            }
            return View("TaxCalculation", vehicleStandardValue);
        }

        private async void MakeViewBag()
        {
            //ViewBag.StateDivisions = _serviceFactory.CreateStateDivisionService().GetSelectListStateDivisions();
            //ViewBag.Townships = _serviceFactory.CreateTownshipService().GetSelectListTownshipsByStateDivision();//factoryBuilder.CreateTownshipService().GetSelectListTownships();
            TaxpayerInfo taxPayerInfo = _serviceFactory.CreateSessionServiceService().GetLoginUserInfo(HttpContext);
            PersonalDetail personalDetail = await _serviceFactory.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(taxPayerInfo.NRC);
            ViewBag.RegisteredStateDivisionName = personalDetail.Township.StateDivision.StateDivisionName;
            ViewBag.RegisteredTownshipName = personalDetail.Township.TownshipName;
        }

        [HttpPost]
        public async Task<IActionResult> ShowCalculatedTaxForm(VehicleStandardValue vehicleStandardValue)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            TaxpayerInfo loginTaxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
            
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            
            if (vehicleStandardValue.VehicleNumber != null)
            {
                bool IsTaxed = _serviceFactory.CreateTaxValidationService().IsTaxedVehicle(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber);
                if (IsTaxed)
                {
                    Utility.AlertMessage(this, "This vehicle has already taxed.", "alert-info");
                    return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
                }
            }

            string nrc = loginTaxPayerInfo.NRC;
            PersonalDetail personalInformation = await _serviceFactory.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(nrc);//await _serviceFactory.CreatePersonalDetailService().GetPersonalInformationByNRC(nrc);
            /*
             * Save Image Files
             */
            if (vehicleStandardValue.IsImageFilesNotNull())
            {
                FileService fileService = _serviceFactory.CreateFileService();
                string NrcFileName = fileService.GetFileName(vehicleStandardValue.NrcImageFile);
                string CensusFileName = fileService.GetFileName(vehicleStandardValue.CensusImageFile);
                string TransactionContractFileName = fileService.GetFileName(vehicleStandardValue.TransactionContractImageFile);
                string OwnerBookFileName = fileService.GetFileName(vehicleStandardValue.OwnerBookImageFile);
                string WheelTabFileName = fileService.GetFileName(vehicleStandardValue.WheelTagImageFile);
                string VehicleFileName = fileService.GetFileName(vehicleStandardValue.VehicleImageFile);
                List<(string fileName, IFormFile file)> files = new List<(string fileName, IFormFile file)> {
                         (NrcFileName, vehicleStandardValue.NrcImageFile),
                         (CensusFileName, vehicleStandardValue.CensusImageFile),
                         (TransactionContractFileName, vehicleStandardValue.TransactionContractImageFile),
                         (OwnerBookFileName, vehicleStandardValue.OwnerBookImageFile),
                         (WheelTabFileName, vehicleStandardValue.WheelTagImageFile),
                         (VehicleFileName, vehicleStandardValue.VehicleImageFile),
                    };
                string savePath = loginTaxPayerInfo.NRC;
                fileService.SaveFile(Utility.ConcatNRCSemiComa(savePath), vehicleStandardValue.VehicleNumber, files);

                TaxPersonImage taxPersonImage = _serviceFactory.CreateTaxPersonImageService().GetTaxPersonImageByPersonalDetailPkIdAndCarNumber(personalInformation.PersonalPkid, vehicleStandardValue.VehicleNumber);
                if (taxPersonImage != null)
                {
                    Console.WriteLine("here not null..........................................");
                    taxPersonImage.NrcImagePath = NrcFileName;
                    taxPersonImage.CensusImagePath = CensusFileName;
                    taxPersonImage.TransactionContractImagePath = TransactionContractFileName;
                    taxPersonImage.OwnerBookImagePath = OwnerBookFileName;
                    taxPersonImage.WheelTagImagePath = WheelTabFileName;
                    taxPersonImage.VehicleImagePath = VehicleFileName;
                    taxPersonImage.NrcImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, NrcFileName);
                    taxPersonImage.CensusImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, CensusFileName);
                    taxPersonImage.TransactionContractImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, TransactionContractFileName);
                    taxPersonImage.OwnerBookImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, OwnerBookFileName);
                    taxPersonImage.WheelTagImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, WheelTabFileName);
                    taxPersonImage.VehicleImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, VehicleFileName);
                    _serviceFactory.CreateTaxPersonImageService().UpdateTaxPersonImage(taxPersonImage);
                }
                else
                {
                    Console.WriteLine("here null..........................................");

                    _serviceFactory.CreateTaxPersonImageService().SaveTaxPersonImage(
                        new TaxPersonImage()
                        {
                            NrcImagePath = NrcFileName,
                            CensusImagePath = CensusFileName,
                            TransactionContractImagePath = TransactionContractFileName, 
                            OwnerBookImagePath = OwnerBookFileName,
                            WheelTagImagePath = WheelTabFileName,
                            VehicleImagePath = VehicleFileName,
                            NrcImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, NrcFileName),
                            CensusImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, CensusFileName),
                            TransactionContractImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, TransactionContractFileName),
                            OwnerBookImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, OwnerBookFileName),
                            WheelTagImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, WheelTabFileName),
                            VehicleImageUrl = Utility.MakeImageUrl(loginTaxPayerInfo.NRC, vehicleStandardValue.VehicleNumber, VehicleFileName),
                            CarNumber = vehicleStandardValue.VehicleNumber,
                            PersonalDetail = personalInformation
                        }    
                    );
                }
            }
            
            string contractPriceString = Request.Form["ContractPrice"];
            long ContractPrice = Utility.MakeDigit(contractPriceString);
            long AssetValue = Utility.MakeDigit(vehicleStandardValue.StandardValue);
            long totalTax = _serviceFactory.CreateTaxCalculationService().CalculateTotalTax(ContractPrice, AssetValue);
            TaxValidation taxValidation = new TaxValidation
            {
                VehicleStandardValue = vehicleStandardValue,
                PersonalDetail = personalInformation,
                TaxAmount = totalTax,
                ContractValue = ContractPrice,
            };
            LoginUserInfo loginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
            
            loginUserInfo.VehicleNumber = vehicleStandardValue?.VehicleNumber;
            loginUserInfo.TaxAmount = totalTax.ToString();
            loginUserInfo.ContractValue = ContractPrice.ToString();
            _serviceFactory.CreateLoginUserInfoDBService().UpdateLoginUserInfo(loginUserInfo);
            ViewBag.BaseValue = AssetValue > ContractPrice ? AssetValue.ToString() : ContractPrice.ToString();
            return View("CalculatedTax", taxValidation);
        }

        public  IActionResult ShowTaxOfficeAddressForm()
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            MakeViewBag();
            return View("TaxOfficeAddressForm", new TaxInfo());
        }

        [HttpPost]
        public async Task<IActionResult> SaveTaxTransaction(TaxInfo taxInfo)
        {
            try
            {
                SessionService sessionService = _serviceFactory.CreateSessionServiceService();
                if (!sessionService.IsActiveSession(HttpContext))
                {
                    MakeViewBag();
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }
                TaxpayerInfo taxPayerinfo = sessionService.GetLoginUserInfo(HttpContext);
                PersonalDetail personalInformation = await _serviceFactory.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(taxPayerinfo.NRC);//await factoryBuilder.CreatePersonalDetailService().GetPersonalInformationByNRC(loginUserInfo.NRC);
                LoginUserInfo loginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
                await _serviceFactory.CreateTaxCalculationService().SaveTaxValidation(HttpContext, taxInfo);
                await _serviceFactory.CreateSMSVerificationService().SendSMSOTP(personalInformation.PhoneNumber, "လူကြီးမင်း၏မော်တော်ယာဉ်အမှတ်["+loginUserInfo.VehicleNumber+"]အခွန်စည်းကြပ်မှုအတွက် ကျသင့်အခွန်ငွေ("+loginUserInfo.TaxAmount+") ကျပ် ပေးသွင်းမှုအောင်မြင်ပြီးဖြစ်ပါ၍"+personalInformation.Township.TownshipName+"မြို့နယ်အခွန်ရုံးတွင်ဤအထောက်အထားအားပြသ၍အခွန်ကင်းရှင်းကြောင်းထောက်ခံချက်ပုံစံ(၁)အခွန်စိမ်းကိုလူကြီးမင်းအဆင်ပြေသည့်နေ့ရုံးချိန်အတွင်းသွားရောက်ထုတ်ယူနိုင်ပါသည်။");
                Utility.AlertMessage(this, "Success. Please wait for admin response", "alert-success");
                return RedirectToAction("PendingList", "TaxValidation");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occur when saving taxValidation" + e);
                Utility.AlertMessage(this, "Internal server error.", "alert-danger");
                return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
                
            }
        }
    }
}
