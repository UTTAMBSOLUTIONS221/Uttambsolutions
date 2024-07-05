using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IOrganizationRepository
    {
        Genericmodel Registersystemorganizationdata(string JsonData);
        SystemOrganization Getsystemorganizationdatabyid(long Organizationid);
        SystemOrganizationDetails Getsystemorganizationdetaildatabyid(long Organizationid);
        Genericmodel Registerorganizationshopproductdata(string JsonData);
    }
}
