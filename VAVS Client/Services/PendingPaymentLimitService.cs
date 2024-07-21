namespace VAVS_Client.Services
{
    public interface PendingPaymentLimitService
    {
        public bool CreatePendingPaymentLimit(PendingPaymentLimit pendingPaymentLimit);
        public PendingPaymentLimit GetPendingPaymentLimitByNrc(string nrc);
        public bool HardDeletePendingPaymentLimit(string nrc);
        public bool UpdatePendingPaymentLimit(string nrc);
        public bool UpdatePendingPaymentLimitAfterMakePayment(string nrc);
    }
}
