using Dapper;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        #region System Emails
        public Monthlyrentinvoicedata Getsystemunsentemaildata()
        {

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                Monthlyrentinvoicedata resp = new Monthlyrentinvoicedata();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RentinvoiceDetails", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemunsentemaildata", parameters, commandType: CommandType.StoredProcedure);
                string detailsJson = parameters.Get<string>("@RentinvoiceDetails");
                JObject responseJson = JObject.Parse(detailsJson);
                string userModelJson = responseJson["Data"].ToString();
                resp.Data = JsonConvert.DeserializeObject<List<Monthlyrentinvoice>>(userModelJson);
                return resp;
            }
        }
        #endregion
    }
}
