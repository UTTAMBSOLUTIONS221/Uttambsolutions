using Dapper;
using DBL.Entities;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        public SettingsRepository(string connectionString) : base(connectionString)
        {
        }
        #region System Permissions
        public IEnumerable<Systempermissions> Getsystempermissiondata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Systempermissions>("Usp_Getsystempermissiondata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersystempermissiondata(string jsonObjectdata)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", jsonObjectdata);
                return connection.Query<Genericmodel>("Usp_Registersystempermissiondata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systempermissions Getsystempermissiondatabyid(long Permissionid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Permissionid", Permissionid);
                return connection.Query<Systempermissions>("Usp_Getsystempermissiondatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion

        #region Communication Templates
        public IEnumerable<Communicationtemplate> Getsystemcommunicationtemplatedata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Communicationtemplate>("Usp_Getsystemcommunicationtemplatedata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Communicationtemplate Getsystemcommunicationtemplatedatabyname(bool Isemail, string Templatename)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Isemail", Isemail);
                parameters.Add("@Templatename", Templatename);
                return connection.Query<Communicationtemplate>("Usp_Getsystemcommunicationtemplatedatabyname", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Registersystemcommunicationtemplatedata(string jsonObjectdata)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", jsonObjectdata);
                return connection.Query<Genericmodel>("Usp_Registersystemcommunicationtemplatedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Communicationtemplate Getsystemcommunicationtemplatedatabyid(long Templateid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Templateid", Templateid);
                return connection.Query<Communicationtemplate>("Usp_Getsystemcommunicationtemplatedatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion

        #region Log Email Messages
        public Genericmodel LogEmailMessage(string JsonEntity)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();

                parameters.Add("@JsonObjectdata", JsonEntity);
                return connection.Query<Genericmodel>("Usp_RegisterSystemEmailLogs", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion

        #region Log User Activities
        public Genericmodel Registersystemuseractivitydata(string jsonObjectdata)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", jsonObjectdata);
                return connection.Query<Genericmodel>("Usp_Registersystemuseractivitydata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion
    }
}
