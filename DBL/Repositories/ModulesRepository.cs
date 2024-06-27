using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class ModulesRepository : BaseRepository, IModulesRepository
    {

        public ModulesRepository(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<Systemmodule> Getsystemmoduledata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Systemmodule>("Usp_Getsystemmoduledata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersystemmoduledata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemmoduledata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systemmodule Getsystemmoduledatabyid(long Moduleid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Moduleid", Moduleid);
                return connection.Query<Systemmodule>("Usp_Getsystemmoduledatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
