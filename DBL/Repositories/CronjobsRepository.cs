using Dapper;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class CronjobsRepository : BaseRepository, ICronjobsRepository
    {
        public CronjobsRepository(string connectionString) : base(connectionString)
        {
        }
        #region Generate Monthly Invoices
        public Genericmodel Generatemonthlyrentinvoicedata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Genericmodel>("Usp_Generatemonthlyrentinvoicedata", null, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion
    }
}
