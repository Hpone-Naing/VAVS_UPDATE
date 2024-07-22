using System.Globalization;
using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_LoginAuth")]
    public class LoginAuth
    {

        [Key]
        public int LoginAuthId { get; set; }

        [StringLength(200)]
        public string? Nrc { get; set; }

        [StringLength(200)]
        public string? PhoneNumber { get; set; }
        public int ResendOTPCount { get; set; }

        [StringLength(200)]
        public string? ReResendCodeTime { get; set; }

        [StringLength(200)]
        public string? OTP { get; set; }

        public bool IsExceedMaximunResendCode() => (this.ResendOTPCount >= Utility.MAXIMUM_RESEND_CODE_TIME);
        public bool AllowNextTimeResendOTP()
        {
            try
            {
                Console.WriteLine("here AllowNextTimePendingPayment............................");
                Console.WriteLine("ReSearchTime: " + this.ReResendCodeTime);
                Console.WriteLine("Date time now: " + DateTime.Now);
                if (DateTime.TryParseExact(this.ReResendCodeTime, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rePaymentTime))
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
                Console.WriteLine("Exception ................." + e.ToString()); 
                return false;
            }
            //return (DateTime.Now >= DateTime.Parse(this.ReResendCodeTime));
        }
    }
}
