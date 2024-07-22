using VAVS_Client.Data;
using VAVS_Client.Util;

namespace VAVS_Client.Services.Impl
{
    public class PendingPaymentLimitServiceImpl : AbstractServiceImpl<PendingPaymentLimit>, PendingPaymentLimitService
    {
        private readonly ILogger<PendingPaymentLimitServiceImpl> _logger;

        public PendingPaymentLimitServiceImpl(VAVSClientDBContext context, ILogger<PendingPaymentLimitServiceImpl> logger) : base(context, logger)
        {
            _logger = logger;
        }

        public bool CreatePendingPaymentLimit(PendingPaymentLimit pendingPaymentLimit)
        {
            try
            {
                return Create(pendingPaymentLimit);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while save" + ex);
                throw;
            }
        }

        public PendingPaymentLimit GetPendingPaymentLimitByNrc(string nrc)
        {
            return FindByString("Nrc", nrc);
        }
        public bool HardDeletePendingPaymentLimit(string nrc)
        {
            try
            {
                PendingPaymentLimit searchlimit = GetPendingPaymentLimitByNrc(nrc);
                if (searchlimit != null)
                {
                    return HardDelete(searchlimit);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while dekete" + ex);
                throw;
            }
        }

        public bool HardDeletePendingPaymentLimit(PendingPaymentLimit pendingPaymentLimit)
        {
            try
            {
                    return HardDelete(pendingPaymentLimit);
                
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while dekete" + ex);
                throw;
            }
        }

        public bool UpdatePendingPaymentLimit(string nrc)
        {
            try
            {
                PendingPaymentLimit pendingPaymentLimit = GetPendingPaymentLimitByNrc(nrc);
                if (pendingPaymentLimit.IsExceedMaximun())
                {
                    Console.WriteLine("Here is exceed max........................................................................../");
                    if (pendingPaymentLimit.LimitTime != null && pendingPaymentLimit.AllowNextTimePendingPayment())
                    {
                        Console.WriteLine("Here is limittime != null and allow next time............................................................../");

                        return HardDeletePendingPaymentLimit(pendingPaymentLimit);
                    }
                    Console.WriteLine("Here is limit time null......................................................................../");

                    pendingPaymentLimit.LimitTime = DateTime.Now.AddDays(Utility.NEXT_PENDINGPAYMENT_TIME_IN_DAYS).ToString("dd/MM/yyyy hh:mm:ss tt");
                    return Update(pendingPaymentLimit);
                }
                Console.WriteLine("Here is not exceed max........................................................../");

                pendingPaymentLimit.Count++;
                return Update(pendingPaymentLimit);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while update" + ex);
                throw;
            }
        }
        public bool UpdatePendingPaymentLimitAfterMakePayment(string nrc)
        {
            try
            {
                PendingPaymentLimit pendingPaymentLimit = GetPendingPaymentLimitByNrc(nrc);
                if(pendingPaymentLimit != null)
                {
                    if(pendingPaymentLimit.Count < 2)
                    {
                        return HardDeletePendingPaymentLimit(pendingPaymentLimit);
                    }
                    pendingPaymentLimit.LimitTime = null;
                    --pendingPaymentLimit.Count;
                    return Update(pendingPaymentLimit);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occur while update" + ex);
                throw;
            }
        }

    }
}
