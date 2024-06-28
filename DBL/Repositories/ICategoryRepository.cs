using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface ICategoryRepository
    {
        Genericmodel Registersystemcategorydata(string JsonData);
        IEnumerable<Productcategories> Getsystemcategorydata(int Page, int PageSize);
    }
}
