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
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(string connectionString) : base(connectionString)
        {
        }
        #region Register Staffs
        public IEnumerable<SystemStaff> Getsystemstaffdata(int Page, int PageSize)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Page", Page);
                parameters.Add("@PageSize", PageSize);
                return connection.Query<SystemStaff>("Usp_Getsystemstaffdata", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersystemuserdevicedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemuserdevicedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Registersystemstaffdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemstaffdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel SaveStaffRefreshToken(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Savestaffrefreshtokendata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SystemStaff Getsystemstaffdatabyid(long Staffid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Staffid", Staffid);
                return connection.Query<SystemStaff>("Usp_Getsystemstaffdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SystemStaff Getsystemstaffdatabyrefreshtoken(string Refreshtoken)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Refreshtoken", Refreshtoken);
                return connection.Query<SystemStaff>("Usp_Getsystemstaffdatabyrefreshtoken", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public SystemUserProfileData Getsystemuserprofiledata(long Userid)
        {

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                SystemUserProfileData resp = new SystemUserProfileData();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Userid", Userid);
                parameters.Add("@UserProfileData", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemuserprofiledata", parameters, commandType: CommandType.StoredProcedure);
                string userProfileDataJson = parameters.Get<string>("@UserProfileData");
                if (userProfileDataJson != null)
                {
                    return JsonConvert.DeserializeObject<SystemUserProfileData>(userProfileDataJson);
                }
                else
                {
                    return new SystemUserProfileData();
                }

            }
        }
        public Genericmodel Updatestaffprofilepicturedata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Updatestaffprofilepicturedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Updatestaffcurriculumdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Updatestaffcurriculumdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Registersystemjobapplicationdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersystemjobapplicationdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Systemstaffdetaildata Getsystemstaffdetaildatabyid(long Staffid)
        {
            Systemstaffdetaildata staffResponseModel = new Systemstaffdetaildata();
            StaffDetailData staffResponseData = new StaffDetailData();
            List<AccountVerificationBank> bankAccoutsResponseModel = new List<AccountVerificationBank>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Staffid", Staffid);
                parameters.Add("@Systemstaffdetaildata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemstaffdetaildatabyid", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systemstaffdetaildata");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    JObject staffreponseJson = JObject.Parse(responseJson["Data"].ToString());
                    staffResponseData.Userid = Convert.ToInt32(staffreponseJson["Userid"]);
                    staffResponseData.Fullname = staffreponseJson["Fullname"].ToString();
                    staffResponseData.Phonenumber = staffreponseJson["Phonenumber"].ToString();
                    staffResponseData.Accountid = Convert.ToInt32(staffreponseJson["Accountid"]);
                    staffResponseData.Accountnumber = Convert.ToInt32(staffreponseJson["Accountnumber"]);
                    staffResponseData.Loginstatus = Convert.ToInt32(staffreponseJson["Loginstatus"]);
                    staffResponseData.Subscriptionamount = Convert.ToDecimal(staffreponseJson["Subscriptionamount"]);
                    if (staffreponseJson["AccountVerificationBanks"] != null)
                    {
                        string bankAccountsJson = staffreponseJson["AccountVerificationBanks"].ToString();
                        bankAccoutsResponseModel = JsonConvert.DeserializeObject<List<AccountVerificationBank>>(bankAccountsJson);
                        staffResponseData.AccountVerificationBanks = bankAccoutsResponseModel;
                    }
                    staffResponseModel.Data = staffResponseData;
                    return staffResponseModel;
                }
                else
                {
                    return staffResponseModel;
                }
            }
        }

        public Systemtenantdetailsdata Getsystemstaffdatabyidnumber(int Idnumber)
        {
            Systemtenantdetailsdata TenantResponseModel = new Systemtenantdetailsdata();
            Systemtenantdetails TenantDataResponse = new Systemtenantdetails();
            List<PropertyHouseTenant> Tenantroomhistory = new List<PropertyHouseTenant>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Idnumber", Idnumber);
                parameters.Add("@Systempropertyhouseroomtenantdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemstaffdatabyidnumber", parameters, commandType: CommandType.StoredProcedure);
                string systempropertydataJson = parameters.Get<string>("@Systempropertyhouseroomtenantdata");
                if (systempropertydataJson != null)
                {
                    JObject responseJson = JObject.Parse(systempropertydataJson);
                    JObject tenantreponseJson = JObject.Parse(responseJson["Data"].ToString());
                    TenantDataResponse.Userid = Convert.ToInt32(tenantreponseJson["Userid"]);
                    TenantDataResponse.Fullname = tenantreponseJson["Fullname"].ToString();
                    TenantDataResponse.Phonenumber = tenantreponseJson["Phonenumber"].ToString();
                    TenantDataResponse.Emailaddress = tenantreponseJson["Emailaddress"].ToString();
                    TenantDataResponse.Idnumber = Convert.ToInt32(tenantreponseJson["Idnumber"]);
                    TenantDataResponse.Walletbalance = Convert.ToDecimal(tenantreponseJson["Walletbalance"]);
                    TenantDataResponse.Loginstatus = Convert.ToInt32(tenantreponseJson["Loginstatus"]);
                    if (tenantreponseJson["Tenantroomhistory"] != null)
                    {
                        string TenantroomhistoryJson = tenantreponseJson["Tenantroomhistory"].ToString();
                        Tenantroomhistory = JsonConvert.DeserializeObject<List<PropertyHouseTenant>>(TenantroomhistoryJson);
                        TenantDataResponse.Tenantroomhistory = Tenantroomhistory;
                    }
                    TenantResponseModel.Data = TenantDataResponse;
                    return TenantResponseModel;
                }
                else
                {
                    return TenantResponseModel;
                }
            }
        }
        #endregion

        #region Verify System Staff
        public UsermodelResponce VerifySystemStaff(string Username)
        {

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                UsermodelResponce resp = new UsermodelResponce();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Username", Username);
                parameters.Add("@StaffDetails", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Verifysystemstaffdata", parameters, commandType: CommandType.StoredProcedure);
                string staffDetailsJson = parameters.Get<string>("@StaffDetails");
                JObject responseJson = JObject.Parse(staffDetailsJson);
                if (Convert.ToInt32(responseJson["RespStatus"]) == 0)
                {
                    string userModelJson = responseJson["Usermodel"].ToString();
                    UsermodeldataResponce userResponse = JsonConvert.DeserializeObject<UsermodeldataResponce>(userModelJson);
                    resp.RespStatus = Convert.ToInt32(responseJson["RespStatus"]);
                    resp.RespMessage = responseJson["RespMessage"].ToString();
                    resp.Usermodel = userResponse;
                    return resp;
                }
                else
                {
                    resp.RespStatus = Convert.ToInt32(responseJson["RespStatus"]);
                    resp.RespMessage = responseJson["RespMessage"].ToString();
                    resp.Usermodel = new UsermodeldataResponce();
                    return resp;
                }
            }
        }
        #endregion


        #region Verify System Staff Forgot Password
        public ForgotPasswordUserResponce VerifyForgotPasswordSystemStaff(string JsonData)
        {

            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                ForgotPasswordUserResponce resp = new ForgotPasswordUserResponce();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectData", JsonData);
                parameters.Add("@StaffDetails", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Verifyforgotpasswordsystemstaff", parameters, commandType: CommandType.StoredProcedure);
                string staffDetailsJson = parameters.Get<string>("@StaffDetails");
                JObject responseJson = JObject.Parse(staffDetailsJson);
                string userModelJson = responseJson["Data"].ToString();
                Forgotpassword userResponse = JsonConvert.DeserializeObject<Forgotpassword>(userModelJson);
                resp.Data = userResponse;
                return resp;
            }
        }
        #endregion


        #region System Permission by Roles
        public List<string> Getsystempermissiondatabyroleid(long Roleid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Roleid", Roleid);
                return connection.Query<string>("Usp_Getsystempermissiondatabyroleid", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        #endregion
    }
}
