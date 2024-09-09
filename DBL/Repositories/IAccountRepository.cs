using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<SystemStaff> Getsystemstaffdata(int Page, int PageSize);
        Genericmodel Registersystemuserdevicedata(string JsonData);
        Genericmodel Registersystemstaffdata(string JsonData);
        Genericmodel SaveStaffRefreshToken(string JsonData);
        SystemStaff Getsystemstaffdatabyid(long Staffid);
        Genericmodel Verifystaffaccountdatabyid(long Staffid);
        SystemUserProfileData Getsystemuserprofiledata(long Userid);
        Genericmodel Updatestaffprofilepicturedata(string JsonData);
        Genericmodel Updatestaffcurriculumdata(string JsonData);
        Systemstaffdetaildata Getsystemstaffdetaildatabyid(long Staffid);
        SystemStaff Getsystemstaffdatabyrefreshtoken(string Refreshtoken);
        Systemtenantdetailsdata Getsystemstaffdatabyidnumber(int Idnumber);
        Genericmodel Registersystemjobapplicationdata(string JsonData);
        ForgotPasswordUserResponce VerifyForgotPasswordSystemStaff(string JsonData);
        UsermodelResponce VerifySystemStaff(string Username);
        List<string> Getsystempermissiondatabyroleid(long Roleid);
    }
}
