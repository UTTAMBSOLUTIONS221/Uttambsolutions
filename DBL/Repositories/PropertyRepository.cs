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
            PropertyHouseRoomTenantData TenantDataResponse = new PropertyHouseRoomTenantData();
            Systempropertyhousetenantsroom TenantRoomDataResponse = new Systempropertyhousetenantsroom();
            List<PropertyHousetenantroomhistory> Tenantroomhistory = new List<PropertyHousetenantroomhistory>();
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
                    JObject tenantreponseJson = JObject.Parse(responseJson["Data"].ToString());
                    TenantDataResponse.Userid = Convert.ToInt32(tenantreponseJson["Userid"]);
                    TenantDataResponse.Fullname = tenantreponseJson["Fullname"].ToString();
                    TenantDataResponse.Phonenumber = tenantreponseJson["Phonenumber"].ToString();
                    TenantDataResponse.Emailaddress = tenantreponseJson["Emailaddress"].ToString();
                    TenantDataResponse.Gender = tenantreponseJson["Gender"].ToString();
                    TenantDataResponse.Maritalstatus = tenantreponseJson["Maritalstatus"].ToString();
                    TenantDataResponse.Loginstatus = Convert.ToInt32(tenantreponseJson["Loginstatus"]);
                    TenantDataResponse.Parentid = Convert.ToInt32(tenantreponseJson["Parentid"]);
                    TenantDataResponse.Userprofileimageurl = tenantreponseJson["Userprofileimageurl"].ToString();
                    TenantDataResponse.Usercurriculumvitae = tenantreponseJson["Usercurriculumvitae"].ToString();
                    TenantDataResponse.Idnumber = Convert.ToInt32(tenantreponseJson["Idnumber"]);
                    TenantDataResponse.Updateprofile = Convert.ToBoolean(tenantreponseJson["Updateprofile"]);
                    TenantDataResponse.Accountnumber = Convert.ToInt32(tenantreponseJson["Accountnumber"]);
                    TenantDataResponse.Accountid = Convert.ToInt32(tenantreponseJson["Accountid"]);
                    TenantDataResponse.Walletbalance = Convert.ToDecimal(tenantreponseJson["Walletbalance"]);
                    TenantDataResponse.Datemodified = Convert.ToDateTime(tenantreponseJson["Datemodified"]);
                    TenantDataResponse.Datecreated = Convert.ToDateTime(tenantreponseJson["Datecreated"]);
                    if (tenantreponseJson["Tenantroomhistory"] != null)
                    {
                        string TenantroomhistoryJson = tenantreponseJson["Tenantroomhistory"].ToString();
                        Tenantroomhistory = JsonConvert.DeserializeObject<List<PropertyHousetenantroomhistory>>(TenantroomhistoryJson);
                        TenantDataResponse.Tenantroomhistory = Tenantroomhistory;
                    }
                    TenantResponseModel.Data = TenantDataResponse;
                    if (tenantreponseJson["Tenantroomdata"] != null)
                    {
                        string TenantroomJson = tenantreponseJson["Tenantroomdata"].ToString();
                        TenantRoomDataResponse = JsonConvert.DeserializeObject<Systempropertyhousetenantsroom>(TenantroomJson);
                        TenantResponseModel.Data.Tenantroomdata = TenantRoomDataResponse;
                    }
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
        public Systempropertyhouseroomdata Getsystempropertyhouseroomdatabyid(long Houseroomid)
        {
            Systempropertyhouseroomdata response = new Systempropertyhouseroomdata();
            Systempropertyhouserooms responseData = new Systempropertyhouserooms();
            List<Systempropertyhouseroommeterhistory> responseMeterData = new List<Systempropertyhouseroommeterhistory>();
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
                    JObject responseJson = JObject.Parse(systempropertyroomdataJson);
                    JObject roomResponseJson = JObject.Parse(responseJson["Data"].ToString());
                    response.Data = new Systempropertyhouserooms
                    {
                        Hasprevious = Convert.ToBoolean(roomResponseJson["Hasprevious"]),
                        Systempropertyhouseroomid = Convert.ToInt64(roomResponseJson["Systempropertyhouseroomid"]),
                        Systempropertyhouseid = Convert.ToInt64(roomResponseJson["Systempropertyhouseid"]),
                        Systempropertyhousesizeid = Convert.ToInt64(roomResponseJson["Systempropertyhousesizeid"]),
                        Systempropertyhousesizename = roomResponseJson["Systempropertyhousesizename"]?.ToString(),
                        Isvacant = Convert.ToBoolean(roomResponseJson["Isvacant"]),
                        Isunderrenovation = Convert.ToBoolean(roomResponseJson["Isunderrenovation"]),
                        Isshop = Convert.ToBoolean(roomResponseJson["Isshop"]),
                        Isgroundfloor = Convert.ToBoolean(roomResponseJson["Isgroundfloor"]),
                        Hasbalcony = Convert.ToBoolean(roomResponseJson["Hasbalcony"]),
                        Forcaretaker = Convert.ToBoolean(roomResponseJson["Forcaretaker"]),
                        Kitchentypeid = Convert.ToInt64(roomResponseJson["Kitchentypeid"]),
                        Systempropertyhousemeterid = Convert.ToInt32(roomResponseJson["Systempropertyhousemeterid"]),
                        Systempropertyhouseroommeternumber = roomResponseJson["Systempropertyhouseroommeternumber"]?.ToString(),
                        Openingmeter = Convert.ToDecimal(roomResponseJson["Openingmeter"]),
                        Movedmeter = Convert.ToDecimal(roomResponseJson["Movedmeter"]),
                        Closingmeter = Convert.ToDecimal(roomResponseJson["Closingmeter"]),
                        Consumedamount = Convert.ToDecimal(roomResponseJson["Consumedamount"]),
                        Waterunitprice = Convert.ToDecimal(roomResponseJson["Waterunitprice"]),
                        Tenantid = Convert.ToInt64(roomResponseJson["Tenantid"]),
                        Createdby = Convert.ToInt32(roomResponseJson["Createdby"]),
                        Datecreated = Convert.ToDateTime(roomResponseJson["Datecreated"])
                    };
                    if (roomResponseJson["Meterhistorydata"] != null)
                    {
                        string MeterhistoryJson = roomResponseJson["Meterhistorydata"].ToString();
                        responseMeterData = JsonConvert.DeserializeObject<List<Systempropertyhouseroommeterhistory>>(MeterhistoryJson);
                        responseData.Meterhistorydata = responseMeterData;
                    }
                    return response;
                }
                else
                {
                    return response;
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