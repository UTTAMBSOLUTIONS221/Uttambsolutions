using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Systemstoreitems> Getsystemstoreitemdata();
        Genericmodel Registerstoreproductdata(string JsonData);
        Systemstoreitems Getsystemstoreitemdatabyid(int Storeitemid);
        Genericmodel Registersystemproductdata(string JsonData);
        IEnumerable<Systemproducts> Getsystemproductdata(int Page, int PageSize);
        Systemproducts Getsystemproductdatabyid(long Productid);
    }
}
