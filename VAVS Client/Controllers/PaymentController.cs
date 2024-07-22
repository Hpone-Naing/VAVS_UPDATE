using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Models;
using VAVS_Client.Paging;
using VAVS_Client.Services;
using VAVS_Client.Util;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly PendingPaymentLimitService _pendingPaymentLimitService;
        public PaymentController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _pendingPaymentLimitService = _serviceFactory.CreatePendingPaymentLimitService();
        }

        private void MakeBillingUserViewBag(PersonalDetail personalDetail)
        {
            ViewBag.Name = personalDetail.Name;
            ViewBag.NRC = Utility.MakeNRC(personalDetail.NRCTownshipNumber, personalDetail.NRCTownshipInitial, personalDetail.NRCType, personalDetail.NRCNumber);
            ViewBag.PhoneNumber = personalDetail.PhoneNumber;
            var financialYear = _serviceFactory.CreateFinancialYearService().GetFinancialYear(personalDetail.EntryDate);
            if (financialYear != null)
            {
                ViewBag.TaxYear = financialYear["FinancialYear"].ToString();
            }
            ViewBag.TaxOffice = string.Concat(personalDetail.StateDivision.StateDivisionName, " Office");
        }

        private PendingPaymentLimit InitializePendingPaymentLimit(string nrc)
        {
            return new PendingPaymentLimit()
            {
                Nrc = nrc,
                Count = 0,
                LimitTime = null,
            };
        }

        public async Task<IActionResult> Payment(int paymentPkid)
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
            TaxpayerInfo taxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
            Payment payment;
            if (paymentPkid != 0)
            {
                payment = await _serviceFactory.CreatePaymentService().FindPaymentByIdEgerLoad(paymentPkid);
                if(payment == null || (payment != null && (taxPayerInfo.NRC != payment.TaxValidation.PersonNRC)))
                {
                    Utility.AlertMessage(this, "Payment အချက်အလက်များပြင်ဆင်ခွင့်မရှိပါ!", "alert-danger");
                    return RedirectToAction("RemainPayments", "Payment");
                }
                return View(payment);
            }
            else
            {
                PendingPaymentLimit pendingPaymentLimit = _pendingPaymentLimitService.GetPendingPaymentLimitByNrc(taxPayerInfo.NRC);
                if(pendingPaymentLimit == null)
                {
                    _pendingPaymentLimitService.CreatePendingPaymentLimit(InitializePendingPaymentLimit(taxPayerInfo.NRC));
                }
                
                if (pendingPaymentLimit != null && (pendingPaymentLimit.LimitTime != null && !pendingPaymentLimit.AllowNextTimePendingPayment()))
                {
                    ViewBag.LimitTime = pendingPaymentLimit.LimitTime;
                    return View("LimitPayment");
                }
                if (pendingPaymentLimit != null && pendingPaymentLimit.IsExceedMaximun() && pendingPaymentLimit.LimitTime == null)
                {
                    _pendingPaymentLimitService.UpdatePendingPaymentLimit(taxPayerInfo.NRC);
                    ViewBag.LimitTime = pendingPaymentLimit.LimitTime;
                    return View("LimitPayment");
                }
                _pendingPaymentLimitService.UpdatePendingPaymentLimit(taxPayerInfo.NRC);
                payment = await _serviceFactory.CreatePaymentService().CretePayment(HttpContext);
                return View(payment);
            }
        }        

        public async Task<IActionResult> RemainPayments()
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "Login First!", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            LoginUserInfo loginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
            if (loginUserInfo == null)
            {
                Utility.AlertMessage(this, "Search Vehicle First!", "alert-danger");
                return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
            }
            List<Payment> remainPayments = await _serviceFactory.CreatePaymentService().GetRemainPaymentList(HttpContext);
            return View("RemainPayment", remainPayments);
        }

        public async Task<IActionResult> ApprovePayments(int? pageNo)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "Login First!", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            LoginUserInfo loginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
            if (loginUserInfo == null)
            {
                Utility.AlertMessage(this, "Search Vehicle First!", "alert-danger");
                return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
            }
            int pageSize = Utility.DEFAULT_PAGINATION_NUMBER;
            List<Payment> approvePayments = await _serviceFactory.CreatePaymentService().GetApprovePaymentListEgerLoad(HttpContext);
            PagingList<Payment> paymentsPagin = PagingList<Payment>.CreateAsync(approvePayments.AsQueryable(), pageNo ?? 1, pageSize);
            return View("ApprovePayments", paymentsPagin);
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(Payment payment)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "Login First!", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            LoginUserInfo loginUserInfo = _serviceFactory.CreateLoginUserInfoDBService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
            if (loginUserInfo == null)
            {
                Utility.AlertMessage(this, "Search Vehicle First!", "alert-danger");
                return RedirectToAction("SearchVehicleStandardValue", "VehicleStandardValue");
            }
            if (!string.IsNullOrEmpty(payment.TransactionNumber))
            {
                payment = await _serviceFactory.CreatePaymentService().FindPaymentByVAVATransactionNumberEgerLoad(payment.TransactionNumber);
                if (payment == null || (payment != null && (sessionService.GetLoginUserInfo(HttpContext).NRC != payment.TaxValidation.PersonNRC)))
                {
                    Utility.AlertMessage(this, "Payment အချက်အလက်များပြင်ဆင်ခွင့်မရှိပါ!", "alert-danger");
                    return RedirectToAction("RemainPayments", "Payment");
                }
                await _serviceFactory.CreatePaymentService().UpdatePaymentStatus(payment.TransactionNumber);
                _pendingPaymentLimitService.UpdatePendingPaymentLimitAfterMakePayment(sessionService.GetLoginUserInfo(HttpContext).NRC);
                //return RedirectToAction("PendingList", "TaxValidation");
                return RedirectToAction("ShowTaxOfficeAddressForm", "TaxCalculation");
            }
            Utility.AlertMessage(this, "Payment အချက်အလက်များပြင်ဆင်ခွင့်မရှိပါ!", "alert-danger");
            return RedirectToAction("RemainPayments", "Payment");
        }
    }
}
