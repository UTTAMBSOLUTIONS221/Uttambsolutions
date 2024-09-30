using Dapper;
using DBL.Entities;
using DBL.Entities.Tokenization;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using Newtonsoft.Json;
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


        #region System Services
        public IEnumerable<Systemservices> Getsystemservicesdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Systemservices>("Usp_Getsystemservicesdata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersystemservicedata(string jsonObjectdata)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", jsonObjectdata);
                return connection.Query<Genericmodel>("Usp_Registersystemservicedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Systemservices Getsystemservicesdatabyid(long Serviceid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Serviceid", Serviceid);
                parameters.Add("@Systemservicedata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemservicesdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string SystemservicedataJson = parameters.Get<string>("@Systemservicedata");
                if (SystemservicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemservices>(SystemservicedataJson);
                }
                else
                {
                    return new Systemservices();
                }
            }
        }
        public Systemservices Getsystemservicesitemsdatabyid(long Serviceid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Serviceid", Serviceid);
                parameters.Add("@Systemservicesitemsdata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsystemservicesitemsdatabyid", parameters, commandType: CommandType.StoredProcedure);
                string SystemservicedataJson = parameters.Get<string>("@Systemservicesitemsdata");
                if (SystemservicedataJson != null)
                {
                    return JsonConvert.DeserializeObject<Systemservices>(SystemservicedataJson);
                }
                else
                {
                    return new Systemservices();
                }
            }
        }
        #endregion

        #region Software Tokenizations
        public IEnumerable<Softwaretoken> Getsystemsoftwaretokensdata()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                return connection.Query<Softwaretoken>("Usp_Getsystemsoftwaretokensdata", null, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        public Genericmodel Registersoftwaretokendata(string jsonObjectdata)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", jsonObjectdata);
                return connection.Query<Genericmodel>("Usp_Registersoftwaretokendata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Softwaretoken Getsystemsoftwaretokensdatabyid(long Tokenid)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Tokenid", Tokenid);
                return connection.Query<Softwaretoken>("Usp_Getsystemsoftwaretokensdatabyid", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        public Genericmodel Registersoftwaretokenpurchasedata(string jsonObjectdata)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", jsonObjectdata);
                return connection.Query<Genericmodel>("Usp_Registersoftwaretokenpurchasedata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
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
