using DBL.Entities.Mpesa;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IPesaServiceRepository
    {
        Genericmodel GetUrlRegData(int code);
        void UpdateRegStatus(int code, int status);

        Genericmodel CreatePayment(Payment entity);
        BaseEntity UpdatePayment3PStatus(string paymentRef, int status, string message);

        IEnumerable<ListModel> GetCreateListModels();

        IEnumerable<PesaApp> GetApps(int clientCode);
        PesaApp GetApp(int appCode);
        BaseEntity AddApp(PesaApp entity);
        Genericmodel AuthorizeApp(string appId);
        BaseEntity AuthorizeAppStatus(int appCode, int status);
    }
}
