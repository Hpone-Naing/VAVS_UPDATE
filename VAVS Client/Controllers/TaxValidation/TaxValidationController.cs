using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing;
using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Models;
using VAVS_Client.Paging;
using VAVS_Client.Services;
using VAVS_Client.Util;

namespace VAVS_Client.Controllers.TaxValidationController
{
    public class TaxValidationController : Controller
    {
        private readonly ServiceFactory _serviceFactory;
        public TaxValidationController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        private string MakeExcelFileName()
        {

            return "မှတ်တမ်းအားလုံး" + DateTime.Now + ".xlsx";
         
        }

        public IActionResult ExportExcel(PagingList<TaxValidation> taxValidations)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Tax Validations Data Report");
                var headerCell = ws.Cell(1, 1);
                headerCell.Value = "ပြည်တွင်းအခွန်များဦးစီးငှာန";
                headerCell.AsRange().AddToNamed("HeaderRange");
                ws.Range(1, 1, 1, 13).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(1, 1, 1, 13).Style.Font.Bold = true;
                ws.Range(1, 1, 1, 13).Style.Font.FontSize = 16;

                var secondHeaderCell = ws.Cell(2, 1);
                secondHeaderCell.Value = "VAVS";
                secondHeaderCell.AsRange().AddToNamed("HeaderRange");
                ws.Range(2, 1, 2, 13).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(2, 1, 2, 13).Style.Font.Bold = true;
                ws.Range(2, 1, 2, 13).Style.Font.FontSize = 15;

                ws.Cell(3, 1).Value = "ယာဥ်ပိုင်ရှင် အချက်အလက်";
                ws.Range(3, 1, 3, 7).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(3, 1, 3, 7).Style.Font.Bold = true;
                ws.Range(3, 1, 3, 7).Style.Font.FontSize = 14;

                ws.Cell(3, 8).Value = "ယာဥ်အချက်အလက်";
                ws.Range(3, 8, 3, 11).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(3, 8, 3, 11).Style.Font.Bold = true;
                ws.Range(3, 8, 3, 11).Style.Font.FontSize = 14;

                ws.Cell(3, 12).Value = "အခွန်ဆိုင်ရာ အချက်အလက်";
                ws.Range(3, 12, 3, 19).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Range(3, 12, 3, 19).Style.Font.Bold = true;
                ws.Range(3, 12, 3, 19).Style.Font.FontSize = 14;

                // Set background color for the header rows
                ws.Range("A3:S3").Style.Fill.BackgroundColor = XLColor.DodgerBlue;

                DataTable dt = _serviceFactory.CreateTaxValidationService().MakeVehicleDataExcelData(taxValidations, HttpContext);
                ws.Cell(4, 1).InsertTable(dt);
                ws.Range("A4:N4").Style.Fill.BackgroundColor = XLColor.DodgerBlue;
                ws.Rows().AdjustToContents();
                ws.Rows().Height = 20;
                ws.Columns().AdjustToContents();
                ws.Columns().Style.Font.FontSize = 14;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", MakeExcelFileName());
                }
            }
        } 
        public async Task<IActionResult> PendingList(int? pageNo)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            int pageSize = Utility.DEFAULT_PAGINATION_NUMBER;
            try
            {
                TaxpayerInfo loginTaxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
                if(loginTaxPayerInfo.NRC != null)
                {
                    PersonalDetail personalDetail = _serviceFactory.CreatePersonalDetailService().FindPersonalDetailByNrc(Utility.ConcatNRCSemiComa(loginTaxPayerInfo.NRC));
                    ViewBag.Address = personalDetail.HousingNumber + "၊" + personalDetail.Quarter + "၊" + personalDetail.Street;
                    ViewBag.GIR = string.IsNullOrEmpty(personalDetail.GIR) ? "" : personalDetail.GIR;

                }
                List<TaxValidation> PagedtaxValidation = await _serviceFactory.CreatePaymentService().GetTaxValidationListByPendingPayment(HttpContext);
                PagingList<TaxValidation> taxValidations = PagingList<TaxValidation>.CreateAsync(PagedtaxValidation.AsQueryable(), pageNo ?? 1, pageSize);
                if (Request.Query["export"] == "excel")
                {
                    bool ExportAll = Request.Query["ExportAll"] == "true";
                    List<TaxValidation> PagedTaxValidation = await _serviceFactory.CreatePaymentService().GetTaxValidationEgerLoadListByPendingPayment(HttpContext);
                    return ExportExcel(PagingList<TaxValidation>.CreateAsync(PagedTaxValidation.AsQueryable(), pageNo ?? 1, pageSize));
                }
                return View(taxValidations);
            }
            catch (Exception ne)
            {
                Console.WriteLine("Error: " + ne);
                Utility.AlertMessage(this, "Data Issue. Please fill VehicleData in database", "alert-danger");
                return View();
            }

        }

        public IActionResult ApproveList(int? pageNo)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }

            int pageSize = Utility.DEFAULT_PAGINATION_NUMBER;
            try
            {
                TaxpayerInfo loginTaxPayerInfo = sessionService.GetLoginUserInfo(HttpContext);
                if (loginTaxPayerInfo.NRC != null)
                {
                    PersonalDetail personalDetail = _serviceFactory.CreatePersonalDetailService().FindPersonalDetailByNrc(Utility.ConcatNRCSemiComa(loginTaxPayerInfo.NRC));
                    ViewBag.Address = personalDetail.HousingNumber + "၊" + personalDetail.Quarter + "၊" + personalDetail.Street + "၊";
                    ViewBag.StateDivision = personalDetail.Township.StateDivision.StateDivisionName;
                    ViewBag.Township = personalDetail.Township.TownshipName;
                    ViewBag.GIR = string.IsNullOrEmpty(personalDetail.GIR) ? "" : personalDetail.GIR;
                }
                PagingList<TaxValidation> taxValidations = _serviceFactory.CreateTaxValidationService().GetTaxValidationApprevedListPagin(HttpContext, pageNo, pageSize);
                if (Request.Query["export"] == "excel")
                {
                    bool ExportAll = Request.Query["ExportAll"] == "true";
                    return ExportExcel(_serviceFactory.CreateTaxValidationService().GetTaxValidationApprevedListForExcelPagin(HttpContext, pageNo, pageSize));
                }
                return View(taxValidations);
            }
            catch (Exception ne)
            {
                Console.WriteLine("Error: " + ne);
                Utility.AlertMessage(this, "Data Issue. Please fill VehicleData in database", "alert-danger");
                return View();
            }

        }

        public IActionResult Details(int id)
        {
            SessionService sessionService = _serviceFactory.CreateSessionServiceService();
            if (!sessionService.IsActiveSession(HttpContext))
            {
                Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                return RedirectToAction("Index", "Login");
            }
            Console.WriteLine("id: ..................." + id);
            TaxValidation taxValidation = _serviceFactory.CreateTaxValidationService().FindTaxValidationByIdEgerLoad(id);
            Console.WriteLine("person nrc......................" + (taxValidation==null));
            return View(taxValidation);
        }
    }
}
