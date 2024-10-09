using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Models.Dashboards;
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

        #region System Property Summary
        public Maqaoplussummary Getmaqaoplussummarydata()
        {
            Maqaoplussummary Maqaoplussummarydata = new Maqaoplussummary();
            List<Vacanthousesdata> Vacanthouses = new List<Vacanthousesdata>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Maqaoplussummarydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getmaqaoplussummarydata", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Maqaoplussummarydata");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    Maqaoplussummarydata.Tokenprice = Convert.ToDecimal(responseJson["Tokenprice"]);
                    Maqaoplussummarydata.Totalsupply = Convert.ToDecimal(responseJson["Totalsupply"]);
                    Maqaoplussummarydata.Listedproperties = Convert.ToInt32(responseJson["Listedproperties"]);
                    Maqaoplussummarydata.Listedjobs = Convert.ToInt32(responseJson["Listedjobs"]);
                    Maqaoplussummarydata.Registeredtenants = Convert.ToInt32(responseJson["Registeredtenants"]);
                    Maqaoplussummarydata.Registeredowners = Convert.ToInt32(responseJson["Registeredowners"]);
                    Maqaoplussummarydata.Occupiedhouses = Convert.ToInt32(responseJson["Occupiedhouses"]);
                    Maqaoplussummarydata.Collectedrent = Convert.ToDecimal(responseJson["Collectedrent"]);
                    if (responseJson["Vacanthouses"] != null)
                    {
                        string VacanthousesJson = responseJson["Vacanthouses"].ToString();
                        Vacanthouses = JsonConvert.DeserializeObject<List<Vacanthousesdata>>(VacanthousesJson);
                        Maqaoplussummarydata.Vacanthouses = Vacanthouses;
                    }
                    return Maqaoplussummarydata;
                }
                else
                {
                    return Maqaoplussummarydata;
                }
            }
        }
        #endregion

        #region System Property House Listing
        public Genericmodel Registerhousepropertydata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerhousepropertydata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion

        public PropertyHouseDetailData Getallsystempropertyvacanthousesdata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getallsystempropertyvacanthousesdata", parameters, commandType: CommandType.StoredProcedure);
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
        public Genericmodel Registersystemagentpropertyhousedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemagentpropertyhousedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
        public SystemPropertyHouseCareTakerData Getsystempropertyhousecaretakerdatabyownerid(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertyhousecaretakerdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousecaretakerdatabyownerid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertyhousecaretakerdata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemPropertyHouseCareTakerData>(systempropertydataJson);
                }
                else
                {
                    return new SystemPropertyHouseCareTakerData();
                }
            }
        }

        public SystemStaffData Getsystempropertyhousecaretakerdatabyid(long Caretakerhouseid)
        {
            SystemStaffData CareTakerResponseModel = new SystemStaffData();
            SystemStaff CareTakerResponse = new SystemStaff();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Caretakerhouseid", Caretakerhouseid);
                parameters.Add("@Systempropertyhousecaretakerdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousecaretakerdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertyhousecaretakerdata");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    JObject tenantreponseJson = JObject.Parse(responseJson["Data"].ToString());
                    CareTakerResponse.Userid = Convert.ToInt32(tenantreponseJson["Userid"]);
                    CareTakerResponse.Caretakerhouseid = Convert.ToInt32(tenantreponseJson["Caretakerhouseid"]);
                    CareTakerResponse.Propertyhouseid = Convert.ToInt32(tenantreponseJson["Propertyhouseid"]);
                    CareTakerResponse.Propertyhousename = tenantreponseJson["Propertyhousename"].ToString();
                    CareTakerResponse.Firstname = tenantreponseJson["Firstname"].ToString();
                    CareTakerResponse.Lastname = tenantreponseJson["Lastname"].ToString();
                    CareTakerResponse.Fullname = tenantreponseJson["Fullname"].ToString();
                    CareTakerResponse.Phonenumber = tenantreponseJson["Phonenumber"].ToString();
                    CareTakerResponse.Designation = tenantreponseJson["Staffdesignation"].ToString();
                    CareTakerResponse.Username = tenantreponseJson["Username"].ToString();
                    CareTakerResponse.Emailaddress = tenantreponseJson["Emailaddress"].ToString();
                    CareTakerResponse.Genderid = Convert.ToInt32(tenantreponseJson["Genderid"]);
                    CareTakerResponse.Maritalstatusid = Convert.ToInt32(tenantreponseJson["Maritalstatusid"]);
                    CareTakerResponse.Roleid = Convert.ToInt32(tenantreponseJson["Roleid"]);
                    CareTakerResponse.Isactive = Convert.ToBoolean(tenantreponseJson["Isactive"]);
                    CareTakerResponse.Isdeleted = Convert.ToBoolean(tenantreponseJson["Isdeleted"]);
                    CareTakerResponse.Isdefault = Convert.ToBoolean(tenantreponseJson["Isdefault"]);
                    CareTakerResponse.Loginstatus = Convert.ToInt32(tenantreponseJson["Loginstatus"]);
                    CareTakerResponse.Passwordresetdate = Convert.ToDateTime(tenantreponseJson["Passwordresetdate"]);
                    CareTakerResponse.Parentid = Convert.ToInt32(tenantreponseJson["Parentid"]);
                    CareTakerResponse.Userprofileimageurl = tenantreponseJson["Userprofileimageurl"].ToString();
                    CareTakerResponse.Usercurriculumvitae = tenantreponseJson["Usercurriculumvitae"].ToString();
                    CareTakerResponse.Idnumber = Convert.ToInt32(tenantreponseJson["Idnumber"]);
                    CareTakerResponse.Expirydate = Convert.ToDateTime(tenantreponseJson["Expirydate"]);
                    CareTakerResponse.Updateprofile = Convert.ToBoolean(tenantreponseJson["Updateprofile"]);
                    CareTakerResponse.Extra = tenantreponseJson["Extra"].ToString();
                    CareTakerResponse.Extra1 = tenantreponseJson["Extra1"].ToString();
                    CareTakerResponse.Extra2 = tenantreponseJson["Extra2"].ToString();
                    CareTakerResponse.Extra3 = tenantreponseJson["Extra3"].ToString();
                    CareTakerResponse.Extra4 = tenantreponseJson["Extra4"].ToString();
                    CareTakerResponse.Extra5 = tenantreponseJson["Extra5"].ToString();
                    CareTakerResponse.Createdby = Convert.ToInt32(tenantreponseJson["Createdby"]);
                    CareTakerResponse.Modifiedby = Convert.ToInt32(tenantreponseJson["Modifiedby"]);
                    CareTakerResponse.Mpesapaybill = Convert.ToInt32(tenantreponseJson["Mpesapaybill"]);
                    CareTakerResponse.Accountnumber = Convert.ToInt32(tenantreponseJson["Accountnumber"]);
                    CareTakerResponse.Subscriptionamount = Convert.ToDecimal(tenantreponseJson["Accountnumber"]);
                    CareTakerResponse.Kinname = tenantreponseJson["Kinname"].ToString();
                    CareTakerResponse.Kinphonenumber = tenantreponseJson["Kinphonenumber"].ToString();
                    CareTakerResponse.Kinrelationshipid = Convert.ToInt32(tenantreponseJson["Kinrelationshipid"]);
                    CareTakerResponse.Columnreadonly = Convert.ToBoolean(tenantreponseJson["Columnreadonly"]);
                    CareTakerResponse.Lastlogin = Convert.ToDateTime(tenantreponseJson["Lastlogin"]);
                    CareTakerResponse.Datemodified = Convert.ToDateTime(tenantreponseJson["Datemodified"]);
                    CareTakerResponse.Datecreated = Convert.ToDateTime(tenantreponseJson["Datecreated"]);
                    CareTakerResponseModel.Data = CareTakerResponse;
                    return CareTakerResponseModel;
                }
                else
                {
                    return CareTakerResponseModel;
                }
            }
        }


        public PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyowner(long Ownerid)
        {
            PropertyHouseSummaryDashboard propertyHouseSummaryDashboard = new PropertyHouseSummaryDashboard();
            PropertyHouseSummary propertyHouseSummary = new PropertyHouseSummary();
            List<PropertySummary>? Propertybysummary = new List<PropertySummary>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertyhousedashboardsummarydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedashboardsummarydatabyowner", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhousedashboardsummarydataJson = parameters.Get<string>("@Systempropertyhousedashboardsummarydata");
                if (systempropertyhousedashboardsummarydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyhousedashboardsummarydataJson);
                    if (responseJson["Data"] != null && responseJson["Data"].Type != JTokenType.Null)
                    {
                        JObject dashboardsummaryJson = JObject.Parse(responseJson["Data"].ToString());
                        propertyHouseSummary.Propertyhouseunits = Convert.ToInt32(dashboardsummaryJson["Propertyhouseunits"]);
                        propertyHouseSummary.Systempropertyoccupiedroom = Convert.ToInt32(dashboardsummaryJson["Systempropertyoccupiedroom"]);
                        propertyHouseSummary.Systempropertyvacantroom = Convert.ToInt32(dashboardsummaryJson["Systempropertyvacantroom"]);
                        propertyHouseSummary.Expectedcollections = Convert.ToDecimal(dashboardsummaryJson["Expectedcollections"]);
                        propertyHouseSummary.Collectedcollections = Convert.ToDecimal(dashboardsummaryJson["Collectedcollections"]);
                        propertyHouseSummary.Rentarrears = Convert.ToDecimal(dashboardsummaryJson["Rentarrears"]);
                        propertyHouseSummary.Uncollectedpayments = Convert.ToDecimal(dashboardsummaryJson["Uncollectedpayments"]);
                        propertyHouseSummary.Consumedmeters = Convert.ToDecimal(dashboardsummaryJson["Consumedmeters"]);
                        if (dashboardsummaryJson["Propertybysummary"] != null)
                        {
                            string propertybysummaryJson = dashboardsummaryJson["Propertybysummary"].ToString();
                            Propertybysummary = JsonConvert.DeserializeObject<List<PropertySummary>>(propertybysummaryJson);
                            propertyHouseSummary.Propertybysummary = Propertybysummary;
                        }
                    }
                    propertyHouseSummaryDashboard.Data = propertyHouseSummary;
                    return propertyHouseSummaryDashboard;
                }
                else
                {
                    return propertyHouseSummaryDashboard;
                }
            }
        }

        public PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyagent(long Agentid)
        {
            PropertyHouseSummaryDashboard propertyHouseSummaryDashboard = new PropertyHouseSummaryDashboard();
            PropertyHouseSummary propertyHouseSummary = new PropertyHouseSummary();
            List<PropertySummary>? Propertybysummary = new List<PropertySummary>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Agentid", Agentid);
                parameters.Add("@Systempropertyhousedashboardsummarydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedashboardsummarydatabyagent", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhousedashboardsummarydataJson = parameters.Get<string>("@Systempropertyhousedashboardsummarydata");
                if (systempropertyhousedashboardsummarydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyhousedashboardsummarydataJson);
                    if (responseJson["Data"] != null && responseJson["Data"].Type != JTokenType.Null)
                    {
                        JObject dashboardsummaryJson = JObject.Parse(responseJson["Data"].ToString());
                        propertyHouseSummary.Propertyhouseunits = Convert.ToInt32(dashboardsummaryJson["Propertyhouseunits"]);
                        propertyHouseSummary.Systempropertyoccupiedroom = Convert.ToInt32(dashboardsummaryJson["Systempropertyoccupiedroom"]);
                        propertyHouseSummary.Systempropertyvacantroom = Convert.ToInt32(dashboardsummaryJson["Systempropertyvacantroom"]);
                        propertyHouseSummary.Expectedcollections = Convert.ToDecimal(dashboardsummaryJson["Expectedcollections"]);
                        propertyHouseSummary.Collectedcollections = Convert.ToDecimal(dashboardsummaryJson["Collectedcollections"]);
                        propertyHouseSummary.Rentarrears = Convert.ToDecimal(dashboardsummaryJson["Rentarrears"]);
                        propertyHouseSummary.Uncollectedpayments = Convert.ToDecimal(dashboardsummaryJson["Uncollectedpayments"]);
                        propertyHouseSummary.Consumedmeters = Convert.ToDecimal(dashboardsummaryJson["Consumedmeters"]);
                        if (dashboardsummaryJson["Propertybysummary"] != null)
                        {
                            string propertybysummaryJson = dashboardsummaryJson["Propertybysummary"].ToString();
                            Propertybysummary = JsonConvert.DeserializeObject<List<PropertySummary>>(propertybysummaryJson);
                            propertyHouseSummary.Propertybysummary = Propertybysummary;
                        }
                    }
                    propertyHouseSummaryDashboard.Data = propertyHouseSummary;
                    return propertyHouseSummaryDashboard;
                }
                else
                {
                    return propertyHouseSummaryDashboard;
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
        public Systempropertyhousedata Getallsystempropertyhousedata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getallsystempropertyhousedata", parameters, commandType: CommandType.StoredProcedure);
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

        public Systempropertyhousedata Getsystempropertyhousedatabyagent(long Agentid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Agentid", Agentid);
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedatabyagentid", parameters, commandType: CommandType.StoredProcedure);
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
            List<PropertyHouseDetails> Vacanthousesdataresponse = new List<PropertyHouseDetails>();
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
                    }
                    TenantDataResponse.Tenantroomdata = TenantRoomDataResponse;
                    if (tenantreponseJson["Vacanthousesdata"] != null)
                    {
                        string VacanthousesdataJson = tenantreponseJson["Vacanthousesdata"].ToString();
                        Vacanthousesdataresponse = JsonConvert.DeserializeObject<List<PropertyHouseDetails>>(VacanthousesdataJson);
                    }
                    TenantDataResponse.Vacanthousesdata = Vacanthousesdataresponse;

                    return TenantResponseModel;
                }
                else
                {
                    return TenantResponseModel;
                }
            }
        }

        public PropertyHouseTenantData Getsystempropertyhouseroomtenantsdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systempropertyhouseroomtenantsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getallsystempropertyhouseroomtenantsdata", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhouseroomtenantsdataJson = parameters.Get<string>("@Systempropertyhouseroomtenantsdata");
                if (systempropertyhouseroomtenantsdataJson != null)
                {
                    return JsonConvert.DeserializeObject<PropertyHouseTenantData>(systempropertyhouseroomtenantsdataJson);
                }
                else
                {
                    return new PropertyHouseTenantData();
                }
            }
        }
        public PropertyHouseTenantData Getsystempropertyhouseroomtenantsdata(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertyhouseroomtenantsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseroomtenantsdata", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhouseroomtenantsdataJson = parameters.Get<string>("@Systempropertyhouseroomtenantsdata");
                if (systempropertyhouseroomtenantsdataJson != null)
                {
                    return JsonConvert.DeserializeObject<PropertyHouseTenantData>(systempropertyhouseroomtenantsdataJson);
                }
                else
                {
                    return new PropertyHouseTenantData();
                }
            }
        }

        public PropertyHouseTenantData Getsystemagentpropertyhouseroomtenantsdata(long Agentid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Agentid", Agentid);
                parameters.Add("@Systempropertyhouseroomtenantsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemagentpropertyhouseroomtenantsdata", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhouseroomtenantsdataJson = parameters.Get<string>("@Systempropertyhouseroomtenantsdata");
                if (systempropertyhouseroomtenantsdataJson != null)
                {
                    return JsonConvert.DeserializeObject<PropertyHouseTenantData>(systempropertyhouseroomtenantsdataJson);
                }
                else
                {
                    return new PropertyHouseTenantData();
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

        public OwnerTenantAgreementDetailDataModel Getsystempropertyhouseagreementdetaildatabyownerid(long Ownerid)
        {
            OwnerTenantAgreementDetailDataModel response = new OwnerTenantAgreementDetailDataModel();
            OwnerTenantAgreementDetailData responseData = new OwnerTenantAgreementDetailData();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@OwnerTenantAgreementDetailData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseagreementdetaildatabyownerid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@OwnerTenantAgreementDetailData");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    JObject tenantreponseJson = JObject.Parse(responseJson["Data"].ToString());
                    responseData.Propertyhouseid = Convert.ToInt64(tenantreponseJson["Propertyhouseid"]);
                    responseData.Propertyhouseowner = Convert.ToInt64(tenantreponseJson["Propertyhouseowner"]);
                    responseData.Propertyhousename = tenantreponseJson["Propertyhousename"].ToString();
                    responseData.Fullname = tenantreponseJson["Fullname"].ToString();
                    responseData.Phonenumber = tenantreponseJson["Phonenumber"].ToString();
                    responseData.Emailaddress = tenantreponseJson["Emailaddress"].ToString();
                    responseData.Countyname = tenantreponseJson["Countyname"].ToString();
                    responseData.Subcountyname = tenantreponseJson["Subcountyname"].ToString();
                    responseData.Subcountywardname = tenantreponseJson["Subcountywardname"].ToString();
                    responseData.OwnerDatecreated = Convert.ToDateTime(tenantreponseJson["OwnerDatecreated"]);
                    responseData.OwnerSignatureimageurl = tenantreponseJson["OwnerSignatureimageurl"].ToString();

                    response.Data = responseData;
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }

        public OwnerTenantAgreementDetailDataModel Getsystempropertyhouseagreementdetaildatabyagentid(long Agentid)
        {
            OwnerTenantAgreementDetailDataModel response = new OwnerTenantAgreementDetailDataModel();
            OwnerTenantAgreementDetailData responseData = new OwnerTenantAgreementDetailData();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Agentid", Agentid);
                parameters.Add("@OwnerTenantAgreementDetailData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseagreementdetaildatabyagentid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@OwnerTenantAgreementDetailData");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    JObject tenantreponseJson = JObject.Parse(responseJson["Data"].ToString());
                    responseData.Propertyhouseid = Convert.ToInt64(tenantreponseJson["Propertyhouseid"]);
                    responseData.Propertyhouseowner = Convert.ToInt64(tenantreponseJson["Propertyhouseowner"]);
                    responseData.Propertyhousename = tenantreponseJson["Propertyhousename"].ToString();
                    responseData.Fullname = tenantreponseJson["Fullname"].ToString();
                    responseData.Phonenumber = tenantreponseJson["Phonenumber"].ToString();
                    responseData.Emailaddress = tenantreponseJson["Emailaddress"].ToString();
                    responseData.Countyname = tenantreponseJson["Countyname"].ToString();
                    responseData.Subcountyname = tenantreponseJson["Subcountyname"].ToString();
                    responseData.Subcountywardname = tenantreponseJson["Subcountywardname"].ToString();
                    responseData.OwnerDatecreated = Convert.ToDateTime(tenantreponseJson["OwnerDatecreated"]);
                    responseData.OwnerSignatureimageurl = tenantreponseJson["OwnerSignatureimageurl"].ToString();

                    response.Data = responseData;
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }

        public TenantAgreementDetailDataModel Getsystempropertyhouseroomagreementdetaildatabytenantid(long Tenantid)
        {
            TenantAgreementDetailDataModel response = new TenantAgreementDetailDataModel();
            TenantAgreementDetailData responseData = new TenantAgreementDetailData();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Tenantid", Tenantid);
                parameters.Add("@TenantAgreementDetailData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseroomagreementdetaildatabytenantid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@TenantAgreementDetailData");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    if (responseJson["Data"] != null && responseJson["Data"].Type != JTokenType.Null)
                    {
                        JObject tenantreponseJson = JObject.Parse(responseJson["Data"].ToString());
                        responseData.Userid = Convert.ToInt32(tenantreponseJson["Userid"]);
                        responseData.Propertyhouseid = Convert.ToInt32(tenantreponseJson["Propertyhouseid"]);
                        responseData.Tenantfullname = tenantreponseJson["Tenantfullname"].ToString();
                        responseData.Tenantphonenumber = tenantreponseJson["Tenantphonenumber"].ToString();
                        responseData.Tenantemailaddress = tenantreponseJson["Tenantemailaddress"].ToString();
                        responseData.Tenantidnumber = Convert.ToInt32(tenantreponseJson["Tenantidnumber"]);
                        responseData.Ownerfullname = tenantreponseJson["Ownerfullname"].ToString();
                        responseData.Ownerphonenumber = tenantreponseJson["Ownerphonenumber"].ToString();
                        responseData.Owneremailaddress = tenantreponseJson["Owneremailaddress"].ToString();
                        responseData.Owneridnumber = Convert.ToInt32(tenantreponseJson["Owneridnumber"]);
                        responseData.Propertyhousename = tenantreponseJson["Propertyhousename"].ToString();
                        responseData.Rentdueday = Convert.ToInt32(tenantreponseJson["Rentdueday"]);
                        responseData.Vacantnoticeperiod = Convert.ToInt32(tenantreponseJson["Vacantnoticeperiod"]);
                        responseData.Hasagent = Convert.ToBoolean(tenantreponseJson["Hasagent"]);
                        responseData.Hashousewatermeter = Convert.ToBoolean(tenantreponseJson["Hashousewatermeter"]);
                        responseData.Systemhousewatertypename = tenantreponseJson["Systemhousewatertypename"].ToString();
                        responseData.Systempropertyhousesizename = tenantreponseJson["Systempropertyhousesizename"].ToString();
                        responseData.Systempropertyhousesizerent = Convert.ToDecimal(tenantreponseJson["Systempropertyhousesizerent"]);
                        responseData.Systempropertyhousesizerentdeposit = Convert.ToDecimal(tenantreponseJson["Systempropertyhousesizerentdeposit"]);
                        responseData.Rentdepositmonth = Convert.ToInt32(tenantreponseJson["Rentdepositmonth"]);
                        responseData.Rentdepositrefunddays = Convert.ToInt32(tenantreponseJson["Rentdepositrefunddays"]);
                        responseData.Monthlyrentterms = Convert.ToBoolean(tenantreponseJson["Monthlyrentterms"]);
                        responseData.Allowpets = Convert.ToBoolean(tenantreponseJson["Allowpets"]);
                        responseData.Rentutilityinclusive = Convert.ToBoolean(tenantreponseJson["Rentutilityinclusive"]);
                        responseData.Waterunitprice = Convert.ToDecimal(tenantreponseJson["Waterunitprice"]);
                        responseData.Countyname = tenantreponseJson["Countyname"].ToString();
                        responseData.Subcountyname = tenantreponseJson["Subcountyname"].ToString();
                        responseData.Subcountywardname = tenantreponseJson["Subcountywardname"].ToString();
                        responseData.Streetorlandmark = tenantreponseJson["Streetorlandmark"].ToString();
                        responseData.TenantDatecreated = Convert.ToDateTime(tenantreponseJson["TenantDatecreated"]);
                        responseData.Nextrentduedate = Convert.ToDateTime(tenantreponseJson["Nextrentduedate"]);
                        responseData.TenantSignatureimageurl = tenantreponseJson["TenantSignatureimageurl"].ToString();
                        responseData.OwnerSignatureimageurl = tenantreponseJson["OwnerSignatureimageurl"].ToString();
                        responseData.Agreementdata = tenantreponseJson["Agreementdata"].ToString();
                        responseData.Propertyhouseutility = tenantreponseJson["Propertyhouseutility"].ToString();
                        responseData.Systempropertybankname = tenantreponseJson["Systempropertybankname"].ToString();
                        responseData.Tenantsintheroom = tenantreponseJson["Tenantsintheroom"].ToString();
                        responseData.Termenddate = tenantreponseJson["Termenddate"].ToString();
                    }
                    response.Data = responseData;
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }
        public Genericmodel Registersystempropertyhouseagreementdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystempropertyhouseagreementdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public PropertyHouseDetailData Getsystempropertyhousedetaildatabyhouseid(long Propertyhouseid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Propertyhouseid", Propertyhouseid);
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedetaildatabyhouseid", parameters, commandType: CommandType.StoredProcedure);
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

        public PropertyHouseDetailData Getsystempropertyhousedetaildatabyownerid(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedetaildatabyownerid", parameters, commandType: CommandType.StoredProcedure);
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

        public Genericmodel Registersystempropertyhouseroommeterdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystempropertyhouseroommeterdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systempropertyhouseroomfixturesdata Getsystempropertyhouseroomfixturesdatabyhouseroomid(long Houseroomid)
        {
            Systempropertyhouseroomfixturesdata response = new Systempropertyhouseroomfixturesdata();
            Systempropertyhouseroomfixtures responseData = new Systempropertyhouseroomfixtures();
            List<RoomFixture>? propertyhouseroomfixturesummary = new List<RoomFixture>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Houseroomid", Houseroomid);
                parameters.Add("@Systempropertyhouseroomfixturesdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhouseroomfixturesdatabyhouseroomid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhouseroomfixturesdataJson = parameters.Get<string>("@Systempropertyhouseroomfixturesdata");
                if (systempropertyhouseroomfixturesdataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyhouseroomfixturesdataJson);
                    JObject propertyhouseroomfixtureresponseJson = JObject.Parse(responseJson["Data"].ToString());
                    responseData.Systempropertyhouseroomid = Convert.ToInt32(propertyhouseroomfixtureresponseJson["Systempropertyhouseroomid"]);
                    responseData.Systempropertyhouseid = Convert.ToInt32(propertyhouseroomfixtureresponseJson["Systempropertyhouseid"]);
                    if (propertyhouseroomfixtureresponseJson["Roomfixtures"] != null)
                    {
                        string propertyhouseroomfixturesummaryJson = propertyhouseroomfixtureresponseJson["Roomfixtures"].ToString();
                        propertyhouseroomfixturesummary = JsonConvert.DeserializeObject<List<RoomFixture>>(propertyhouseroomfixturesummaryJson);
                    }
                    responseData.Roomfixtures = propertyhouseroomfixturesummary;
                    response.Data = responseData;

                    return response;
                }
                else
                {
                    return response;
                }
            }
        }

        public Genericmodel Registersystempropertyhouseroomfixturedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystempropertyhouseroomfixturedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Registersystempropertyhouseroomimagedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystempropertyhouseroomimagedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public SystemPropertyHouseImageData Getsystempropertyimagebyhouseroomid(long Houseroomid)
        {
            SystemPropertyHouseImageData response = new SystemPropertyHouseImageData();
            SystemPropertyHouseImage propertyhouseroomimage = new SystemPropertyHouseImage();
            List<SystemPropertyHouseImageModel>? propertyhouseroomimagesummary = new List<SystemPropertyHouseImageModel>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Houseroomid", Houseroomid);
                parameters.Add("@Systempropertyhouseroomimagedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyimagebyhouseroomid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhouseroomimagedataJson = parameters.Get<string>("@Systempropertyhouseroomimagedata");
                if (systempropertyhouseroomimagedataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyhouseroomimagedataJson);
                    if (responseJson["Data"] != null && responseJson["Data"].Type != JTokenType.Null)
                    {
                        JObject propertyhouseroomimageresponseJson = JObject.Parse(responseJson["Data"].ToString());
                        propertyhouseroomimage.Propertyhouseid = Convert.ToInt32(propertyhouseroomimageresponseJson["Propertyhouseid"]);
                        if (propertyhouseroomimageresponseJson["PropertyHouseImage"] != null)
                        {
                            string propertyhouseroomimagesummaryJson = propertyhouseroomimageresponseJson["PropertyHouseImage"].ToString();
                            propertyhouseroomimagesummary = JsonConvert.DeserializeObject<List<SystemPropertyHouseImageModel>>(propertyhouseroomimagesummaryJson);
                        }
                        propertyhouseroomimage.PropertyHouseImage = propertyhouseroomimagesummary;
                        response.Data = propertyhouseroomimage;
                    }
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }
        public SystemPropertyHouseImageData Getsystempropertyimagebyhouseid(long Houseid)
        {
            SystemPropertyHouseImageData response = new SystemPropertyHouseImageData();
            SystemPropertyHouseImage propertyhouseroomimage = new SystemPropertyHouseImage();
            List<SystemPropertyHouseImageModel>? propertyhouseroomimagesummary = new List<SystemPropertyHouseImageModel>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Houseid", Houseid);
                parameters.Add("@Systempropertyhouseimagedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyimagebyhouseid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhouseroomimagedataJson = parameters.Get<string>("@Systempropertyhouseimagedata");
                if (systempropertyhouseroomimagedataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyhouseroomimagedataJson);
                    if (responseJson["Data"] != null && responseJson["Data"].Type != JTokenType.Null)
                    {
                        JObject propertyhouseroomimageresponseJson = JObject.Parse(responseJson["Data"].ToString());
                        propertyhouseroomimage.Propertyhouseid = Convert.ToInt32(propertyhouseroomimageresponseJson["Propertyhouseid"]);
                        if (propertyhouseroomimageresponseJson["PropertyHouseImage"] != null)
                        {
                            string propertyhouseroomimagesummaryJson = propertyhouseroomimageresponseJson["PropertyHouseImage"].ToString();
                            propertyhouseroomimagesummary = JsonConvert.DeserializeObject<List<SystemPropertyHouseImageModel>>(propertyhouseroomimagesummaryJson);
                        }
                        propertyhouseroomimage.PropertyHouseImage = propertyhouseroomimagesummary;
                        response.Data = propertyhouseroomimage;
                    }
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }
        public Genericmodel Registerpropertyhousevacaterequestdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerpropertyhousevacaterequestdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systempropertyhouseroomdata Getsystempropertyhouseroomdatabyid(long Houseroomid)
        {
            Systempropertyhouseroomdata response = new Systempropertyhouseroomdata();
            Systempropertyhouserooms responseData = new Systempropertyhouserooms();
            List<Systempropertyhouseroommeterhistory> responseMeterData = new List<Systempropertyhouseroommeterhistory>();
            List<RoomFixture>? propertyhouseroomfixturesummary = new List<RoomFixture>();
            List<PropertyHousetenantroomhistory>? Tenantroomhistory = new List<PropertyHousetenantroomhistory>();
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
                    responseData.Hasprevious = Convert.ToBoolean(roomResponseJson["Hasprevious"]);
                    responseData.Systempropertyhouseroomid = Convert.ToInt64(roomResponseJson["Systempropertyhouseroomid"]);
                    responseData.Systempropertyhouseid = Convert.ToInt64(roomResponseJson["Systempropertyhouseid"]);
                    responseData.Systempropertyhousesizeid = Convert.ToInt64(roomResponseJson["Systempropertyhousesizeid"]);
                    responseData.Systempropertyhousesizename = roomResponseJson["Systempropertyhousesizename"]?.ToString();
                    responseData.Systempropertyhousesizerent = Convert.ToDecimal(roomResponseJson["Systempropertyhousesizerent"]);
                    responseData.Systempropertyhousesizedeposit = Convert.ToBoolean(roomResponseJson["Systempropertyhousesizedeposit"]);
                    responseData.Isvacant = Convert.ToBoolean(roomResponseJson["Isvacant"]);
                    responseData.Isunderrenovation = Convert.ToBoolean(roomResponseJson["Isunderrenovation"]);
                    responseData.Hashousewatermeter = Convert.ToBoolean(roomResponseJson["Hashousewatermeter"]);
                    responseData.Isshop = Convert.ToBoolean(roomResponseJson["Isshop"]);
                    responseData.Isgroundfloor = Convert.ToBoolean(roomResponseJson["Isgroundfloor"]);
                    responseData.Hasbalcony = Convert.ToBoolean(roomResponseJson["Hasbalcony"]);
                    responseData.Forcaretaker = Convert.ToBoolean(roomResponseJson["Forcaretaker"]);
                    responseData.Kitchentypeid = Convert.ToInt64(roomResponseJson["Kitchentypeid"]);
                    responseData.Systempropertyhousemeterid = Convert.ToInt32(roomResponseJson["Systempropertyhousemeterid"]);
                    responseData.Systempropertyhouseroommeternumber = roomResponseJson["Systempropertyhouseroommeternumber"]?.ToString();
                    responseData.Openingmeter = Convert.ToDecimal(roomResponseJson["Openingmeter"]);
                    responseData.Movedmeter = Convert.ToDecimal(roomResponseJson["Movedmeter"]);
                    responseData.Closingmeter = Convert.ToDecimal(roomResponseJson["Closingmeter"]);
                    responseData.Consumedamount = Convert.ToDecimal(roomResponseJson["Consumedamount"]);
                    responseData.Waterunitprice = Convert.ToDecimal(roomResponseJson["Waterunitprice"]);
                    responseData.Tenantid = Convert.ToInt64(roomResponseJson["Tenantid"]);
                    responseData.Roomoccupant = Convert.ToInt32(roomResponseJson["Roomoccupant"]);
                    responseData.Roomoccupantdetail = roomResponseJson["Roomoccupantdetail"].ToString();
                    responseData.Createdby = Convert.ToInt32(roomResponseJson["Createdby"]);
                    responseData.Fullname = roomResponseJson["Fullname"].ToString();
                    responseData.Phonenumber = roomResponseJson["Phonenumber"].ToString();
                    responseData.Emailaddress = roomResponseJson["Emailaddress"].ToString();
                    responseData.Gender = roomResponseJson["Gender"].ToString();
                    responseData.Maritalstatus = roomResponseJson["Maritalstatus"].ToString();
                    responseData.Loginstatus = Convert.ToInt32(roomResponseJson["Loginstatus"]);
                    responseData.Parentid = Convert.ToInt32(roomResponseJson["Parentid"]);
                    responseData.Userprofileimageurl = roomResponseJson["Userprofileimageurl"].ToString();
                    responseData.Usercurriculumvitae = roomResponseJson["Usercurriculumvitae"].ToString();
                    responseData.Idnumber = Convert.ToInt32(roomResponseJson["Idnumber"]);
                    responseData.Updateprofile = Convert.ToBoolean(roomResponseJson["Updateprofile"]);
                    responseData.Accountnumber = Convert.ToInt32(roomResponseJson["Accountnumber"]);
                    responseData.Accountid = Convert.ToInt32(roomResponseJson["Accountid"]);
                    responseData.Walletbalance = Convert.ToDecimal(roomResponseJson["Walletbalance"]);
                    responseData.Datemodified = Convert.ToDateTime(roomResponseJson["Datemodified"]);
                    responseData.Datecreated = Convert.ToDateTime(roomResponseJson["Datecreated"]);
                    if (roomResponseJson["Meterhistorydata"] != null)
                    {
                        string MeterhistoryJson = roomResponseJson["Meterhistorydata"].ToString();
                        responseMeterData = JsonConvert.DeserializeObject<List<Systempropertyhouseroommeterhistory>>(MeterhistoryJson);
                    }
                    if (roomResponseJson["Roomfixtures"] != null)
                    {
                        string PropertyhouseroomfixtureJson = roomResponseJson["Roomfixtures"].ToString();
                        propertyhouseroomfixturesummary = JsonConvert.DeserializeObject<List<RoomFixture>>(PropertyhouseroomfixtureJson);
                    }
                    if (roomResponseJson["Roomtenanthistorydata"] != null)
                    {
                        string RoomtenanthistoryJson = roomResponseJson["Roomtenanthistorydata"].ToString();
                        Tenantroomhistory = JsonConvert.DeserializeObject<List<PropertyHousetenantroomhistory>>(RoomtenanthistoryJson);
                    }
                    responseData.Meterhistorydata = responseMeterData;
                    responseData.Roomfixtures = propertyhouseroomfixturesummary;
                    responseData.Roomtenanthistorydata = Tenantroomhistory;
                    response.Data = responseData;
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

        public SystemPropertyHouseVacatingRequestModel Gettenantvacatingrequestsdatabyownerid()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systempropertyvacatingrequestdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getalltenantvacatingrequestsdata", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertyvacatingrequestdata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemPropertyHouseVacatingRequestModel>(systempropertydataJson);
                }
                else
                {
                    return new SystemPropertyHouseVacatingRequestModel();
                }
            }
        }
        public SystemPropertyHouseVacatingRequestModel Gettenantvacatingrequestsdatabyownerid(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systempropertyvacatingrequestdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantvacatingrequestsdatabyownerid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertyvacatingrequestdata");
                if (systempropertydataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemPropertyHouseVacatingRequestModel>(systempropertydataJson);
                }
                else
                {
                    return new SystemPropertyHouseVacatingRequestModel();
                }
            }
        }

        public Genericmodel Approvepropertyhousevacatingrequest(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Approvepropertyhousevacatingrequest", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabyownerid(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systemtenantmonthlyinvoicedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicedatabyownerid", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicedata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoiceData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoiceData();
                }
            }
        }
        public TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabyagentid(long Agentid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Agentid", Agentid);
                parameters.Add("@Systemtenantmonthlyinvoicedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicedatabyagentid", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicedata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoiceData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoiceData();
                }
            }
        }
        public TenantMonthlyInvoiceData Gettenantmonthlyinvoicedatabytenantid(long Tenantid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Tenantid", Tenantid);
                parameters.Add("@Systemtenantmonthlyinvoicedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicedatabytenantid", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicedata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoiceData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoiceData();
                }
            }
        }


        public TenantMonthlyInvoiceDetailData Gettenantmonthlyinvoicedetaildatabyinvoiceid(long Invoiceid)
        {
            TenantMonthlyInvoiceDetailData response = new TenantMonthlyInvoiceDetailData();
            MonthlyRentInvoiceModel responseData = new MonthlyRentInvoiceModel();
            List<MonthlyRentInvoiceItem> responseInvoiceDetailData = new List<MonthlyRentInvoiceItem>();
            List<Propertyhousebankingdetail> responseInvoiceBankDetailData = new List<Propertyhousebankingdetail>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Invoiceid", Invoiceid);
                parameters.Add("@Systemtenantmonthlyinvoicedetaildata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicedetaildatabyinvoiceid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyroomdataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicedetaildata");
                if (systempropertyroomdataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyroomdataJson);
                    JObject invoiceResponseJson = JObject.Parse(responseJson["Data"].ToString());
                    responseData.Invoiceid = Convert.ToInt32(invoiceResponseJson["Invoiceid"]);
                    responseData.Financetransactionid = Convert.ToInt32(invoiceResponseJson["Financetransactionid"]);
                    responseData.Transactioncode = invoiceResponseJson["TransactionCode"]?.ToString();
                    responseData.Invoiceno = invoiceResponseJson["Invoiceno"]?.ToString();
                    responseData.Propertyhouseroomid = Convert.ToInt32(invoiceResponseJson["Propertyhouseroomid"]);
                    responseData.Systemhousesizename = invoiceResponseJson["Systemhousesizename"]?.ToString();
                    responseData.Systempropertyhousesizename = invoiceResponseJson["Systempropertyhousesizename"]?.ToString();
                    responseData.Propertyhouseroomtenantid = Convert.ToInt32(invoiceResponseJson["Propertyhouseroomtenantid"]);
                    responseData.Tenantname = invoiceResponseJson["Tenantname"]?.ToString();
                    responseData.Datecreated = Convert.ToDateTime(invoiceResponseJson["Datecreated"]);
                    responseData.Duedate = Convert.ToDateTime(invoiceResponseJson["Duedate"]);
                    responseData.Amount = Convert.ToDecimal(invoiceResponseJson["Amount"]);
                    responseData.Discount = Convert.ToDecimal(invoiceResponseJson["Discount"]);
                    responseData.Balance = Convert.ToDecimal(invoiceResponseJson["Balance"]);
                    responseData.Ispaid = Convert.ToBoolean(invoiceResponseJson["Ispaid"]);
                    responseData.Paidamount = Convert.ToDecimal(invoiceResponseJson["Paidamount"]);
                    responseData.Issent = Convert.ToBoolean(invoiceResponseJson["Issent"]);
                    responseData.Paidstatus = invoiceResponseJson["Paidstatus"]?.ToString();
                    if (invoiceResponseJson["InvoiceDetails"] != null)
                    {
                        string TenantroomhistoryJson = invoiceResponseJson["InvoiceDetails"].ToString();
                        responseInvoiceDetailData = JsonConvert.DeserializeObject<List<MonthlyRentInvoiceItem>>(TenantroomhistoryJson);
                    }
                    if (invoiceResponseJson["Propertyhousebankingdetail"] != null)
                    {
                        string PropertyhousebankingdetailJson = invoiceResponseJson["Propertyhousebankingdetail"].ToString();
                        responseInvoiceBankDetailData = JsonConvert.DeserializeObject<List<Propertyhousebankingdetail>>(PropertyhousebankingdetailJson);
                    }
                    responseData.InvoiceDetails = responseInvoiceDetailData;
                    responseData.Propertyhousebankingdetail = responseInvoiceBankDetailData;
                    response.Data = responseData;
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }

        public Genericmodel Registerpropertyhouseroomrentpaymentrequestdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registerpropertyhouseroomrentpaymentrequestdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyownerid()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Systemtenantmonthlyinvoicepaymentdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getalltenantmonthlyinvoicepaymentdata", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicepaymentdata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoicePaymentData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoicePaymentData();
                }
            }
        }
        public TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyownerid(long Ownerid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Systemtenantmonthlyinvoicepaymentdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicepaymentdatabyownerid", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicepaymentdata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoicePaymentData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoicePaymentData();
                }
            }
        }
        public TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabyagentid(long Agentid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Agentid", Agentid);
                parameters.Add("@Systemtenantmonthlyinvoicepaymentdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicepaymentdatabyagentid", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicepaymentdata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoicePaymentData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoicePaymentData();
                }
            }
        }
        public TenantMonthlyInvoicePaymentData Gettenantmonthlyinvoicepaymentdatabytenantid(long Tenantid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Tenantid", Tenantid);
                parameters.Add("@Systemtenantmonthlyinvoicepaymentdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Gettenantmonthlyinvoicepaymentdatabytenantid", parameters, commandType: CommandType.StoredProcedure);
                string systemtenantmonthlyinvoicedataJson = parameters.Get<string>("@Systemtenantmonthlyinvoicepaymentdata");
                if (systemtenantmonthlyinvoicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<TenantMonthlyInvoicePaymentData>(systemtenantmonthlyinvoicedataJson);
                }
                else
                {
                    return new TenantMonthlyInvoicePaymentData();
                }
            }
        }

        public CustomerPaymentValidationData Getsystempropertyroompaymentbypaymentid(long Paymentid)
        {
            CustomerPaymentValidationData response = new CustomerPaymentValidationData();
            CustomerPaymentValidation responseData = new CustomerPaymentValidation();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Paymentid", Paymentid);
                parameters.Add("@Systempropertyroompaymentdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyroompaymentbypaymentid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyroompaymentdataJson = parameters.Get<string>("@Systempropertyroompaymentdata");
                if (systempropertyroompaymentdataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyroompaymentdataJson);
                    JObject invoiceResponseJson = JObject.Parse(responseJson["Data"].ToString());
                    responseData.CustomerPaymentId = Convert.ToInt32(invoiceResponseJson["CustomerPaymentId"]);
                    responseData.HouseRoomTenantId = Convert.ToInt32(invoiceResponseJson["HouseRoomTenantId"]);
                    responseData.Houseroomid = Convert.ToInt32(invoiceResponseJson["Houseroomid"]);
                    responseData.PaymentModeId = Convert.ToInt32(invoiceResponseJson["PaymentModeId"]);
                    responseData.Financetransactionid = Convert.ToInt32(invoiceResponseJson["Financetransactionid"]);
                    responseData.Tenantid = Convert.ToInt64(invoiceResponseJson["Tenantid"]);
                    responseData.Confirmedby = Convert.ToInt64(invoiceResponseJson["Confirmedby"]);
                    responseData.Amount = Convert.ToDecimal(invoiceResponseJson["Amount"]);
                    responseData.Actualamount = Convert.ToDecimal(invoiceResponseJson["Actualamount"]);
                    responseData.TransactionReference = invoiceResponseJson["TransactionReference"]?.ToString();
                    responseData.TransactionDate = Convert.ToDateTime(invoiceResponseJson["TransactionDate"]);
                    responseData.IsPaymentValidated = Convert.ToBoolean(invoiceResponseJson["IsPaymentValidated"]);
                    responseData.ChequeNo = invoiceResponseJson["ChequeNo"]?.ToString();
                    responseData.ChequeDate = invoiceResponseJson["ChequeDate"] != null ? Convert.ToDateTime(invoiceResponseJson["ChequeDate"]) : (DateTime?)null;
                    responseData.Memo = invoiceResponseJson["Memo"]?.ToString();
                    responseData.DrawerBank = invoiceResponseJson["DrawerBank"]?.ToString();
                    responseData.DepositBank = invoiceResponseJson["DepositBank"]?.ToString();
                    responseData.PaidBy = Convert.ToInt32(invoiceResponseJson["PaidBy"]);
                    responseData.ValidatedBy = Convert.ToInt32(invoiceResponseJson["ValidatedBy"]);
                    responseData.SlipReference = invoiceResponseJson["SlipReference"]?.ToString();
                    responseData.Datecreated = Convert.ToDateTime(invoiceResponseJson["Datecreated"]);
                    responseData.Datemodified = Convert.ToDateTime(invoiceResponseJson["Datemodified"]);

                    response.Data = responseData;
                    return response;
                }
                else
                {
                    return response;
                }
            }
        }

        public Genericmodel Registervalidatecustomerpaymentrequestdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registervalidatecustomerpaymentrequestdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel Updatemonthlyrentinvoicedata(long Invoiceid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Invoiceid", Invoiceid);
                return connection.Query<Genericmodel>("Usp_Updatemonthlyrentinvoicedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}