using DocumentFormat.OpenXml.Office2010.ExcelAc;
using VAVS_Client.Classes;
using VAVS_Client.Data;
using VAVS_Client.Models;
using VAVS_Client.Util;
using LoginUserInfo = VAVS_Client.Models.LoginUserInfo;

namespace VAVS_Client.Services.Impl
{
    public class PaymentServiceImpl : AbstractServiceImpl<Payment>, PaymentService
    {
        private readonly ILogger<PaymentServiceImpl> _logger;
        private readonly PersonalDetailService _personalDetailService;
        private readonly TownshipService _townshipService;
        private readonly LoginUserInfoDBService _taxPayerInfoService;
        private readonly FinancialYearService _financialYearService;
        private readonly TaxCalculationService _taxCalculationService;
        private readonly TaxPersonImageService _taxPersonImageService;

        public PaymentServiceImpl(VAVSClientDBContext context, ILogger<PaymentServiceImpl> logger, PersonalDetailService personalDetailService, TownshipService townshipService, LoginUserInfoDBService loginUserInfoService, FinancialYearService financialYearService, TaxCalculationService taxCalculationService, TaxPersonImageService taxPersonImageService) : base(context, logger)
        {
            _logger = logger;
            _personalDetailService = personalDetailService;
            _townshipService = townshipService;
            _taxPayerInfoService = loginUserInfoService;
            _financialYearService = financialYearService;
            _taxCalculationService = taxCalculationService;
            _taxPersonImageService = taxPersonImageService;
        }

        public async Task<Payment> FindPaymentById(int pkId)
        {
            try
            {
                if(pkId != 0)
                {
                    Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PaymentPkid == pkId).FirstOrDefaultAsync();
                    return payment;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur FindPaymentById: " + e);
                throw;
            }
        }
        public async Task<Payment> FindPaymentByIdEgerLoad(int pkId)
        {
            try
            {
                if (pkId != 0)
                {
                    Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PaymentPkid == pkId).Include(payment => payment.PersonalDetail).Include(payment => payment.PersonalDetail.Township).Include(payment => payment.PersonalDetail.StateDivision).Include(payment => payment.TaxValidation).FirstOrDefaultAsync();
                    return payment;
                }
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur FindPaymentById: " + e);
                throw;
            }
        }


