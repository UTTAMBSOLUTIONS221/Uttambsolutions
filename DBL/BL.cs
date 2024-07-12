using DBL.Entities;
using DBL.Enum;
using DBL.Helpers;
using DBL.Models;
using DBL.UOW;
using Newtonsoft.Json;
using System.Text;

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
        #region System Roles
        public Task<Genericmodel> Registersystemroledata(SystemRole Obj)
        {
            return Task.Run(() =>
            {
                Obj.TenantId = 1;
                Obj.IsDefault = false;
                Obj.IsActive = true;
                Obj.IsDeleted = false;
                Obj.DateCreated = DateTime.Now;
                Obj.DateModified = DateTime.Now;
                var Resp = db.RoleRepository.Registersystemroledata(JsonConvert.SerializeObject(Obj));
                return Resp;
            });
        }
        public Task<IEnumerable<SystemRole>> Getsystemroledata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.RoleRepository.Getsystemroledata(Page, PageSize);
                return Resp;
            });
        }
        public Task<SystemRole> Getsystemroledatabyid(long Roleid)
        {
            return Task.Run(() =>
            {
                var Resp = db.RoleRepository.Getsystemroledatabyid(Roleid);
                return Resp;
            });
        }
        #endregion

        #region System Staffs
        public Task<IEnumerable<SystemStaff>> Getsystemstaffdata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemstaffdata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemstaffdata(SystemStaff obj)
        {
            return Task.Run(() =>
            {
                string Passwordhash = str.RandomString(12);
                string Password = str.RandomString(8).ToString();
                obj.Passwords = sec.Encrypt(Password, Passwordhash);
                obj.Passharsh = Passwordhash;
                obj.Username = obj.Emailaddress;
                obj.Datecreated = DateTime.Now;
                obj.Datemodified = DateTime.Now;
                obj.Passwordresetdate = DateTime.Now.AddDays(90);
                var Resp = db.AccountRepository.Registersystemstaffdata(JsonConvert.SerializeObject(obj));
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemportalstaffdata(SystemStaff obj)
        {
            return Task.Run(() =>
            {
                string Passwordhash = str.RandomString(12);
                string Password = str.RandomString(8).ToString();
                obj.Passwords = sec.Encrypt(Password, Passwordhash);
                obj.Passharsh = Passwordhash;
                obj.Username = obj.Emailaddress;
                obj.Datecreated = DateTime.Now;
                obj.Datemodified = DateTime.Now;
                obj.Passwordresetdate = DateTime.Now.AddDays(90);
                var Resp = db.AccountRepository.Registersystemstaffdata(JsonConvert.SerializeObject(obj));
                return Resp;
            });
        }
        public Task<SystemStaff> Getsystemstaffdatabyid(long Staffid)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemstaffdatabyid(Staffid);
                return Resp;
            });
        }

        public Task<SystemUserProfileData> Getsystemuserprofiledata(long Userid)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemuserprofiledata(Userid);
                return Resp;
            });
        }
        #endregion

        #region Verify System Staff Forgot Password
        public Task<Genericmodel> ValidateSystemForgotpasswordStaff(string userName)
        {
            return Task.Run(() =>
            {
                Genericmodel model = new Genericmodel();
                var resp = db.AccountRepository.VerifySystemStaff(userName);
                if (resp.RespStatus == 0)
                {
                    //send email for reseting password

                    var commtempdata = db.SettingsRepository.Getsystemcommunicationtemplatedatabyname(true, "Staffforgotpasswordtemplate");
                    if (commtempdata != null)
                    {
                        StringBuilder StrBodyEmail = new StringBuilder(commtempdata.Templatebody);
                        StrBodyEmail.Replace("@CompanyName", "Uttamb Solutions");
                        StrBodyEmail.Replace("@CompanyEmail", "contact@uttambsolutions.com");
                        StrBodyEmail.Replace("@Fullname", resp.Usermodel.Fullname);
                        StrBodyEmail.Replace("@Username", resp.Usermodel.Username);
                        StrBodyEmail.Replace("@CurrentYear", DateTime.Now.Year.ToString());
                        string message = StrBodyEmail.ToString();
                        //log Email Messages
                        EmailLogs Logs = new EmailLogs
                        {
                            EmailLogId = 0,
                            ModuleId = 1,
                            EmailAddress = userName,
                            EmailSubject = commtempdata.Templatesubject,
                            EmailMessage = message,
                            IsEmailSent = false,
                            DateTimeSent = DateTime.Now,
                            Datecreated = DateTime.Now,
                        };
                        var respdata = db.SettingsRepository.LogEmailMessage(JsonConvert.SerializeObject(Logs));
                        bool data = emlsnd.UttambsolutionssendemailAsync(resp.Usermodel.Emailaddress, commtempdata.Templatesubject, message, true, "", "", "");
                        if (data)
                        {
                            model.RespStatus = 0;
                            model.RespMessage = "Email Sent";
                            //Update Email is sent 
                            EmailLogs Logs1 = new EmailLogs
                            {
                                EmailLogId = Convert.ToInt64(respdata.Data1),
                                ModuleId = 1,
                                EmailAddress = userName,
                                EmailSubject = commtempdata.Templatesubject,
                                EmailMessage = message,
                                IsEmailSent = true,
                                DateTimeSent = DateTime.Now,
                                Datecreated = DateTime.Now,
                            };
                            var resp1 = db.SettingsRepository.LogEmailMessage(JsonConvert.SerializeObject(Logs1));
                        }
                        else
                        {
                            model.RespStatus = 1;
                            model.RespMessage = "Email not Sent";
                        }
                    }
                    else
                    {
                        model.RespStatus = 1;
                        model.RespMessage = "Template not found!";
                    }

                }
                else
                {
                    model.RespStatus = 1;
                    model.RespMessage = resp.RespMessage;
                }
                return model;
            });
        }
        #endregion

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

        #region Communication Templates
        public Task<IEnumerable<Communicationtemplate>> Getsystemcommunicationtemplatedata()
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Getsystemcommunicationtemplatedata();
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemcommunicationtemplatedata(string obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Registersystemcommunicationtemplatedata(obj);
                return Resp;
            });
        }
        public Task<Communicationtemplate> Getsystemcommunicationtemplatedatabyid(long TemplateId)
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Getsystemcommunicationtemplatedatabyid(TemplateId);
                return Resp;
            });
        }
        #endregion

        #region System Orgnizations
        public Task<Genericmodel> Registersystemorganizationdata(string obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Registersystemorganizationdata(obj);
                return Resp;
            });
        }
        public Task<SystemOrganization> Getsystemorganizationdatabyid(long Organizationid)
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Getsystemorganizationdatabyid(Organizationid);
                return Resp;
            });
        }
        public Task<SystemOrganizationDetails> Getsystemorganizationdetaildatabyid(long Organizationid)
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Getsystemorganizationdetaildatabyid(Organizationid);
                return Resp;
            });
        }
        public Task<Organizationshopproductsdata> Registerorganizationshopproductdata(string obj)
        {
            return Task.Run(() =>
            {
                var respData = db.OrganizationRepository.Registerorganizationshopproductdata(obj);
                return respData;
            });
        }
        public Task<Organizationshopproducts> Getorganizationshopproductdatabyid(long Shopproductid)
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Getorganizationshopproductdatabyid(Shopproductid);
                return Resp;
            });
        }
        public Task<Systemorganizationshopproducts> Getsystemorganizationshopproductsdata()
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Getsystemorganizationshopproductsdata();
                return Resp;
            });
        }
        public Task<Organizationshopproductsdata> Getsystemorganizationshopproductsdatabyid(long Shopproductid)
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Getsystemorganizationshopproductsdatabyid(Shopproductid);
                return Resp;
            });
        }

        #endregion

        #region System Blog Category
        public Task<IEnumerable<Systemblogcategories>> Getsystemblogcategorydata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogcategoryRepository.Getsystemblogcategorydata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemblogcategorydata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogcategoryRepository.Registersystemblogcategorydata(Obj);
                return Resp;
            });
        }
        public Task<Systemblogcategories> Getsystemblogcategorydatabyid(long Blogcategoryid)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogcategoryRepository.Getsystemblogcategorydatabyid(Blogcategoryid);
                return Resp;
            });
        }

        public Task<Genericmodel> RetrieveandSaveBlogs(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Registersystemserverblogdata(Obj);
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

        #region System Blogs
        public Task<Systemblogdata> Getsystemallblogdata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Getsystemallblogdata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemblogdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Registersystemblogdata(Obj);
                return Resp;
            });
        }
        #endregion

        #region System Product Brands
        public Task<Genericmodel> Registersystemproductbranddata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.BrandRepository.Registersystemproductbranddata(Obj);
                return Resp;
            });
        }
        public Task<IEnumerable<Productbrand>> Getsystemproductbranddata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.BrandRepository.Getsystemproductbranddata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Productbrand> Getsystemproductbranddatabyid(long Brandid)
        {
            return Task.Run(() =>
            {
                var Resp = db.BrandRepository.Getsystemproductbranddatabyid(Brandid);
                return Resp;
            });
        }
        #endregion

        #region System Category
        public Task<Genericmodel> Registersystemcategorydata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.CategoryRepository.Registersystemcategorydata(Obj);
                return Resp;
            });
        }
        public Task<IEnumerable<Productcategories>> Getsystemcategorydata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.CategoryRepository.Getsystemcategorydata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Productcategories> Getsystemcategorydatabyid(long Categoryid)
        {
            return Task.Run(() =>
            {
                var Resp = db.CategoryRepository.Getsystemcategorydatabyid(Categoryid);
                return Resp;
            });
        }
        #endregion

        #region System Products
        public Task<Genericmodel> Registersystemproductdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.ProductRepository.Registersystemproductdata(Obj);
                return Resp;
            });
        }
        public Task<IEnumerable<Systemproducts>> Getsystemproductdata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.ProductRepository.Getsystemproductdata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Systemproducts> Getsystemproductdatabyid(long Productid)
        {
            return Task.Run(() =>
            {
                var Resp = db.ProductRepository.Getsystemproductdatabyid(Productid);
                return Resp;
            });
        }
        #endregion


        #region System Dropdowns
        public Task<IEnumerable<ListModel>> GetListModel(ListModelType listType)
        {
            return Task.Run(() =>
            {
                return db.GeneralRepository.GetListModel(listType);
            });
        }
        #endregion
    }
}
