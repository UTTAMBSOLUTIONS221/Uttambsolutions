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
        Genericmodel Updatestaffcurriculumdata(string JsonData);
        SystemStaff Getsystemstaffdatabyidnumber(int Idnumber);
        Genericmodel Registersystemjobapplicationdata(string JsonData);
        UsermodelResponce VerifySystemStaff(string Username);
        List<string> Getsystempermissiondatabyroleid(long Roleid);
    }
}
