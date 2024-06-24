
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using VAVS_Client.Classes;
using VAVS_Client.Classes.TaxCalculation;
using VAVS_Client.Data;
using VAVS_Client.Factories;
using VAVS_Client.Util;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Services.Impl
{
    public class TaxCalculationServiceImpl : AbstractServiceImpl<TaxValidation>, TaxCalculationService
    {
        private readonly ILogger<TaxCalculationServiceImpl> _logger;
        private readonly HttpClient _httpClient;
        private readonly PersonalDetailService _personalDetailService;
        private readonly TownshipService _townshipService;
        private readonly StateDivisionService _staetDivisionService;
        private readonly VehicleStandardValueService _vehicleStandardValueService;
        private readonly FuelTypeService _fuelTypeService;
        private readonly LoginUserInfoDBService _taxPayerInfoService;
        private readonly FinancialYearService _financialYearService;
        public TaxCalculationServiceImpl(VAVSClientDBContext context, HttpClient httpClient, ILogger<TaxCalculationServiceImpl> logger, PersonalDetailService personalDetailService, TownshipService townshipService, StateDivisionService staetDivisionService, VehicleStandardValueService vehicleStandardValueService, FuelTypeService fuelTypeService, LoginUserInfoDBService taxPayerInfoService, FinancialYearService financialYearService) : base(context, logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _personalDetailService = personalDetailService;
            _townshipService = townshipService;
            _staetDivisionService = staetDivisionService;
            _vehicleStandardValueService = vehicleStandardValueService;
            _fuelTypeService = fuelTypeService;
            _taxPayerInfoService = taxPayerInfoService;
            _financialYearService = financialYearService;
        }

        public long CalculateTotalTax(long contractPrice, long assetValue)
        {
            long maximumValue = (contractPrice > assetValue) ? contractPrice : assetValue;
            VehicleTaxCalculation taxCalculation = new VehicleTaxCalculation();
            return taxCalculation.CalculateTax(maximumValue);
        }

        public async Task<bool> SaveTaxValidation(HttpContext httpContext, TaxInfo taxInfo1)
        {
            try
            {
                LoginUserInfo loginTaxPayerInfo = _taxPayerInfoService.GetLoginUserByHashedToken(SessionUtil.GetToken(httpContext));
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return false;
                /*
                 * Find PersonalDetail from Database and api
                 */
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                Township township = _townshipService.FindTownshipByPkId(personalDetail.TownshipPkid);
                
                if (personalDetail.PersonalPkid == 0)
                {
                     personalDetail.TownshipPkid = personalDetail.TownshipPkid;
                    personalDetail.StateDivisionPkid = personalDetail.StateDivisionPkid;
                    _personalDetailService.CreatePersonalDetail(personalDetail);
                }
                Console.WriteLine("here personalpkid not null.............." + personalDetail.PersonalPkid);
                var financialYear = _financialYearService.GetFinancialYear(personalDetail.EntryDate);
                string taxYear = null;
                if (financialYear != null)
                {
                    taxYear = financialYear["FinancialYear"].ToString();
                }

                TaxValidation taxValidation = new TaxValidation
                {
                    PersonTINNumber = personalDetail?.PersonTINNumber,
                    PersonNRC = loginUserInfo.NRC,
                    VehicleNumber = loginTaxPayerInfo.VehicleNumber,
                    Manufacturer = loginTaxPayerInfo.Manufacturer,
                    CountryOfMade = loginTaxPayerInfo.CountryOfMade,
                    VehicleBrand = loginTaxPayerInfo.VehicleBrand,
                    BuildType = loginTaxPayerInfo.BuildType,
                    ModelYear = loginTaxPayerInfo.ModelYear,
                    EnginePower = loginTaxPayerInfo.EnginePower,
                    FuelType = loginTaxPayerInfo.FuelType,
                    StandardValue = decimal.Parse(loginTaxPayerInfo.StandardValue),
                    ContractValue = decimal.Parse(loginTaxPayerInfo.ContractValue),
                    TaxAmount = decimal.Parse(loginTaxPayerInfo.TaxAmount),
                    TaxYear = taxYear,
                    PersonalDetail = personalDetail,
                    Township = township,
                    EntryDate = DateTime.Now,
                    IsDeleted = false,
                    CreatedBy = personalDetail.PersonalPkid,
                    CreatedDate = DateTime.Now,
                    //VehicleStandardValue = vehicleStandardValue
                };
                return Create(taxValidation);

            }
            catch (Exception e)
            {
                Console.WriteLine("Error occur when saving taxvalidation: " + e);
                return false;
            }
        }
    }
}
