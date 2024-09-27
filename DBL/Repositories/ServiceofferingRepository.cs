using Dapper;
using DBL.Entities;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class ServiceofferingRepository : BaseRepository, IServiceofferingRepository
    {
        public ServiceofferingRepository(string connectionString) : base(connectionString)
        {
        }
        public ServiceOfferings Getsystemserviceofferingdatabyid(long Serviceid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Serviceid", Serviceid);
                return connection.Query<ServiceOfferings>("Usp_Getsystemserviceofferingdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
