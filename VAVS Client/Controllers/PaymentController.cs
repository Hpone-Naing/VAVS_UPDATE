using Microsoft.AspNetCore.Mvc;
using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Services;
using VAVS_Client.Util;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ServiceFactory _serviceFactory;
        public PaymentController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        private void MakeBillingUserViewBag(PersonalDetail personalDetail)
        {
            ViewBag.Name = personalDetail.Name;
            ViewBag.NRC = Utility.MakeNRC(personalDetail.NRCTownshipNumber, personalDetail.NRCTownshipInitial, personalDetail.NRCType, personalDetail.NRCNumber);
            ViewBag.PhoneNumber = personalDetail.PhoneNumber;
            ViewBag.Address = string.Concat("အမှတ်("+personalDetail.HousingNumber+")၊", personalDetail.Street, "၊", personalDetail.Quarter, "၊", personalDetail.Township.TownshipName, "၊", personalDetail.Township.StateDivision.StateDivisionName);
            var financialYear = _serviceFactory.CreateFinancialYearService().GetFinancialYear(personalDetail.EntryDate);
            if (financialYear != null)
            {
                ViewBag.TaxYear = financialYear["FinancialYear"].ToString();
            }
            ViewBag.TaxOffice = string.Concat(personalDetail.StateDivision.StateDivisionName, " Office");
        }

        public async Task<IActionResult> Payment()
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "Login First!", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            LoginUserInfo loginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
            if(loginUserInfo == null)
            {
                Utility.AlertMessage(this, "Search Vehicle First!", "alert-danger");
                return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
            }
            TaxpayerInfo loginTaxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
            PersonalDetail personalDetail = await _serviceFactory.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(loginTaxPayerInfo.NRC);
            MakeBillingUserViewBag(personalDetail);
            ViewBag.TaxAmount = loginUserInfo.TaxAmount;
            return View();
        }
    }
}
