using Dapper;
using DBL.Enum;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data.SqlClient;
using System.Data;

namespace DBL.Repositories
{
    public class GeneralRepository : BaseRepository, IGeneralRepository
    {
        public GeneralRepository(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<ListModel> GetListModel(ListModelType listType)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Type", (int)listType);
                return connection.Query<ListModel>("Usp_GetListModel", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public IEnumerable<ListModel> GetListModelbycode(ListModelType listType, long code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Type", (int)listType);
                parameters.Add("@Code", code);
                return connection.Query<ListModel>("Usp_GetListModelbycode", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
