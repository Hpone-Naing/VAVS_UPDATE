using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using VAVS_Client.Classes;
using VAVS_Client.Factories;
using VAVS_Client.Models;
using VAVS_Client.Paging;
using VAVS_Client.Services;
using VAVS_Client.Util;
using VAVS_Client.ViewModels;

namespace VAVS_Client.Controllers.VehicleStandardValueController
{
    public class VehicleStandardValueController : Controller
    {
        private readonly ServiceFactory _serviceFactory;
        public VehicleStandardValueController(ServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        private SearchLimit InitializeSearchLimit(string nrc)
        {
            return new SearchLimit()
            {
                Nrc = nrc,
                SearchCount = 0,
                ReSearchTime = null,
            };
        }

        public async Task<IActionResult> SearchVehicleStandardValue()
        {
            try
            {
                /*LoginUserInfo loginTaxPayerInfo = _serviceFactory.CreateTaxPayerInfoService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
                if (loginTaxPayerInfo.IsTaxpayerInfoNull())
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }*/
                SessionService sessionService = _serviceFactory.CreateSessionServiceService();
                if(!sessionService.IsActiveSession(HttpContext))
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }
                string nrc = sessionService.GetLoginUserInfo(HttpContext).NRC;
                SearchLimitService searchLimitSerice = _serviceFactory.CreateSearchLimitService();
                SearchLimit searchLimit = searchLimitSerice.GetSearchLimitByNrc(nrc);
                string searchString;
                VehicleStandardValue vehicleStandardValue;
                /*if(searchLimit == null)
                {
                    searchLimitSerice.CreateSearchLimit(InitializeSearchLimit(nrc));
                }
                if(searchLimit.IsExceedMaximunSearch())
                {
                    if(searchLimit.ReSearchTime == null)
                    {
                        searchLimitSerice.UpdateSearchLimit(nrc);
                        ViewBag.SearchLimit = searchLimit.ReSearchTime;
                        return View("LimitSearch");
                    }
                    if(!searchLimit.AllowNextTimeRegiste())
                    {
                        ViewBag.SearchLimit = searchLimit.ReSearchTime;
                        return View("LimitSearch");
                    }
                    
                    searchString = Request.Query["SearchString"];
                    ViewBag.SearchString = searchString;
                    if (string.IsNullOrEmpty(searchString))
                        return View();
                    searchLimitSerice.UpdateSearchLimit(nrc);
                    vehicleStandardValue = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleValueByVehicleNumberInDBAndAPI(searchString);//await _serviceFactory.CreateVehicleStandardValueService().GetVehicleValueByVehicleNumber(searchString);
                    if (vehicleStandardValue == null)
                    {
                        ViewBag.SearchString = "Not Found";
                        ViewBag.CurrentPage = "SearchVehicleStandardValue";
                        return View("SearchVehicleStandardValue");
                    }
                    if (string.IsNullOrEmpty(vehicleStandardValue.ChessisNumber))
                    {
                        vehicleStandardValue.ChessisNumber = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleChessisNumber(searchString);
                    }
                    return View("Details", vehicleStandardValue);
                }*/
                searchString = Request.Query["SearchString"];
                ViewBag.SearchString = searchString;

                if (string.IsNullOrEmpty(searchString))
                    return View();
                //searchLimitSerice.UpdateSearchLimit(nrc);
                vehicleStandardValue = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleValueByVehicleNumberInDBAndAPI(searchString);//await _serviceFactory.CreateVehicleStandardValueService().GetVehicleValueByVehicleNumber(searchString);
                if (vehicleStandardValue == null)
                {
                    ViewBag.SearchString = "Not Found";
                    ViewBag.CurrentPage = "SearchVehicleStandardValue";
                    return View("SearchVehicleStandardValue");
                }
                if(string.IsNullOrEmpty(vehicleStandardValue.ChessisNumber))
                {
                     vehicleStandardValue.ChessisNumber = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleChessisNumber(searchString);
                }
                return View("Details", vehicleStandardValue);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception occur: " + e);
                Utility.AlertMessage(this, "Server Error encounter. Fail to view detail page.", "alert-danger");
                return RedirectToAction(nameof(SearchVehicleStandardValue));
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetVehicleValue()
        {
            try
            {
                /*LoginUserInfo loginTaxPayerInfo = _serviceFactory.CreateTaxPayerInfoService().GetLoginUserByHashedToken(SessionUtil.GetToken(HttpContext));
                if (loginTaxPayerInfo.IsTaxpayerInfoNull())
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }*/
                if (!_serviceFactory.CreateSessionServiceService().IsActiveSession(HttpContext))
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }

                string manufacturer = Request.Form["manufacturer"];
                string buildType = Request.Form["buildType"];
                string fuelType = Request.Form["fuelType"];
                string vehicleBrand = Request.Form["vehicleBrand"];
                string modelYear = Request.Form["modelYear"];
                string enginePower = Request.Form["enginePower"];

                VehicleStandardValue vehicleStandardValue = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleValue(manufacturer, buildType, fuelType, vehicleBrand, modelYear, enginePower);
                if(vehicleStandardValue == null)
                {
                    ViewBag.SearchString = "Not Found";
                    return View("SearchVehicleStandardValue");
                }
                return View("Details", vehicleStandardValue);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception...." + e);
                Utility.AlertMessage(this, "Server Error encounter. Fail to view detail page.", "alert-danger");
                return RedirectToAction(nameof(SearchVehicleStandardValue));
            }
        }

        public async Task<IActionResult> GetVehicleValueByModelAndYear(string MadeModel, string makeModelYear, int? pageNo)
        {
            try
            {
                if (!_serviceFactory.CreateSessionServiceService().IsActiveSession(HttpContext))
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }
                List<VehicleStandardValue> vehicleStandardValues = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleStandardValueByModelAndYear(MadeModel, makeModelYear);
                int pageSize = Utility.DEFAULT_PAGINATION_NUMBER;
                PagingList<VehicleStandardValue> pageVehicleStandardValues =  PagingList<VehicleStandardValue>.CreateAsync(vehicleStandardValues.AsQueryable<VehicleStandardValue>(), pageNo ?? 1, pageSize);

                if (vehicleStandardValues.Count == 1)
                {
                    Console.WriteLine("Here cont 1..................");
                    return View("Details", vehicleStandardValues[0]); 
                }
                Console.WriteLine("Here cont > 1..................");
                ViewBag.MadeModel = MadeModel;
                ViewBag.MadeYear = makeModelYear;
                return View("SearchVehicleStandardValue", pageVehicleStandardValues);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception...." + e);
                Utility.AlertMessage(this, "Server Error encounter. Fail to view detail page.", "alert-danger");
                return RedirectToAction(nameof(SearchVehicleStandardValue));
            }
        }

