using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IBrandRepository
    {
        Genericmodel Registersystemproductbranddata(string JsonData);
        IEnumerable<Productbrand> Getsystemproductbranddata(int Page, int PageSize);
        Productbrand Getsystemproductbranddatabyid(long Brandid);
    }
}
