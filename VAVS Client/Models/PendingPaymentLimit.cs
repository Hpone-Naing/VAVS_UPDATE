using System.Globalization;
using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_PendingPaymentLimit")]
    public class PendingPaymentLimit
    {
        [Key]
        public int PendingPaymentLimitPkid { get; set; }

        [StringLength(50)]
        public string? Nrc { get; set; }

        public int Count { get; set; }

        [StringLength(200)]
        public string? LimitTime { get; set; }

        public bool IsExceedMaximun() => (this.Count >= Utility.MAXIMUM_PENDINGPAYMENT_TIME);

        /*public bool AllowNextTimeRegiste()
        {
            return (this.LimitTime != null && DateTime.Now >= DateTime.Parse(this.LimitTime));
        }*/
        public bool AllowNextTimePendingPayment()
        {
            Console.WriteLine("here AllowNextTimePendingPayment............................");
            if (DateTime.TryParseExact(this.LimitTime, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rePaymentTime))
            {
                Console.WriteLine("here if....................");
                Console.WriteLine("  > ...................." + (DateTime.Now >= rePaymentTime) + " Now / time" + DateTime.Now + " / " + rePaymentTime);
                return DateTime.Now >= rePaymentTime;
            }
            else
            {
                Console.WriteLine("here else");
                return false;
            }
        }



    }
}
