using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class OpportunityRepository : BaseRepository, IOpportunityRepository
    {
        public OpportunityRepository(string connectionString) : base(connectionString)
        {
        }
        public Systemjobdata Getsystemallopportuntydata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systemjobdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemallopportuntydata", parameters, commandType: CommandType.StoredProcedure);
                string systemjobdatadataJson = parameters.Get<string>("@Systemjobdata");
                if (systemjobdatadataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemjobdata>(systemjobdatadataJson);
                }
                else
                {
                    return new Systemjobdata();
                }
            }
        }
        public Genericmodel Registersystemopportuntydata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemopportuntydata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SystemJob Getsystemopportuntydatabyid(long Opportunityid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Opportunityid", Opportunityid);
                parameters.Add("@Systemjobdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemopportuntydatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systemjobdataJson = parameters.Get<string>("@Systemjobdata");
                if (systemjobdataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemJob>(systemjobdataJson);
                }
                else
                {
                    return new SystemJob();
                }
            }
        }
        public IEnumerable<SystemJob> Getsystemallunpublishedopportunitydata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                return connection.Query<SystemJob>("Usp_Getsystemallunpublishedblogdata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public Genericmodel Updatepublishedopportunitydata(long Opportunityid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Opportunityid", Opportunityid);
                return connection.Query<Genericmodel>("Usp_Updatepublishedopportunitydata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}
