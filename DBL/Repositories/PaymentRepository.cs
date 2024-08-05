using DBL.Repositories.DBL.Repositories;

namespace DBL.Repositories
{
    public class PaymentRepository : BaseRepository, IPaymentRepository
    {
        public PaymentRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
