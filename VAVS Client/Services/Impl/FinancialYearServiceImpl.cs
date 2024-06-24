using Microsoft.Data.SqlClient;
using System.Data;

namespace VAVS_Client.Services.Impl
{
    public class FinancialYearServiceImpl : FinancialYearService
    {
        private readonly string _connectionString;

        public FinancialYearServiceImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("VAVSClientDBContext");
        }
        public DataRow GetFinancialYear(DateTime? entryDate)
        {
            Console.WriteLine("Entry date.................." + entryDate);
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM TB_FinancialYear WHERE @EntryDate BETWEEN FinancialStartDate AND FinancialEndDate", connection))
                {
                    command.Parameters.Add(new SqlParameter("@EntryDate", SqlDbType.DateTime) { Value = entryDate });

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        if (dataTable.Rows.Count > 0)
                        {
                            return dataTable.Rows[0];
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

    }
}
