using DBL.Entities;
using DBL.Helpers;
using DBL.Models;
using DBL.UOW;
using Newtonsoft.Json;

namespace DBL
{
    public class BL
    {
        private UnitOfWork db;
        private string _connString;
        static bool mailSent = false;
        Encryptdecrypt sec = new Encryptdecrypt();
        Stringgenerator str = new Stringgenerator();
        EmailSenderHelper emlsnd = new EmailSenderHelper();
        public BL(string connString)
        {
            this._connString = connString;
            db = new UnitOfWork(connString);
        }
        #region Verify and Validate System Staff
        public Task<UsermodelResponce> ValidateSystemStaff(string userName, string password)
        {
            return Task.Run(() =>
            {
                UsermodelResponce userModel = new UsermodelResponce { };
                var resp = db.AccountRepository.VerifySystemStaff(userName);
                if (resp.RespStatus == 0)
                {
                    Encryptdecrypt sec = new Encryptdecrypt();
                    string descpass = sec.Decrypt(resp.Usermodel.Passwords, resp.Usermodel.Passharsh);
                    if (password == descpass)
                    {
                        userModel = new UsermodelResponce
                        {
                            RespStatus = resp.RespStatus,
                            RespMessage = resp.RespMessage,
                            Token = "",
                            Usermodel = new UsermodeldataResponce
                            {
                                Userid = resp.Usermodel.Userid,
                                Tenantid = resp.Usermodel.Tenantid,
                                Tenantname = resp.Usermodel.Tenantname,
                                Tenantsubdomain = resp.Usermodel.Tenantsubdomain,
                                TenantLogo = resp.Usermodel.TenantLogo,
                                Currencyname = resp.Usermodel.Currencyname,
                                Utcname = resp.Usermodel.Utcname,
                                Firstname = resp.Usermodel.Firstname,
                                Fullname = resp.Usermodel.Fullname,
                                Phonenumber = resp.Usermodel.Phonenumber,
                                Username = resp.Usermodel.Username,
                                Emailaddress = resp.Usermodel.Emailaddress,
                                Roleid = resp.Usermodel.Roleid,
                                Rolename = resp.Usermodel.Rolename,
                                Passharsh = resp.Usermodel.Passharsh,
                                Passwords = resp.Usermodel.Passwords,
                                Isactive = resp.Usermodel.Isactive,
                                Isdeleted = resp.Usermodel.Isdeleted,
                                Loginstatus = resp.Usermodel.Loginstatus,
                                Passwordresetdate = resp.Usermodel.Passwordresetdate,
                                Createdby = resp.Usermodel.Createdby,
                                Modifiedby = resp.Usermodel.Modifiedby,
                                Lastlogin = resp.Usermodel.Lastlogin,
                                Datemodified = resp.Usermodel.Datemodified,
                                Datecreated = resp.Usermodel.Datecreated
                            }
                        };
                        return userModel;
                    }
                    else
                    {
                        userModel.RespStatus = 1;
                        userModel.RespMessage = "Incorrect Username or Password";
                    }
                }
                else
                {
                    userModel.RespStatus = 1;
                    userModel.RespMessage = resp.RespMessage;
                }
                return userModel;
            });
        }
        #endregion

        #region System Modules
        public Task<IEnumerable<Systemmodule>> Getsystemmoduledata()
        {
            return Task.Run(() =>
            {
                var Resp = db.ModulesRepository.Getsystemmoduledata();
                return Resp;
            });
        }
        public Task<Genericmodel> Addsystemmoduledata(string obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.ModulesRepository.Registersystemmoduledata(obj);
                return Resp;
            });
        }
        public Task<Systemmodule> Getsystemmoduledatabyid(long Moduleid)
        {
            return Task.Run(() =>
            {
                var Resp = db.ModulesRepository.Getsystemmoduledatabyid(Moduleid);
                return Resp;
            });
        }
        #endregion

        #region Retrieve and save blogs
        public Task<Genericmodel> RetrieveandSaveBlogs(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Registersystemblogdata(Obj);
                return Resp;
            });
        }
        public Task<IEnumerable<Newsapiarticles>> Getsystemblogsdata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Getsystemblogsdata(Page, PageSize);
                return Resp;
            });
        }
        #endregion

    }
}
