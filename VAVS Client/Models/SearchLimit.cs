using System.Globalization;
using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_SearchLimit")]
    public class SearchLimit
    {
        [Key]
        public int SearchLimitPkid { get; set; }

        [StringLength(50)]
        public string? Nrc { get; set; }

        public int SearchCount { get; set; }

        [StringLength(200)]
        public string? ReSearchTime { get; set; }

        public bool IsExceedMaximunSearch() => (this.SearchCount >= Utility.MAXIMUM_SEARCH_TIME);

        /*public bool AllowNextTimeRegiste()
        {
            return (this.ReSearchTime != null && DateTime.Now >= DateTime.Parse(this.ReSearchTime));
        }*/
        public bool AllowNextTimeRegiste()
        {
            try
            {
                Console.WriteLine("here AllowNextTimePendingPayment............................");
                Console.WriteLine("ReSearchTime: " + this.ReSearchTime);
                Console.WriteLine("Date time now: " + DateTime.Now);
                if (DateTime.TryParseExact(this.ReSearchTime, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rePaymentTime))
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
            catch (Exception e)
            {
                Console.WriteLine("Exception ................." + e.ToString()); // Detailed exception output
                return false;
            }

        }

    }
}
