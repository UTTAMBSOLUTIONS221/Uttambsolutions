using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class PropertyRepository : BaseRepository, IPropertyRepository
    {
        public PropertyRepository(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<Systemproperty> Getsystempropertyhousedata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                return connection.Query<Systemproperty>("Usp_Getsystempropertyhousedata", null, commandType: CommandType.StoredProcedure).ToList();
            }
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
                parameters.Add("@Propertyhouseid", Propertyid);
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
        public Systempropertyhousedata Getsystempropertyhousedatabyowner(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedatabyownerid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertydata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systempropertyhousedata>(systempropertydataJson);
                }
                else
                {
                    return new Systempropertyhousedata();
                }
            }
        }

        public PropertyHouseRoomTenantModel Getsystempropertyhousetenantdatabytenantid(long TenantId)
        {
            PropertyHouseRoomTenantModel TenantResponseModel = new PropertyHouseRoomTenantModel();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Tenantid", TenantId);
                parameters.Add("@SystemPropertyHouseTenantData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousetenantdatabytenantid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@SystemPropertyHouseTenantData");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    string TenantDetailJson = responseJson["Data"].ToString();
                    PropertyHouseRoomTenantData TenantResponse = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(TenantDetailJson);
                    TenantResponseModel.Data = TenantResponse;
                    return TenantResponseModel;
                }
                else
                {
                    return TenantResponseModel;
                }
            }
        }
        public PropertyHouseDetailData Getsystempropertyhousedetaildatabypropertyidandownerid(long Propertyid, long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Propertyid", Propertyid);
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedetaildatabypropertyidandownerid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertydata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<PropertyHouseDetailData>(systempropertydataJson);
                }
                else
                {
                    return new PropertyHouseDetailData();
                }
            }
        }
        public Genericmodel Registersystempropertyhouseroomtenantdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystempropertyhouseroomtenantdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel Registerpropertyhouseroomdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerpropertyhouseroomdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systempropertyhouserooms Getsystempropertyhouseroomdatabyid(long Houseroomid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Houseroomid", Houseroomid);
                parameters.Add("@Systempropertyhouseroomdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseroomdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyroomdataJson = parameters.Get<string>("@Systempropertyhouseroomdata");
                if (systempropertyroomdataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systempropertyhouserooms>(systempropertyroomdataJson);
                }
                else
                {
                    return new Systempropertyhouserooms();
                }
            }
        }
        public Systempropertyhouseroommeters Getsystempropertyhouseroommeterdatabyid(long Houseroomid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Houseroomid", Houseroomid);
                parameters.Add("@Systempropertyhousemetedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseroommeterdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertyhousemetedata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systempropertyhouseroommeters>(systempropertydataJson);
                }
                else
                {
                    return new Systempropertyhouseroommeters();
                }
            }
        }
        public Genericmodel Registerpropertyhouseroommeterdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerpropertyhouseroommeterdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}