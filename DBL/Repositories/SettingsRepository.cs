using DBL.Repositories.DBL.Repositories;

namespace DBL.Repositories
{
    public class SettingsRepository : BaseRepository, ISettingsRepository
    {
        public SettingsRepository(string connectionString) : base(connectionString)
        {
        }
    }
}
