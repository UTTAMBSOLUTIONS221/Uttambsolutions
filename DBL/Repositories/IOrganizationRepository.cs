using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IOrganizationRepository
    {
        IEnumerable<SystemOrganization> Getsystemorganizationdata();
        Genericmodel Registersystemorganizationdata(string JsonData);
        SystemOrganization Getsystemorganizationdatabyid(long Organizationid);
        SystemOrganizationDetails Getsystemorganizationdetaildatabyid(long Organizationid);
        Organizationshopproductsdata Registerorganizationshopproductdata(string JsonData);
        Organizationshopproducts Getorganizationshopproductdatabyid(long Shopproductid);
        Systemorganizationshopproducts Getsystemorganizationshopproductsdata();
        Organizationshopproductsdata Getsystemorganizationshopproductsdatabyid(long Shopproductid);
    }
}
