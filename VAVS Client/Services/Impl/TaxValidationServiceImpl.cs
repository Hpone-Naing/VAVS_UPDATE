using System.Data;
using VAVS_Client.Classes;
using VAVS_Client.Data;
using VAVS_Client.Paging;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class TaxValidationServiceImpl : AbstractServiceImpl<TaxValidation>, TaxValidationService
    {

        private readonly ILogger<TaxValidationServiceImpl> _logger;
        private readonly SessionService _sessionService;
        public TaxValidationServiceImpl(VAVSClientDBContext context, ILogger<TaxValidationServiceImpl> logger, SessionService sessionService) : base(context, logger)
        {

            _logger = logger;
            _sessionService = sessionService;
        }

        public TaxValidation FindTaxValidationByNrc(string nrc)
        {
            _logger.LogInformation(">>>>>>>>>> [TaxValidationServiceImpl][FindTaxValidationByNrc] Find Taxed Vehicle by nrc. <<<<<<<<<<");

            try
            {
                TaxValidation taxValidation = FindByString("PersonNRC", nrc);
                _logger.LogInformation(">>>>>>>>>> Success. Find Taxed Vehicle by nrc. <<<<<<<<<<");
                return taxValidation;

            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when finding Taxed Vehicle by nrc. <<<<<<<<<<" + e);
                throw;
            }

        }

        public TaxValidation FindTaxValidationByIdEgerLoad(int id)
        {
            try
            {
                Console.WriteLine("here taxvalidation service impl id: " + id);
                TaxValidation taxValidation = _context.TaxValidations.Where(taxValidation => taxValidation.TaxValidationPkid == id && taxValidation.IsDeleted == false)
                    .Include(taxValidation => taxValidation.PersonalDetail)
                    .Include(taxValidation => taxValidation.PersonalDetail.Township)
                    .Include(taxValidation => taxValidation.PersonalDetail.Township.StateDivision)
                    .Include(taxValidation => taxValidation.Township)
                    .Include(taxValidation => taxValidation.Township.StateDivision)
                    .FirstOrDefault();
                Console.WriteLine("person nrc..........................." + (taxValidation==null));
                return taxValidation;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                throw;
            }

        }

        public PagingList<TaxValidation> GetTaxValidationPendigListPagin(HttpContext httpContext, int? pageNo, int PageSize)
        {
            try
            {
                try
                {
                    TaxpayerInfo loginTaxPayerInfo = _sessionService.GetLoginUserInfo(httpContext);

                    List<TaxValidation> taxValidationPendingList =  _context.TaxValidations.Where(taxValidation => taxValidation.PersonNRC == loginTaxPayerInfo.NRC && (taxValidation.QRCodeNumber == null && taxValidation.DemandNumber == null) && taxValidation.IsDeleted == false).ToList();
                    return GetAllWithPagin(taxValidationPendingList, pageNo, PageSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occur " + e);
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur. TaxValidation Pending  List paginate eger load list. <<<<<<<<<<" + e);
                throw;
            }
        }

        public PagingList<TaxValidation> GetTaxValidationPendigListForExcelPagin(HttpContext httpContext, int? pageNo, int PageSize)
        {
            try
            {
                try
                {
                    TaxpayerInfo loginTaxPayerInfo = _sessionService.GetLoginUserInfo(httpContext);

                    List<TaxValidation> taxValidationPendingList = _context.TaxValidations.Where(taxValidation => taxValidation.PersonNRC == loginTaxPayerInfo.NRC && (taxValidation.QRCodeNumber == null && taxValidation.DemandNumber == null) && taxValidation.IsDeleted == false)
                        .Include(taxValidation => taxValidation.PersonalDetail)
                        .Include(taxValidation => taxValidation.PersonalDetail.Township)
                        .Include(taxValidation => taxValidation.PersonalDetail.Township.StateDivision)
                        .ToList();
                    return GetAllWithPagin(taxValidationPendingList, pageNo, PageSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occur " + e);
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur. TaxValidation Pending  List paginate eger load list. <<<<<<<<<<" + e);
                throw;
            }
        }

        public  PagingList<TaxValidation> GetTaxValidationApprevedListPagin(HttpContext httpContext, int? pageNo, int PageSize)
        {
            try
            {
                try
                {
                    TaxpayerInfo loginTaxPayerInfo = _sessionService.GetLoginUserInfo(httpContext);
                    List<TaxValidation> taxValidationPendingList = _context.TaxValidations.Where(taxValidation => taxValidation.PersonNRC == loginTaxPayerInfo.NRC && (taxValidation.QRCodeNumber != null || taxValidation.DemandNumber != null) && taxValidation.IsDeleted == false).ToList();
                    return GetAllWithPagin(taxValidationPendingList, pageNo, PageSize);
                }
                catch (Exception e)
                {
                    _logger.LogError(" Error occur " + e);
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur. TaxValidation Pending  List paginate eger load list. <<<<<<<<<<" + e);
                throw;
            }
        }

        public PagingList<TaxValidation> GetTaxValidationApprevedListForExcelPagin(HttpContext httpContext, int? pageNo, int PageSize)
        {
            try
            {
                try
                {
                    TaxpayerInfo loginTaxPayerInfo = _sessionService.GetLoginUserInfo(httpContext);
                    List<TaxValidation> taxValidationApproveList = _context.TaxValidations.Where(taxValidation => taxValidation.PersonNRC == loginTaxPayerInfo.NRC && (taxValidation.QRCodeNumber != null || taxValidation.DemandNumber != null) && taxValidation.IsDeleted == false)
                        .Include(taxValidation => taxValidation.PersonalDetail)
                        .Include(taxValidation => taxValidation.PersonalDetail.Township)
                        .Include(taxValidation => taxValidation.PersonalDetail.Township.StateDivision)
                        .Include(taxValidation => taxValidation.PersonalDetail.Township)
                        .Include(taxValidation => taxValidation.PersonalDetail.Township.StateDivision)
                        .ToList();
                    return GetAllWithPagin(taxValidationApproveList, pageNo, PageSize);
                }
                catch (Exception e)
                {
                    _logger.LogError(" Error occur " + e);
                    throw;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur. TaxValidation Pending  List paginate eger load list. <<<<<<<<<<" + e);
                throw;
            }
        }

        public DataTable MakeVehicleDataExcelData(PagingList<TaxValidation> taxValidations, HttpContext httpContext)
        {
            _logger.LogInformation(">>>>>>>>>> [VehicleDataServiceImpl][MakeVehicleDataExcelData] Assign SearchAll or GetAll VehicleData list to dataTable. <<<<<<<<<<");
            DataTable dt = new DataTable("Call Center");
            dt.Columns.AddRange(new DataColumn[19] {
                                        new DataColumn("ပိုင်ရှင်အမည်"),
                                        new DataColumn("မှတ်ပုံတင်နံပါတ်"),
                                        new DataColumn("တိုင်းဒေသကြီး"),
                                        new DataColumn("မြို့နယ်"),
                                        new DataColumn("လိပ်စာ"),
                                        new DataColumn("ဖုန်ံးနံပါတ်"),
                                        new DataColumn("အီးမေးလ်"),
                                        new DataColumn("ကားနံပါတ်"),
                                        new DataColumn("ကားမော်ဒယ်"),
                                        new DataColumn("အမျိုးအစား"),
                                        new DataColumn("အင်ဂျင်ပါဝါ"),
                                        new DataColumn("ခန့်မှန်းတန်ဖိုး"),
                                        new DataColumn("ကျသင့်အခွန်"),
                                        new DataColumn("ပေးသွင်းသည့်အခွန်"),
                                        new DataColumn("ငွေလွှဲအိုင်ဒီ"),
                                        new DataColumn("ငွေလွှဲရက်စွဲ"),
                                        new DataColumn("ဖောင်နံပါတ်"),
                                        new DataColumn("QR နံပါတ်"),
                                        new DataColumn("Demand နံပါတ်"),
                                        });
            var list = new List<TaxValidation>();

                _logger.LogInformation(">>>>>>>>>> For export paginate or searchResult VehicleData list. <<<<<<<<<<");
                _logger.LogInformation(">>>>>>>>>> Get all paginate or searchResult VehicleData eger load list. <<<<<<<<<<");
                try
                {
                    _logger.LogInformation(">>>>>>>>>> Success. Get all paginate or searchResult VehicleData eger load list. <<<<<<<<<<");
                    list = taxValidations;
                }
                catch (Exception e)
                {
                    _logger.LogError(">>>>>>>>>> Error occur when getting all paginate or searchResult VehicleData eger load list. <<<<<<<<<<" + e);
                    throw;
                }
            
            try
            {
                _logger.LogInformation(">>>>>>>>>> Assign list to dataTable. <<<<<<<<<<");
                SessionService sessionService = new SessionServiceImpl();
                if (list.Count() > 0)
                {
                    foreach (var taxValidation in list)
                    {
                            dt.Rows.Add(sessionService.GetLoginUserInfo(httpContext).Name,
                            taxValidation.PersonNRC,
                            taxValidation.PersonalDetail.Township.StateDivision.StateDivisionName,
                            taxValidation.PersonalDetail.Township.TownshipName,
                            string.Concat(taxValidation.PersonalDetail.HousingNumber,"၊", taxValidation.PersonalDetail.Quarter, taxValidation.PersonalDetail.Street), 
                            taxValidation.PersonalDetail.PhoneNumber,
                            taxValidation.PersonalDetail.Email,
                            taxValidation.VehicleNumber, 
                            taxValidation.VehicleBrand,
                            taxValidation.Manufacturer,
                            taxValidation.EnginePower,
                            taxValidation.StandardValue,
                            taxValidation.TaxAmount,
                            taxValidation.TaxAmount,
                            "-",
                            "-",
                            "-", 
                            taxValidation.QRCodeNumber, 
                            taxValidation.DemandNumber);
                    }
                }
                _logger.LogInformation(">>>>>>>>>> Assign list to dataTable success. <<<<<<<<<<");
                return dt;
            }
            catch (Exception e)
            {
                _logger.LogError(">>>>>>>>>> Error occur when assigning SearchAll or GetAll VehicleData list to dataTable. <<<<<<<<<<" + e);
                throw;
            }
        }
    }
}
