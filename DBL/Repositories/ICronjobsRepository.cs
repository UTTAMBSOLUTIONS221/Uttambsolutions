using DBL.Models;

namespace DBL.Repositories
{
    public interface ICronjobsRepository
    {
        Genericmodel Generatemonthlyrentinvoicedata();
        Monthlyrentinvoicedata Getsystemunsentemaildata();
    }
}
