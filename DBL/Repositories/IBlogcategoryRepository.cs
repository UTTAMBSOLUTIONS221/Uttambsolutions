using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IBlogcategoryRepository
    {
        IEnumerable<Systemblogcategories> Getsystemblogcategorydata(int Page, int PageSize);
        Genericmodel Registersystemblogcategorydata(string JsonData);
        Systemblogcategories Getsystemblogcategorydatabyid(long Blogcategoryid);
    }
}
