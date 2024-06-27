using Dapper;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data.SqlClient;
using System.Data;

namespace DBL.Repositories
{
    public class BlogsRepository : BaseRepository, IBlogsRepository
    {
        public BlogsRepository(string connectionString) : base(connectionString)
        {
        }
        public Genericmodel Registersystemblogdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_InsertArticlesAndSourcesdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
