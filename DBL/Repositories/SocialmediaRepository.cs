using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class SocialmediaRepository : BaseRepository, ISocialmediaRepository
    {
        public SocialmediaRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<SocialMediaSettings> Getsysteusersocialmediadata(long Userid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Userid", Userid);
                return connection.Query<SocialMediaSettings>("Usp_Getsystemsocialmediadata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersystemsocialmediapagedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemsocialmediapagedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SocialMediaSettings Getsysteusersocialmediadatabyid(long Socialsettingid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Socialsettingid", Socialsettingid);
                return connection.Query<SocialMediaSettings>("Usp_Getsystemsocialmediadatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
