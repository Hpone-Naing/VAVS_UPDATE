using System.Globalization;
using VAVS_Client.Util;

namespace VAVS_Client.Models
{
    [Table("TB_DeviceInfo")]
    public class DeviceInfo
    {
        [Key]
        public int DeviceInfoId { get; set; }

        [StringLength(200)]
        public string? IpAddress { get; set; }

        [StringLength(200)]
        public string? PublicIpAddress { get; set; }
        public int RegistrationCount { get; set; }
        public int ResendCodeTime { get; set; }

        [StringLength(200)]
        public string? ReRegistrationTime { get; set; }

        [StringLength(200)]
        public string? ReResendCodeTime { get; set; }

        [StringLength(200)]
        public string? OTP { get; set; }

        public bool IsSameIp(string ipAddress) => (this.IpAddress == ipAddress);

        public bool IsExceedMaximunRegistration() => (this.RegistrationCount >= Utility.MAXIMUM_REGISTRATION_TIME);
        public bool IsExceedMaximunResendCode() => (this.ResendCodeTime >= Utility.MAXIMUM_RESEND_CODE_TIME);
        //public bool AllowNextTimeRegister() => (DateTime.Now >= DateTime.Parse(this.ReRegistrationTime));
        //public bool AllowNextTimeResendOTP() => (DateTime.Now >= DateTime.Parse(ReResendCodeTime));
        public bool AllowNextTimeRegister()
        {
            try
            {
                Console.WriteLine("here AllowNextTimePendingPayment............................");
                Console.WriteLine("ReSearchTime: " + this.ReRegistrationTime);
                Console.WriteLine("Date time now: " + DateTime.Now);
                if (DateTime.TryParseExact(this.ReRegistrationTime, "dd/MM/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime rePaymentTime))
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
            //return (DateTime.Now >= DateTime.Parse(this.ReRegistrationTime));
        }

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
