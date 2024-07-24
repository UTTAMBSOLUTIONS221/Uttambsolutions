using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<SystemStaff> Getsystemstaffdata(int Page, int PageSize);
        Genericmodel Registersystemstaffdata(string JsonData);
        SystemStaff Getsystemstaffdatabyid(long Staffid);
        SystemUserProfileData Getsystemuserprofiledata(long Userid);
        Genericmodel Updatestaffprofilepicturedata(string JsonData);
        UsermodelResponce VerifySystemStaff(string Username);
    }
}
