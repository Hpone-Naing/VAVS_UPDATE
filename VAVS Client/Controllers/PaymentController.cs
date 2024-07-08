using Microsoft.AspNetCore.Mvc;
using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Util;

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
        }

        public async Task<IActionResult> Payment()
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "Login First!", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            TaxpayerInfo loginTaxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
            PersonalDetail personalDetail = await _serviceFactory.CreatePersonalDetailService().GetPersonalInformationByNRCInDBAndAPI(loginTaxPayerInfo.NRC);
            MakeBillingUserViewBag(personalDetail);
            return View();
        }
    }
}
