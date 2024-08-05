using DBL.Entities.Mpesa;
using DBL.Models;
using DBL.Models.Mpesa;

namespace DBL.Repositories
{
    public interface IPaymentRepository
    {
        Genericmodel GetB2CSettings(int serviceCode);
        Genericmodel GetExprSettings(int serviceCode);

        Genericmodel CreatePayment(Payment entity);
        Genericmodel UpdateMPesa(string txnRef, int status, string message, string newRef = "");
        Genericmodel ProcessB2CResult(int serviceCode, B2CResultData data);
        Genericmodel ProcessExprCallback(int serviceCode, ExprCallbackDataModel data);
    }
}
