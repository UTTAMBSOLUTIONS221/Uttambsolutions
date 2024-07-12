using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class BlogcategoryRepository : BaseRepository, IBlogcategoryRepository
    {
        public BlogcategoryRepository(string connectionString) : base(connectionString)
        {
        }
        public Genericmodel Registersystemblogcategorydata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemblogcategorydata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<Systemblogcategories> Getsystemblogcategorydata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<Systemblogcategories>("Usp_Getsystemblogcategorydata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Systemblogcategories Getsystemblogcategorydatabyid(long Blogcategoryid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Blogcategoryid", Blogcategoryid);
                return connection.Query<Systemblogcategories>("Usp_Getsystemblogcategorydatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
