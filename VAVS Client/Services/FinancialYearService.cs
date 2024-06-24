using System.Data;

namespace VAVS_Client.Services
{
    public interface FinancialYearService
    {
        public DataRow GetFinancialYear(DateTime? entryDate);
    }
}
