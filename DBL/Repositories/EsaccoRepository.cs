using DBL.Repositories.DBL.Repositories;

namespace DBL.Repositories
{
    public class EsaccoRepository : BaseRepository, IEsaccoRepository
    {
        public EsaccoRepository(string connectionString) : base(connectionString)
        {
        }

        #region Esacco Sacco Administrator Summary
        #endregion
    }
}
