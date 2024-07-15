using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IBlogsRepository
    {
        Systemblogdata Getsystemallblogdata(int Page, int PageSize);
        Genericmodel Registersystemblogdata(string JsonData);
        Systemblog Getsystemblogdatabyid(long Blogid);
        Genericmodel Registersystemserverblogdata(string JsonData);
        IEnumerable<Newsapiarticles> Getsystemblogsdata(int Page, int PageSize);
        IEnumerable<Newsapiarticles> Getsystemallunpublishedblogdata();
    }
}
