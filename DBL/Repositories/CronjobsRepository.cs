using DBL.Repositories.DBL.Repositories;

namespace DBL.Repositories
{
    public class CronjobsRepository : BaseRepository, ICronjobsRepository
    {
        public CronjobsRepository(string connectionString) : base(connectionString)
        {
        }
        #region Generate Monthly Invoices
        #endregion
    }
}