        public async Task<IActionResult> GetVehicleValueByModelAndBrandAndYear(string MadeModel, string brand, string makeModelYear, int? pageNo)
        {
            try
            {
                SessionService sessionService = _serviceFactory.CreateSessionServiceService();
                if (!sessionService.IsActiveSession(HttpContext))
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }
                string nrc = sessionService.GetLoginUserInfo(HttpContext).NRC;
                SearchLimitService searchLimitSerice = _serviceFactory.CreateSearchLimitService();
                SearchLimit searchLimit = searchLimitSerice.GetSearchLimitByNrc(nrc);
                List<VehicleStandardValue> vehicleStandardValues;
                PagingList<VehicleStandardValue> pageVehicleStandardValues;
                int pageSize;
                if (searchLimit == null)
                {
                    searchLimitSerice.CreateSearchLimit(InitializeSearchLimit(nrc));
                }
                if (searchLimit.IsExceedMaximunSearch())
                {
                    if (searchLimit.ReSearchTime == null)
                    {
                        searchLimitSerice.UpdateSearchLimit(nrc);
                        ViewBag.SearchLimit = searchLimit.ReSearchTime;
                        return View("LimitSearch");
                    }
                    if (!searchLimit.AllowNextTimeRegiste())
                    {
                        ViewBag.SearchLimit = searchLimit.ReSearchTime;
                        return View("LimitSearch");
                    }
                    searchLimitSerice.UpdateSearchLimit(nrc);
                    vehicleStandardValues = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleStandardValueByModelAndBrandAndYear(MadeModel, brand, makeModelYear);
                    pageSize = Utility.DEFAULT_PAGINATION_NUMBER;
                    pageVehicleStandardValues = PagingList<VehicleStandardValue>.CreateAsync(vehicleStandardValues.AsQueryable<VehicleStandardValue>(), pageNo ?? 1, pageSize);

                    if (vehicleStandardValues.Count == 1)
                    {
                        return View("Details", vehicleStandardValues[0]);
                    }
                    ViewBag.MadeModel = MadeModel;
                    ViewBag.MadeYear = makeModelYear;
                    ViewBag.Brand = brand;
                    return View("SearchVehicleStandardValue", pageVehicleStandardValues);
                }
                searchLimitSerice.UpdateSearchLimit(nrc);
                vehicleStandardValues = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleStandardValueByModelAndBrandAndYear(MadeModel, brand, makeModelYear);
                pageSize = Utility.DEFAULT_PAGINATION_NUMBER;
                pageVehicleStandardValues = PagingList<VehicleStandardValue>.CreateAsync(vehicleStandardValues.AsQueryable<VehicleStandardValue>(), pageNo ?? 1, pageSize);

                if (vehicleStandardValues.Count == 1)
                {
                    return View("Details", vehicleStandardValues[0]);
                }
                ViewBag.MadeModel = MadeModel;
                ViewBag.MadeYear = makeModelYear;
                ViewBag.Brand = brand;
                return View("SearchVehicleStandardValue", pageVehicleStandardValues);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception...." + e);
                Utility.AlertMessage(this, "Server Error encounter. Fail to view detail page.", "alert-danger");
                return RedirectToAction(nameof(SearchVehicleStandardValue));
            }
        }

