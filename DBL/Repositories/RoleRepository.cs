using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(string connectionString) : base(connectionString)
        {
        }
        public Genericmodel Registersystemroledata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemroledata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<SystemRole> Getsystemroledata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<SystemRole>("Usp_Getsystemroledata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public SystemRole Getsystemroledatabyid(long Roleid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Roleid", Roleid);
                parameters.Add("@Systemroledata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemroledatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systemroledataJson = parameters.Get<string>("@Systemroledata");
                if (systemroledataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemRole>(systemroledataJson);
                }
                else
                {
                    return new SystemRole();
                }
            }
        }
    }
}
