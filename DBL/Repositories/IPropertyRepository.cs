using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IPropertyRepository
    {
        Genericmodel Registersystempropertyhousedata(string JsonData);
        Systemproperty Getsystempropertyhousedatabyid(long Propertyid);
    }
}