        public async Task<Payment> CretePayment(HttpContext httpContext)
        {
            try
            {

                try
                {
                    LoginUserInfo loginTaxPayerInfo = _taxPayerInfoService.GetLoginUserByHashedToken(SessionUtil.GetToken(httpContext));
                    SessionService sessionService = new SessionServiceImpl();
                    TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                    if (loginUserInfo == null)
                        return null;
                    /*
                     * Find PersonalDetail from Database and api
                     */
                    PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                    Township township = _townshipService.FindTownshipByPkId(personalDetail.TownshipPkid);
                    
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
                    };
                    if(_taxCalculationService.SaveTaxValidation(taxValidation))
                    {
                        return CreateAndReturnObj(new Payment()
                        {
                            TransactionNumber = Utility.UniqueTransactionNumber(),
                            TransactionDate = DateTime.Now,
                            TaxAmount = decimal.Parse(loginTaxPayerInfo.TaxAmount),
                            PaidAmount = 0,
                            BalanceAmount = 0,
                            TaxType = "Income Tax",
                            AccountTitle = "မော်တော်ယာဥ်ဝယ်ယူခြင်း",
                            PaymentType = "Balance Due with Return",
                            IncomeYear = taxYear,
                            PaymentStatus = "Pending",
                            IsDeleted = false,
                            TaxValidation = taxValidation,
                            PersonalDetail = personalDetail,
                        });
                    }
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occur when saving taxvalidation: " + e);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occur when save payment." + ex);
                throw;
            }
        }

        public async Task<int> GetPaymentCount(HttpContext httpContext, string paymentStatus)
        {
            try
            {
                SessionService _sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = _sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                {
                    _logger.LogWarning("Login user info is null.");
                    return 0;
                }

                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                {
                    _logger.LogWarning("Personal detail is null for NRC: {NRC}", loginUserInfo.NRC);
                    return 0;
                }

                int count = await _context.Payments
                    .Where(payment => payment.IsDeleted == false && payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == paymentStatus)
                    .CountAsync();

                _logger.LogInformation("Retrieved payment count successfully for status: {PaymentStatus}", paymentStatus);
                return count;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred when retrieving payment count for status {PaymentStatus}: {Error}", paymentStatus, ex);
                throw;
            }
        }

        public async Task<int> GetPendingPaymentCount(HttpContext httpContext)
        {
            return await GetPaymentCount(httpContext, "Pending");
        }

        public async Task<int> GetApprovePaymentCount(HttpContext httpContext)
        {
            return await GetPaymentCount(httpContext, "Approve");
        }

        public async Task<List<Payment>> GetRemainPaymentList(HttpContext httpContext)
        {
            try
            {
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if(personalDetail == null)
                    return null;
                List<Payment> remainingPaymentList = _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Pending").Include(payment => payment.PersonalDetail).Include(payment => payment.TaxValidation).ToList() ;
                _logger.LogInformation("Get all remaining payment list success.");
                return remainingPaymentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occur when save payment." + ex);
                throw;
            }
        }

        public async Task<List<Payment>> GetRemainPaymentListTaxValidationEgerLoad(HttpContext httpContext)
        {
            try
            {
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                    return null;
                List<Payment> remainingPaymentList = _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Pending").Include(payment => payment.TaxValidation).ToList();
                _logger.LogInformation("Get all remaining payment list success.");
                return remainingPaymentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occur when save payment." + ex);
                throw;
            }
        }

        public async Task<bool> FindPaymentByTaxValidationWithPendingStatus(int taxValidationPkId)
        {
            Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.TaxValidationPkid == taxValidationPkId && payment.PaymentStatus == "Pending").FirstOrDefaultAsync();
            if (payment != null)
                return true;
            return false;
        }

        public async Task<bool> HardDeletePendingStatusPayments(HttpContext httpContext)
        {
            try
            {
                List<Payment> payments = await GetRemainPaymentList(httpContext);
                if (payments.Count >= 3)
                {
                    foreach (Payment payment in payments)
                    {
                        HardDelete(payment);
                        _taxPersonImageService.HardDeleteTaxPersonImage(payment.PersonalDetail.PersonalPkid, payment.TaxValidation.VehicleNumber);
                        _taxCalculationService.HardDeleteTaxCalculation(payment.TaxValidation);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex) 
            {
                _logger.LogError("Exception occur when hard delete pending status payments." + ex);
                throw;
            }
        }

        public async Task<List<Payment>> GetApprovePaymentList(HttpContext httpContext)
        {
            try
            {
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                    return null;
                List<Payment> approvePaymentList = _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Approve").ToList();
                return approvePaymentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occur GetApprovePaymentList." + ex);
                throw;
            }
        }

        public async Task<List<Payment>> GetApprovePaymentListEgerLoad(HttpContext httpContext)
        {
            try
            {
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                    return null;
                List<Payment> approvePaymentList = _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Approve").Include(payment => payment.PersonalDetail).Include(payment => payment.TaxValidation).ToList();
                _logger.LogInformation("Get all remaining payment list success.");
                return approvePaymentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occur GetApprovePaymentList." + ex);
                throw;
            }
        }

        public async Task<List<Payment>> GetApprovePaymentListWithTaxValidationEgerLoad(HttpContext httpContext)
        {
            try
            {
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                    return null;
                List<Payment> approvePaymentList = _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Approve").Include(payment => payment.PersonalDetail).Include(payment => payment.TaxValidation).ToList();
                _logger.LogInformation("Get all remaining payment list success.");
                return approvePaymentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occur GetApprovePaymentList." + ex);
                throw;
            }
        }

        public async Task<List<Payment>> GetApprovePaymentWithTaxValidationEgerLoad1(HttpContext httpContext)
        {
            try
            {
                SessionService _sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = _sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                {
                    _logger.LogWarning("Login user info is null.");
                    return null;
                }

                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                {
                    _logger.LogWarning("Personal detail is null for NRC: {NRC}", loginUserInfo.NRC);
                    return null;
                }

                List<Payment> approvePaymentList = await _context.Payments
                    .Where(payment => payment.IsDeleted == false && payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Approve")
                    .Include(payment => payment.TaxValidation)
                    .ToListAsync();

                _logger.LogInformation("Retrieved all approved payment list successfully.");
                return approvePaymentList;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred in GetApprovePaymentWithTaxValidationEgerLoad1: {Error}", ex);
                throw;
            }
        }


        public async Task<Payment> FindPaymentByNrcWithApproveStatus(HttpContext httpContext)
        {
            try
            {

                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                    return null;
                Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Approve").FirstOrDefaultAsync();
                return payment;
            }
            catch(Exception e)
            {
                _logger.LogError("Exception occur when FindPaymentByNrcWithApproveStatus." + e);
                throw;
            }
        }

        public async Task<Payment> FindPaymentByNrcWithApproveStatusEgerLoad(HttpContext httpContext)
        {
            try
            {
                SessionService sessionService = new SessionServiceImpl();
                TaxpayerInfo loginUserInfo = sessionService.GetLoginUserInfo(httpContext);
                if (loginUserInfo == null)
                    return null;
                PersonalDetail personalDetail = await _personalDetailService.GetPersonalInformationByNRCInDBAndAPI(loginUserInfo.NRC);
                if (personalDetail == null)
                    return null;
                Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.PersonalPkid == personalDetail.PersonalPkid && payment.PaymentStatus == "Approve").Include(payment => payment.TaxValidation).FirstOrDefaultAsync();
                return payment;
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occur when FindPaymentByNrcWithApproveStatus." + e);
                throw;
            }
        }

        public async Task<bool> IsALreadyPayment(HttpContext httpContext, string vehicleNumber)
        {
            try
            {
                List<Payment> payments = await GetApprovePaymentListWithTaxValidationEgerLoad(httpContext);
                return payments.Any(payment => payment.TaxValidation.VehicleNumber == vehicleNumber);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occur when IsALreadyPayment." + e);
                throw;
            }
        }

        public async Task<bool> IsALreadyPendingPayment(HttpContext httpContext, string vehicleNumber)
        {
            try
            {
                List<Payment> payments = await GetRemainPaymentListTaxValidationEgerLoad(httpContext);
                return payments.Any(payment => payment.TaxValidation.VehicleNumber == vehicleNumber);
            }
            catch (Exception e)
            {
                _logger.LogError("Exception occur when IsALreadyPayment." + e);
                throw;
            }
        }

        public async Task<Payment> FindPaymentByVAVATransactionNumber(string trasNo)
        {
            try
            {
                Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.TransactionNumber == trasNo).FirstOrDefaultAsync();
                return payment;                
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur FindPaymentByVAVATransactionNumber: " + e);
                throw;
            }
        }

        public async Task<Payment> FindPaymentByVAVATransactionNumberEgerLoad(string trasNo)
        {
            try
            {
                Payment payment = await _context.Payments.Where(payment => payment.IsDeleted == false).Where(payment => payment.TransactionNumber == trasNo).Include(payment => payment.TaxValidation).FirstOrDefaultAsync();
                return payment;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur FindPaymentByVAVATransactionNumber: " + e);
                throw;
            }
        }

        public async Task<bool> UpdatePaymentStatus(string transactionNumber)
        {
            try
            {
                Payment payment = await FindPaymentByVAVATransactionNumber(transactionNumber);
                if(payment != null)
                {
                    payment.PaymentStatus = "Approve";
                    Update(payment);
                }
                return false;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur UpdatePaymentStatus: " + e);
                throw;
            }
        }

        public async Task<int> GetTaxValidationCountByPendingPayment(HttpContext httpContext)
        {
            try
            {
                List<Payment> payments = await GetApprovePaymentWithTaxValidationEgerLoad1(httpContext);
                int pendingTaxValidationCount = payments
                .Select(payment => payment.TaxValidation).Where(taxValidation => taxValidation.IsDeleted == false && taxValidation.QRCodeNumber == null &&
                taxValidation.DemandNumber == null)
                .Count();
                _logger.LogInformation("Retrieved taxValidation list successfully.");
                return pendingTaxValidationCount;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur UpdatePaymentStatus: " + e);
                throw;
            }
        }

        public async Task<int> GetTaxValidationCountByApprovePayment(HttpContext httpContext)
        {
            try
            {
                var payments = await GetApprovePaymentWithTaxValidationEgerLoad1(httpContext);

                if (payments == null)
                {
                    _logger.LogWarning("No approved payments found.");
                    return 0;
                }

                // Filter and count the pending TaxValidations directly
                int pendingTaxValidationCount = payments
                    .Select(payment => payment.TaxValidation)
                    .Count(taxValidation => taxValidation.IsDeleted == false
                        && taxValidation.QRCodeNumber == null
                        && taxValidation.DemandNumber == null);

                _logger.LogInformation("Retrieved taxValidation list successfully. Pending count: {Count}", pendingTaxValidationCount);

                return pendingTaxValidationCount;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occurred in GetTaxValidationCountByApprovePayment: {Error}", e);
                throw;
            }
        }


        public async Task<List<TaxValidation>> GetTaxValidationListByPendingPayment(HttpContext httpContext)
        {
            try
            {
                List<Payment> payments = await GetApprovePaymentListWithTaxValidationEgerLoad(httpContext);
                List<TaxValidation> taxValidationList = payments
                .Select(payment => payment.TaxValidation).Where(taxValidation => taxValidation.IsDeleted == false && taxValidation.QRCodeNumber == null &&
                taxValidation.DemandNumber == null)
                .ToList();
                _logger.LogInformation("Retrieved taxValidation list successfully.");
                return taxValidationList;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur UpdatePaymentStatus: " + e);
                throw;
            }
        }

        public async Task<List<TaxValidation>> GetTaxValidationEgerLoadListByPendingPayment(HttpContext httpContext)
        {
            try
            {
                List<Payment> payments = await GetApprovePaymentListEgerLoad(httpContext);

                if (payments == null)
                    return null;

               List<TaxValidation> taxValidationList = await _context.TaxValidations
                        .Include(taxValidation => taxValidation.PersonalDetail)
                        .ThenInclude(personalDetail => personalDetail.Township)
                        .ThenInclude(township => township.StateDivision)
                    .Where(taxValidation => taxValidation.IsDeleted == false && payments.Any(payment => payment.TaxValidationPkid == taxValidation.TaxValidationPkid))
                    .ToListAsync();

                _logger.LogInformation("Retrieved taxValidation list successfully.");
                return taxValidationList;
            }
            catch (Exception e)
            {
                _logger.LogError("Error occur UpdatePaymentStatus: " + e);
                throw;
            }
        }

    }
}
