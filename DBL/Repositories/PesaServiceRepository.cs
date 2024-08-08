using Dapper;
using DBL.Entities.Mpesa;
using DBL.Models;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class PesaServiceRepository : BaseRepository, IPesaServiceRepository
    {
        public PesaServiceRepository(string connectionString) : base(connectionString)
        {
        }

        public BaseEntity AddApp(PesaApp entity)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", entity.ServiceCode);
                parameters.Add("@AppName", entity.AppName.ToUpper());
                parameters.Add("@AppID", entity.AppID);
                parameters.Add("@AppToken", entity.AppToken);

                return connection.Query<BaseEntity>("sp_AddPesaApp", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel AuthorizeApp(string appId)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AppId", appId);

                return connection.Query<Genericmodel>("sp_AuthorizePesaApp", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public BaseEntity AuthorizeAppStatus(int appCode, int status)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@AppCode", appCode);
                parameters.Add("@Stat", status);

                return connection.Query<BaseEntity>("sp_AuthorizePesaAppStat", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }


        public Genericmodel CreatePayment(Payment entity)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", entity.ServiceCode);
                parameters.Add("@AccountNo", entity.AccountNo);
                parameters.Add("@AccountName", entity.AccountName);
                parameters.Add("@Amount", entity.Amount);
                parameters.Add("@PType", entity.PType);
                parameters.Add("@TPRef", entity.TPRef);
                parameters.Add("@ExtRef", entity.ExtRef);
                parameters.Add("@Extra1", entity.Extra1);
                parameters.Add("@Extra2", entity.Extra2);
                parameters.Add("@Extra3", entity.Extra3);
                parameters.Add("@Extra4", entity.Extra4);

                return connection.Query<Genericmodel>("sp_AddPayment", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public PesaApp GetApp(int appCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                return connection.Query<PesaApp>(
                    FindStatement(PesaApp.TableName, "AppCode"),
                    param: new { Id = appCode }
                ).FirstOrDefault();
            }
        }

        public IEnumerable<PesaApp> GetApps(int serviceCode)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                return connection.Query<PesaApp>(
                     FindStatement(PesaApp.TableName, "ServiceCode"),
                     param: new { Id = serviceCode }
                  ).ToList();
            }
        }


        public IEnumerable<ListModel> GetCreateListModels()
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();

                return connection.Query<ListModel>(
                     GetAllStatement("vw_CreateServiceLists"),
                     param: new { }
                  ).ToList();
            }
        }

        public Genericmodel GetUrlRegData(int code)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", code);

                return connection.Query<Genericmodel>("sp_GetMPesaUrlRegData", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public BaseEntity UpdatePayment3PStatus(string paymentRef, int status, string message)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@RefNo", paymentRef);
                parameters.Add("@Stat", status);
                parameters.Add("@Msg", message);

                return connection.Query<BaseEntity>("sp_UpdatePayment3PStat", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public void UpdateRegStatus(int code, int status)
        {
            using (var connection = new SqlConnection(_connString))
            {
                connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", code);
                parameters.Add("@Stat", status);

                string sql = "Update PesaServices Set RegStat = @Stat Where ServiceCode = @ServiceCode";
                connection.Execute(sql, parameters, commandType: CommandType.Text);
            }
        }
    }
}
