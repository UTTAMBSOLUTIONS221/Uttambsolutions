using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IPropertyRepository
    {
        IEnumerable<Systemproperty> Getsystempropertyhousedata();
        Genericmodel Registersystempropertyhousedata(string JsonData);
        Systemproperty Getsystempropertyhousedatabyid(long Propertyid);
    }
}
