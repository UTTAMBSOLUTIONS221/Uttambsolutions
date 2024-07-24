using DBL.Entities;
using DBL.Models;

namespace DBL.Repositories
{
    public interface ISettingsRepository
    {
        #region Communication Templates
        IEnumerable<Communicationtemplate> Getsystemcommunicationtemplatedata();
        Communicationtemplate Getsystemcommunicationtemplatedatabyname(bool Isemail, string Templatename);
        Genericmodel Registersystemcommunicationtemplatedata(string jsonObjectdata);
        Communicationtemplate Getsystemcommunicationtemplatedatabyid(long Templateid);
        #endregion

        #region Log Email Messages
        Genericmodel LogEmailMessage(string JsonEntity);
        #endregion

        #region Log User Activities
        Genericmodel Registersystemuseractivitydata(string jsonObjectdata);
        #endregion
    }
}
