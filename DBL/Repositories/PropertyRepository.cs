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
        public PropertyHouseSummaryDashboard Getsystempropertyhousedashboardsummarydatabyowner(long Ownerid, long Posterid)
        {
            PropertyHouseSummaryDashboard propertyHouseSummaryDashboard = new PropertyHouseSummaryDashboard();
            PropertyHouseSummary propertyHouseSummary = new PropertyHouseSummary();
            List<PropertySummary>? Propertybysummary = new List<PropertySummary>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ownerid", Ownerid);
                parameters.Add("@Posterid", Posterid);
                parameters.Add("@Systempropertyhousedashboardsummarydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystempropertyhousedashboardsummarydatabyowner", parameters, commandType: CommandType.StoredProcedure);
                string systempropertyhousedashboardsummarydataJson = parameters.Get<string>("@Systempropertyhousedashboardsummarydata");
                if (systempropertyhousedashboardsummarydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertyhousedashboardsummarydataJson);
                    JObject dashboardsummaryJson = JObject.Parse(responseJson["Data"].ToString());
                    propertyHouseSummary.Propertyhouseunits = Convert.ToInt32(dashboardsummaryJson["Propertyhouseunits"]);
                    propertyHouseSummary.Systempropertyoccupiedroom = Convert.ToInt32(dashboardsummaryJson["Systempropertyoccupiedroom"]);
                    propertyHouseSummary.Systempropertyvacantroom = Convert.ToInt32(dashboardsummaryJson["Systempropertyvacantroom"]);
                    propertyHouseSummary.Rentarrears = Convert.ToDecimal(dashboardsummaryJson["Rentarrears"]);
                    propertyHouseSummary.Uncollectedpayments = Convert.ToDecimal(dashboardsummaryJson["Uncollectedpayments"]);
                    propertyHouseSummary.Consumedmeters = Convert.ToDecimal(dashboardsummaryJson["Consumedmeters"]);
                    if (dashboardsummaryJson["Propertybysummary"] != null)
                    {
                        string propertybysummaryJson = dashboardsummaryJson["Propertybysummary"].ToString();
                        Propertybysummary = JsonConvert.DeserializeObject<List<PropertySummary>>(propertybysummaryJson);
                        propertyHouseSummary.Propertybysummary = Propertybysummary;
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
                    }
                    TenantDataResponse.Tenantroomdata = TenantRoomDataResponse;
                    return TenantResponseModel;
                }
                else
                {
                    return TenantResponseModel;
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
                    responseData.Createdby = Convert.ToInt32(roomResponseJson["Createdby"]);
                    responseData.Datecreated = Convert.ToDateTime(roomResponseJson["Datecreated"]);
                    if (roomResponseJson["Meterhistorydata"] != null)
                    {
                        string MeterhistoryJson = roomResponseJson["Meterhistorydata"].ToString();
                        responseMeterData = JsonConvert.DeserializeObject<List<Systempropertyhouseroommeterhistory>>(MeterhistoryJson);
                    }
                    responseData.Meterhistorydata = responseMeterData;
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
                    responseData.InvoiceDetails = responseInvoiceDetailData;
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
    }
}