using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class PropertyRepository : BaseRepository, IPropertyRepository
    {
        public PropertyRepository(string connectionString) : base(connectionString)
        {
        }

        public Genericmodel Registersystempropertyhousedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystempropertyhousedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systemproperty Getsystempropertyhousedatabyid(long Propertyid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Propertyid", Propertyid);
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertydata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemproperty>(systempropertydataJson);
                }
                else
                {
                    return new Systemproperty();
                }
            }
        }
    }
}