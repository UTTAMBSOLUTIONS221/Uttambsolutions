using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<SystemStaff> Getsystemstaffdata(int Page, int PageSize);
        Genericmodel Registersystemstaffdata(string JsonData);
        SystemStaff Getsystemstaffdatabyid(long Staffid);
        UsermodelResponce VerifySystemStaff(string Username);
    }
}
