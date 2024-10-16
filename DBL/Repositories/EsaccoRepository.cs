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
    public class EsaccoRepository : BaseRepository, IEsaccoRepository
    {
        public EsaccoRepository(string connectionString) : base(connectionString)
        {
        }

        #region Esacco Sacco Administrator Summary
        public Saccosummarydatamodel Getsaccosummarymodeldata(int Staffid)
        {
            Saccosummarydatamodel Saccosummarydata = new Saccosummarydatamodel();
            List<Esaccosaccos> Esaccosaccos = new List<Esaccosaccos>();
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Staffid", Staffid);
                parameters.Add("@Saccosummarydata", dbType: DbType.String, direction: ParameterDirection.Output, size: int.MaxValue);
                var queryResult = connection.Query("Usp_Getsaccosummarymodeldata", parameters, commandType: CommandType.StoredProcedure);
                string saccosummarydataJson = parameters.Get<string>("@Saccosummarydata");
                if (saccosummarydataJson != null)
                {
                    JObject responseJson = JObject.Parse(saccosummarydataJson);

                    if (responseJson["Esaccosaccosdata"] != null)
                    {
                        string EsaccosaccosJson = responseJson["Esaccosaccosdata"].ToString();
                        Esaccosaccos = JsonConvert.DeserializeObject<List<Esaccosaccos>>(EsaccosaccosJson);
                        Saccosummarydata.Esaccosaccosdata = Esaccosaccos;
                    }
                    return Saccosummarydata;
                }
                else
                {
                    return Saccosummarydata;
                }
            }
        }
        #endregion


        #region Esacco Sacco Driver Summary
        public Genericmodel Registersaccodriverdata(string JsonData)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@JsonObjectdata", JsonData);
                return connection.Query<Genericmodel>("Usp_Registersaccodriverdata", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion
    }
}
