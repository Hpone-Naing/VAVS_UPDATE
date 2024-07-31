namespace VAVS_Client.Services
{
    public interface PaymentService
    {
        public Task<Payment> FindPaymentById(int pkId);
        public Task<Payment> FindPaymentByIdEgerLoad(int pkId);
        public Task<Payment> FindPaymentByVAVATransactionNumberEgerLoad(string trasNo);
        public Task<Payment> CretePayment(HttpContext httpContext);
        public Task<List<Payment>> GetRemainPaymentList(HttpContext httpContext);
        public Task<bool> FindPaymentByTaxValidationWithPendingStatus(int taxValidationPkId);
        public Task<bool> IsALreadyPayment(HttpContext httpContext, string vehicleNumber);
        public Task<bool> IsALreadyPendingPayment(HttpContext httpContext, string vehicleNumber);
        public Task<bool> UpdatePaymentStatus(string transactionNumber);
        public Task<List<Payment>> GetApprovePaymentList(HttpContext httpContext);
        public Task<List<Payment>> GetApprovePaymentListEgerLoad(HttpContext httpContext);
        public Task<List<TaxValidation>> GetTaxValidationListByPendingPayment(HttpContext httpContext);
        public Task<List<TaxValidation>> GetTaxValidationEgerLoadListByPendingPayment(HttpContext httpContext);
        public Task<bool> HardDeletePendingStatusPayments(HttpContext httpContext);
        public Task<int> GetPendingPaymentCount(HttpContext httpContext);
        public Task<int> GetApprovePaymentCount(HttpContext httpContext);
        public Task<int> GetTaxValidationCountByApprovePayment(HttpContext httpContext);
    }
}
