namespace VAVS_Client.Classes.TaxCalculation
{
    public class VehicleTaxCalculation
    {
        private bool IsLessThanThreeHundredMillion(long value)
        {
            return value <= 300000000;
        }

        private bool IsBetweenThreeHundredMillionOneAndSixHundredMillion(long value)
        {
            return value > 300000000 && value <= 600000000;
        }

        private bool IsBetweenSixHundredMillionOneAndOneBillion(long value)
        {
            return value > 600000000 && value <= 1000000000;
        }

        private bool IsBetweenOneBillionOneAndThreeBillion(long value)
        {
            return value > 1000000000 && value <= 3000000000;
        }

        public long CalculateTax(long value)
        {
            if (value < 1)
                return value;
            if (IsLessThanThreeHundredMillion(value))
            {
                return (long)(value * 0.03);
            }
            if(IsBetweenThreeHundredMillionOneAndSixHundredMillion(value))
            {
                return (long)((300000000 * 0.03) + (long)((value - 300000000) * 0.05));
            }
            if(IsBetweenSixHundredMillionOneAndOneBillion(value))
            {
                return (long)((300000000 * 0.03) + (long)(300000000 * 0.05) + (long)((value - 600000000) * 0.1));
            }
            if(IsBetweenOneBillionOneAndThreeBillion(value))
            {
                return (long)((300000000 * 0.03) + (long)(300000000 * 0.05) + (long)(400000000 * 0.1) + (long)((value - 1000000000) * 0.15));
            }
            return (long)((300000000 * 0.03) + (long)(300000000 * 0.05) + (long)(400000000 * 0.1) + (long)(2000000000 * 0.15) + (long)((value - 3000000000) * 0.3));
        }
        
    }
}
