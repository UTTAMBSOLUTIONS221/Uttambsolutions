using Dapper;
using DBL.Entities.Mpesa;
using DBL.Models;
using DBL.Models.Mpesa;
using DBL.Repositories.DBL.Repositories;
using System.Data;
using System.Data.SqlClient;

namespace DBL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {
        }
        public Genericmodel CreatePayment(Payment entity)
        {
            using (IDbConnection Connection = new SqlConnection(_connString))
            {
                Connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", entity.ServiceCode);
                parameters.Add("@AccountNo", entity.AccountNo);
                parameters.Add("@AccountName", entity.AccountName);
                parameters.Add("@Amount", entity.Amount);
                parameters.Add("@PType", entity.PType);
                parameters.Add("@PStat", entity.PStatus);
                parameters.Add("@ExtRef", entity.ExtRef);
                parameters.Add("@Extra1", entity.Extra1);
                parameters.Add("@Extra2", entity.Extra2);
                parameters.Add("@Extra3", entity.Extra3);
                parameters.Add("@Extra4", entity.Extra4);
                parameters.Add("@TPMsg", entity.TPMessage);
                parameters.Add("@TPStat", entity.TPStat);
                parameters.Add("@TPRef", entity.TPRef);
                parameters.Add("@AppCode", entity.AppCode);

                return Connection.Query<Genericmodel>("sp_CreatePayment", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel GetB2CSettings(int serviceCode)
        {
            using (IDbConnection Connection = new SqlConnection(_connString))
            {
                Connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", serviceCode);

                return Connection.Query<Genericmodel>("sp_GetB2CSettings", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel GetExprSettings(int serviceCode)
        {
            using (IDbConnection Connection = new SqlConnection(_connString))
            {
                Connection.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", serviceCode);

                return Connection.Query<Genericmodel>("sp_GetExprSettings", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel ProcessB2CResult(int serviceCode, B2CResultData data)
        {
            using (IDbConnection Connection = new SqlConnection(_connString))
            {
                Connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", serviceCode);
                parameters.Add("@OrgRef", data.OrgRef);
                parameters.Add("@TxnID", data.TxnID);
                parameters.Add("@ResultCode", data.ResultCode);
                parameters.Add("@ResultDescr", data.ResultDescr);
                parameters.Add("@Balance", data.WorkingBalance);
                parameters.Add("@Receiver", data.Receiver);

                return Connection.Query<Genericmodel>("sp_ProcessB2CResult", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel ProcessExprCallback(int serviceCode, ExprCallbackDataModel data)
        {
            using (IDbConnection Connection = new SqlConnection(_connString))
            {
                Connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ServiceCode", serviceCode);
                parameters.Add("@OrgRef", data.CheckoutRequestID);
                parameters.Add("@TxnID", data.RefNo);
                parameters.Add("@ResultCode", data.ResultCode);
                parameters.Add("@ResultDescr", data.ResultDesc);
                parameters.Add("@Receiver", data.CustomerDets);
                parameters.Add("@Amount", data.Amount);

                return Connection.Query<Genericmodel>("sp_ProcessExprCallback", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        public Genericmodel UpdateMPesa(string txnRef, int status, string message, string newRef = "")
        {
            using (IDbConnection Connection = new SqlConnection(_connString))
            {
                Connection.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@TxnRef", txnRef);
                parameters.Add("@Stat", status);
                parameters.Add("@Msg", message);
                parameters.Add("@NewRef", newRef);

                return Connection.Query<Genericmodel>("sp_UpateB2CPayment", parameters, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
    }
}

