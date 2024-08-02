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

        public SystemStaff Getsystemstaffdatabyidnumber(int Idnumber)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Idnumber", Idnumber);
                return connection.Query<SystemStaff>("Usp_Getsystemstaffdatabyidnumber", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
                var queryResult = connection.Query("Usp_verifysystemuser", parameters, commandType: CommandType.StoredProcedure);
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
