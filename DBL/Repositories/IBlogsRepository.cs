using DBL.Models;

namespace DBL.Repositories
{
    public interface IBlogsRepository
    {
        Genericmodel Registersystemblogdata(string JsonData);
        IEnumerable<Newsapiarticles> Getsystemblogsdata(int Page, int PageSize);
    }
}
