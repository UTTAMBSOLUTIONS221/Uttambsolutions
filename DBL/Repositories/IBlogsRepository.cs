using DBL.Models;

namespace DBL.Repositories
{
    public interface IBlogsRepository
    {
        Genericmodel Registersystemblogdata(string JsonData);
        Genericmodel Registersystemserverblogdata(string JsonData);
        IEnumerable<Newsapiarticles> Getsystemblogsdata(int Page, int PageSize);
    }
}