        public async Task<IActionResult> Details(int Id, int pkId, string madeModel, string modelYear)
        {

            try
            {
                if (!_serviceFactory.CreateSessionServiceService().IsActiveSession(HttpContext))
                {
                    Utility.AlertMessage(this, "You haven't login yet.", "alert-danger");
                    return RedirectToAction("Index", "Login");
                }
                if (string.IsNullOrEmpty(madeModel) && string.IsNullOrEmpty(modelYear))
                {
                    VehicleStandardValue vehicleStandardValue = _serviceFactory.CreateVehicleStandardValueService().FindVehicleStandardValueByIdEgerLoad(Id);
                    return View(vehicleStandardValue);
                }
                else
                {
                    List<VehicleStandardValue> vehicleStandardValues = await _serviceFactory.CreateVehicleStandardValueService().GetVehicleStandardValueByModelAndYear(madeModel, modelYear);
                    foreach(VehicleStandardValue vehicleStandard in vehicleStandardValues)
                    {
                        if(pkId == vehicleStandard.VehicleStandardValuePkid)
                        {
                            return View(vehicleStandard);
                        }
                    }
                    Utility.AlertMessage(this, "Server Error encounter. Fail to view detail page.", "alert-danger");
                    return RedirectToAction(nameof(SearchVehicleStandardValue));
                }
            }
            catch (Exception e)
            {
                Utility.AlertMessage(this, "Server Error encounter. Fail to view detail page.", "alert-danger");
                return RedirectToAction(nameof(SearchVehicleStandardValue));
            }
        }

        public async Task<JsonResult> GetMadeModel(string searchString)
        {
            List<string> models = await _serviceFactory.CreateVehicleStandardValueService().GetMadeModel(searchString);
            return Json(models);
        }

        public async Task<JsonResult> GetBrandNames(string madeModel)
        {
            Console.WriteLine("here GetBrandNames api call............................." + madeModel);
            List<string> brands = await _serviceFactory.CreateVehicleStandardValueService().GetBrandNames(madeModel);
            brands = brands.OrderBy(m => m).ToList();
            return Json(brands);
        }

        public async Task<JsonResult> GetMadeModelYear(string madeModel)
        {
            Console.WriteLine("here year api call............................."+madeModel);
            List<string> models = await _serviceFactory.CreateVehicleStandardValueService().GetModelYear(madeModel);
            models = models.OrderBy(m => m).ToList();
            return Json(models);
        }

        public async Task<JsonResult> GetMadeModelYearByManufacturerAndBrand(string madeModel, string brandName)
        {
            Console.WriteLine("here year api call............................." + madeModel);
            List<string> models = await _serviceFactory.CreateVehicleStandardValueService().GetModelYear(madeModel, brandName);
            models = models.OrderBy(m => m).ToList();
            return Json(models);
        }
    }
}
