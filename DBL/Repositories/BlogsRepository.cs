using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class BlogsRepository : BaseRepository, IBlogsRepository
    {
        public BlogsRepository(string connectionString) : base(connectionString)
        {
        }
        public Systemblogdata Getsystemallblogdata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systemblogdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemblogdata", parameters, commandType: CommandType.StoredProcedure);
                string systemblogdataJson = parameters.Get<string>("@Systemblogdata");
                if (systemblogdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemblogdata>(systemblogdataJson);
                }
                else
                {
                    return new Systemblogdata();
                }
            }
        }
        public Genericmodel Registersystemblogdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemblogdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systemblog Getsystemblogdatabyid(long Blogid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Blogid", Blogid);
                parameters.Add("@Systemblogdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemblogdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systemblogdataJson = parameters.Get<string>("@Systemblogdata");
                if (systemblogdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemblog>(systemblogdataJson);
                }
                else
                {
                    return new Systemblog();
                }
            }
        }
        public Genericmodel Registersystemserverblogdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerarticlesandsourcesdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public IEnumerable<Newsapiarticles> Getsystemblogsdata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<Newsapiarticles>("Usp_Getsystemblogsdata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public IEnumerable<Newsapiarticles> Getsystemallunpublishedblogdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                return connection.Query<Newsapiarticles>("Usp_Getsystemallunpublishedblogdata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
