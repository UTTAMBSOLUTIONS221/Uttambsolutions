using DBL.Entities;
using DBL.Entities.Tokenization;
using DBL.Models;

namespace DBL.Repositories
{
    public interface ISettingsRepository
    {
        #region System Permissions
        IEnumerable<Systempermissions> Getsystempermissiondata();
        Genericmodel Registersystempermissiondata(string jsonObjectdata);
        Systempermissions Getsystempermissiondatabyid(long Permissionid);
        #endregion

        #region System Services
        IEnumerable<Systemservices> Getsystemservicesdata();
        Genericmodel Registersystemservicedata(string jsonObjectdata);
        Systemservices Getsystemservicesdatabyid(long Serviceid);
        Systemservices Getsystemservicesitemsdatabyid(long Serviceid);
        #endregion

        #region Software Tokenizations
        IEnumerable<Softwaretoken> Getsystemsoftwaretokensdata();
        Genericmodel Registersoftwaretokendata(string jsonObjectdata);
        Softwaretoken Getsystemsoftwaretokensdatabyid(long Tokenid);
        Genericmodel Registersoftwaretokenpurchasedata(string jsonObjectdata);
        IEnumerable<Tokenpurchase> Getsystemsoftwaretokenspurchasedata();
        #endregion


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
