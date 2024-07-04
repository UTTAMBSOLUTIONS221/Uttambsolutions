using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IRoleRepository
    {
        Genericmodel Registersystemroledata(string JsonData);
        IEnumerable<SystemRole> Getsystemroledata(int Page, int PageSize);
        SystemRole Getsystemroledatabyid(long Roleid);
    }
}
