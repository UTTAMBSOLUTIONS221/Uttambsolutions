using DBL.Entities;
using DBL.Entities.Mpesa;
using DBL.Enum;
using DBL.Helpers;
using DBL.Models;
using DBL.Models.Dashboards;
using DBL.Models.Mpesa;
using DBL.UOW;
using DBL.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        FacebookHelper facebook = new FacebookHelper();
        public string LogFile { get; set; }
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
        public Task<IEnumerable<SystemStaff>> Getsystemstaffdatabyparentid(long Parentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemstaffdatabyparentid(Parentid);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemuserdevicedata(string obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Registersystemuserdevicedata(obj);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemstaffdata(SystemStaff obj)
        {
            return Task.Run(() =>
            {
                string Passwordhash = str.RandomString(12);
                if (obj.Userid == 0)
                {
                    obj.Passwords = sec.Encrypt(obj.Passwords, Passwordhash);
                    obj.Passharsh = Passwordhash;
                    obj.Username = obj.Emailaddress;
                }
                var Resp = db.AccountRepository.Registersystemstaffdata(JsonConvert.SerializeObject(obj));
                if (Resp.RespStatus == 0)
                {
                    string passWord = sec.Decrypt(Resp.Data4, Resp.Data5);
                    string companyName = "MAQAO PLUS";
                    string companyEmail = "maqaoplus@uttambsolutions.com";
                    string companySubject = "Your Account Details - MAQAO PLUS";
                    string changePasswordUrl = "https://uttambsolutions.com/Account/changepassword"; // URL for the user to change their password
                    string logoUrl = "https://maqaoplus.uttambsolutions.com/images/maqaopluslogo.png";

                    StringBuilder accountDetailsHtml = new StringBuilder();

                    // Start of HTML structure
                    accountDetailsHtml.Append("<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>");
                    accountDetailsHtml.Append("<table style='width: 100%; max-width: 600px; margin: 0 auto; border-collapse: collapse;'>");

                    // Header with logo and email
                    accountDetailsHtml.Append("<thead style='background-color: #0a506c; color: #fff;'>");
                    accountDetailsHtml.Append("<tr>");
                    accountDetailsHtml.Append("<th rowspan='2' style='padding: 15px; text-align: left;'>");
                    accountDetailsHtml.Append($"<img src=\"{logoUrl}\" alt=\"{companyName}\" style='max-width: 120px; max-height: 120px;' />");
                    accountDetailsHtml.Append("</th>");
                    accountDetailsHtml.Append($"<th colspan='2' style='padding: 15px; text-align: right; font-size: 18px;'>{companyName}</th>");
                    accountDetailsHtml.Append("</tr>");
                    accountDetailsHtml.Append("<tr>");
                    accountDetailsHtml.Append("<th colspan='2' style='padding: 10px; text-align: right; font-size: 14px;'>");
                    accountDetailsHtml.Append($"Email: <a href='mailto:{companyEmail}' style='color: #fff; text-decoration: none;'>{companyEmail}</a>");
                    accountDetailsHtml.Append("</th>");
                    accountDetailsHtml.Append("</tr>");
                    accountDetailsHtml.Append("</thead>");

                    // Body of email with account details
                    accountDetailsHtml.Append("<tbody>");
                    accountDetailsHtml.Append("<tr>");
                    accountDetailsHtml.Append("<td colspan='2' style='padding: 20px 10px;'>");
                    accountDetailsHtml.Append("<h2>Welcome to the Team!</h2>");
                    accountDetailsHtml.Append($"<p>Hello {Resp.Data3},</p>");
                    accountDetailsHtml.Append("<p>Your staff account has been created. Below are your account details:</p>");
                    accountDetailsHtml.Append($"<p><strong>Email:</strong> {Resp.Data7}</p>");
                    accountDetailsHtml.Append($"<p><strong>Temporary Password:</strong> {passWord}</p>");
                    accountDetailsHtml.Append("<p>For security reasons, please log in and change your password immediately using the link below:</p>");
                    accountDetailsHtml.Append($"<p style='text-align: center;'><a href='{changePasswordUrl}' style='background-color: #0a506c; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Change Password</a></p>");
                    accountDetailsHtml.Append("<p>If the button doesn't work, please copy and paste the following URL into your browser:</p>");
                    accountDetailsHtml.Append($"<p><a href='{changePasswordUrl}' style='color: #0a506c;'>{changePasswordUrl}</a></p>");
                    accountDetailsHtml.Append("<p>We look forward to working with you. If you have any questions, feel free to reach out to us.</p>");
                    accountDetailsHtml.Append($"<p>Best regards,</p>");
                    accountDetailsHtml.Append($"<p><strong>{companyName} Team</strong></p>");
                    accountDetailsHtml.Append("</td>");
                    accountDetailsHtml.Append("</tr>");
                    accountDetailsHtml.Append("</tbody>");

                    // Footer Section
                    accountDetailsHtml.Append("<tfoot style='background-color: #0a506c; color: #fff;'>");
                    accountDetailsHtml.Append("<tr>");
                    accountDetailsHtml.Append("<td colspan='2' style='padding: 15px; text-align: center; font-size: 14px;'>");
                    accountDetailsHtml.Append("Uttamb Solutions &copy; 2022 - 2024");
                    accountDetailsHtml.Append("</td>");
                    accountDetailsHtml.Append("</tr>");
                    accountDetailsHtml.Append("<tr>");
                    accountDetailsHtml.Append("<td colspan='2' style='padding: 10px; text-align: center; font-size: 12px;'>");
                    accountDetailsHtml.Append("Vision: Utilizing Technology To Automate Modern Business");
                    accountDetailsHtml.Append("</td>");
                    accountDetailsHtml.Append("</tr>");
                    accountDetailsHtml.Append("<tr>");
                    accountDetailsHtml.Append("<td colspan='2' style='padding: 10px; text-align: center; font-size: 12px;'>");
                    accountDetailsHtml.Append("Mission: For Quality and Value");
                    accountDetailsHtml.Append("</td>");
                    accountDetailsHtml.Append("</tr>");
                    accountDetailsHtml.Append("</tfoot>");

                    accountDetailsHtml.Append("</table>");
                    accountDetailsHtml.Append("</div>");
                    string message = accountDetailsHtml.ToString();
                    //log Email Messages
                    EmailLogs Logs = new EmailLogs
                    {
                        EmailLogId = 0,
                        ModuleId = Convert.ToInt64(Resp.Data2),
                        EmailAddress = Resp.Data7,
                        EmailSubject = companySubject,
                        EmailMessage = message,
                        IsEmailSent = false,
                        DateTimeSent = DateTime.Now,
                        Datecreated = DateTime.Now,
                    };
                    var resp = db.SettingsRepository.LogEmailMessage(JsonConvert.SerializeObject(Logs));
                    bool data = emlsnd.UttambsolutionssendemailAsync(Resp.Data7, companySubject, message, true, "", "", "");
                    if (data)
                    {
                        Resp.RespStatus = 0;
                        Resp.RespMessage = "Email Sent";
                        //Update Email is sent 
                        EmailLogs Logs1 = new EmailLogs
                        {
                            EmailLogId = Convert.ToInt64(resp.Data1),
                            ModuleId = Convert.ToInt64(Resp.Data2),
                            EmailAddress = Resp.Data7,
                            EmailSubject = companySubject,
                            EmailMessage = message,
                            IsEmailSent = true,
                            DateTimeSent = DateTime.Now,
                            Datecreated = DateTime.Now,
                        };
                        var resp1 = db.SettingsRepository.LogEmailMessage(JsonConvert.SerializeObject(Logs1));
                    }
                    else
                    {
                        Resp.RespStatus = 1;
                        Resp.RespMessage = "Email not Sent";
                    }
                }
                else
                {
                    Resp.RespStatus = 1;
                    Resp.RespMessage = Resp.RespMessage;
                }

                return Resp;
            });
        }
        public Task<Genericmodel> SaveStaffRefreshToken(string obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.SaveStaffRefreshToken(JsonConvert.SerializeObject(obj));
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
        public Task<Genericmodel> Verifystaffaccountdatabyid(long Staffid)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Verifystaffaccountdatabyid(Staffid);
                return Resp;
            });
        }
        public Task<SystemStaff> Getsystemstaffdatabyrefreshtoken(string Refreshtoken)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemstaffdatabyrefreshtoken(Refreshtoken);
                return Resp;
            });
        }
        public Task<SystemStaffData> Getsystemstaffprofiledatabyid(long Staffid)
        {
            SystemStaffData Resp = new SystemStaffData();
            return Task.Run(() =>
            {
                Resp.Data = db.AccountRepository.Getsystemstaffdatabyid(Staffid);
                return Resp;
            });
        }
        public Task<Systemstaffdetaildata> Getsystemstaffdetaildatabyid(long Staffid)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemstaffdetaildatabyid(Staffid);
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
        public Task<Genericmodel> Updatestaffprofilepicturedata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Updatestaffprofilepicturedata(Obj);
                return Resp;
            });
        }
        public Task<Genericmodel> Updatestaffcurriculumdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Updatestaffcurriculumdata(Obj);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemjobapplicationdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Registersystemjobapplicationdata(Obj);
                return Resp;
            });
        }

        public Task<Systemtenantdetailsdata> Getsystemstaffdatabyidnumber(int Idnumber)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystemstaffdatabyidnumber(Idnumber);
                return Resp;
            });
        }
        public Task<Systemtenantdetailsdata> Getsystemstaffdetaildatabyidnumber(int Idnumber)
        {
            return Task.Run(() =>
            {
                var resp = db.AccountRepository.Getsystemstaffdatabyidnumber(Idnumber);
                return resp;
            });
        }
        #endregion

        #region Verify System Staff Forgot Password
        public Task<ForgotPasswordUserResponce> ValidateSystemForgotpasswordStaff(Forgotpassword Obj)
        {
            return Task.Run(() =>
            {
                string Passwordhash = str.RandomString(12);
                if (Obj.Userid > 0)
                {
                    Obj.Passwords = sec.Encrypt(Obj.Passwords, Passwordhash);
                    Obj.Passharsh = Passwordhash;
                }
                var resp = db.AccountRepository.VerifyForgotPasswordSystemStaff(JsonConvert.SerializeObject(Obj));

                return resp;
            });
        }

        public Task<Genericmodel> Resendstaffpassword(long StaffId)
        {
            return Task.Run(() =>
            {
                Genericmodel model = new Genericmodel();
                var Resp = db.AccountRepository.Getsystemstaffdatabyid(StaffId);
                string passWord = sec.Decrypt(Resp.Passwords, Resp.Passharsh);
                string companyname = "MAQAO PLUS";
                string companyemail = "maqaoplus@uttambsolutions.com";
                string companysubject = "Your Account Details - MAQAO PLUS";
                string changepasswordurl = "https://uttambsolutions.com/Account/changepassword"; // URL for the user to change their password
                string logoUrl = "https://maqaoplus.uttambsolutions.com/images/maqaopluslogo.png";

                StringBuilder accountDetailsHtml = new StringBuilder();

                // Start of HTML structure
                accountDetailsHtml.Append("<div style='font-family: Arial, sans-serif; color: #333; line-height: 1.6;'>");
                accountDetailsHtml.Append("<table style='width: 100%; max-width: 600px; margin: 0 auto; border-collapse: collapse;'>");

                // Header with logo and email
                accountDetailsHtml.Append("<thead style='background-color: #0a506c; color: #fff;'>");
                accountDetailsHtml.Append("<tr>");
                accountDetailsHtml.Append("<th rowspan='2' style='padding: 15px; text-align: left;'>");
                accountDetailsHtml.Append($"<img src=\"{logoUrl}\" alt=\"{companyname}\" style='max-width: 120px; max-height: 120px;' />");
                accountDetailsHtml.Append("</th>");
                accountDetailsHtml.Append($"<th colspan='2' style='padding: 15px; text-align: right; font-size: 18px;'>{companyname}</th>");
                accountDetailsHtml.Append("</tr>");
                accountDetailsHtml.Append("<tr>");
                accountDetailsHtml.Append("<th colspan='2' style='padding: 10px; text-align: right; font-size: 14px;'>");
                accountDetailsHtml.Append($"Email: <a href='mailto:{companyemail}' style='color: #fff; text-decoration: none;'>{companyemail}</a>");
                accountDetailsHtml.Append("</th>");
                accountDetailsHtml.Append("</tr>");
                accountDetailsHtml.Append("</thead>");

                // Body of email with account details
                accountDetailsHtml.Append("<tbody>");
                accountDetailsHtml.Append("<tr>");
                accountDetailsHtml.Append("<td colspan='2' style='padding: 20px 10px;'>");
                accountDetailsHtml.Append("<h2>Your Account Details</h2>");
                accountDetailsHtml.Append("<p>Hello,</p>");
                accountDetailsHtml.Append("<p>Your account has been created/updated by the admin. Below are your account details:</p>");
                accountDetailsHtml.Append($"<p><strong>Username:</strong> {Resp.Emailaddress}</p>");
                accountDetailsHtml.Append($"<p><strong>Temporary Password:</strong> {passWord}</p>");
                accountDetailsHtml.Append($"<p>For security reasons, please log in and change your password immediately using the link below:</p>");
                accountDetailsHtml.Append($"<p style='text-align: center;'><a href='{changepasswordurl}' style='background-color: #0a506c; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Change Password</a></p>");
                accountDetailsHtml.Append("<p>If the button doesn't work, please copy and paste the following URL into your browser:</p>");
                accountDetailsHtml.Append($"<p><a href='{changepasswordurl}' style='color: #0a506c;'>{changepasswordurl}</a></p>");
                accountDetailsHtml.Append("<p>Thank you,</p>");
                accountDetailsHtml.Append($"<p><strong>{companyname} Team</strong></p>");
                accountDetailsHtml.Append("</td>");
                accountDetailsHtml.Append("</tr>");
                accountDetailsHtml.Append("</tbody>");

                // Footer Section
                accountDetailsHtml.Append("<tfoot style='background-color: #0a506c; color: #fff;'>");
                accountDetailsHtml.Append("<tr>");
                accountDetailsHtml.Append("<td colspan='2' style='padding: 15px; text-align: center; font-size: 14px;'>");
                accountDetailsHtml.Append("Uttamb Solutions &copy; 2022 - 2024");
                accountDetailsHtml.Append("</td>");
                accountDetailsHtml.Append("</tr>");
                accountDetailsHtml.Append("<tr>");
                accountDetailsHtml.Append("<td colspan='2' style='padding: 10px; text-align: center; font-size: 12px;'>");
                accountDetailsHtml.Append("Vision: Utilizing Technology To Automate Modern Business");
                accountDetailsHtml.Append("</td>");
                accountDetailsHtml.Append("</tr>");
                accountDetailsHtml.Append("<tr>");
                accountDetailsHtml.Append("<td colspan='2' style='padding: 10px; text-align: center; font-size: 12px;'>");
                accountDetailsHtml.Append("Mission: For Quality and Value");
                accountDetailsHtml.Append("</td>");
                accountDetailsHtml.Append("</tr>");
                accountDetailsHtml.Append("</tfoot>");

                accountDetailsHtml.Append("</table>");
                accountDetailsHtml.Append("</div>");


                string message = accountDetailsHtml.ToString();
                //log Email Messages
                EmailLogs Logs = new EmailLogs
                {
                    EmailLogId = 0,
                    ModuleId = StaffId,
                    EmailAddress = Resp.Emailaddress,
                    EmailSubject = "Password recovery",
                    EmailMessage = message,
                    IsEmailSent = false,
                    DateTimeSent = DateTime.Now,
                    Datecreated = DateTime.Now,
                };
                var resp = db.SettingsRepository.LogEmailMessage(JsonConvert.SerializeObject(Logs));
                bool data = emlsnd.UttambsolutionssendemailAsync(Resp.Emailaddress, "Password Recovery", message, true, "", "", "");
                if (data)
                {
                    model.RespStatus = 0;
                    model.RespMessage = "Email Sent";
                    //Update Email is sent 
                    EmailLogs Logs1 = new EmailLogs
                    {
                        EmailLogId = Convert.ToInt64(resp.Data1),
                        ModuleId = StaffId,
                        EmailAddress = Resp.Emailaddress,
                        EmailSubject = "Password recovery",
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
                                Firstname = resp.Usermodel.Firstname,
                                Lastname = resp.Usermodel.Lastname,
                                Fullname = resp.Usermodel.Fullname,
                                Phonenumber = resp.Usermodel.Phonenumber,
                                Username = resp.Usermodel.Username,
                                Emailaddress = resp.Usermodel.Emailaddress,
                                Genderid = resp.Usermodel.Genderid,
                                Maritalstatusid = resp.Usermodel.Maritalstatusid,
                                Roleid = resp.Usermodel.Roleid,
                                Passharsh = resp.Usermodel.Passharsh,
                                Passwords = resp.Usermodel.Passwords,
                                Isactive = resp.Usermodel.Isactive,
                                Isdeleted = resp.Usermodel.Isdeleted,
                                Isdefault = resp.Usermodel.Isdefault,
                                Loginstatus = resp.Usermodel.Loginstatus,
                                Designation = resp.Usermodel.Designation,
                                Passwordresetdate = resp.Usermodel.Passwordresetdate,
                                Parentid = resp.Usermodel.Parentid,
                                Userprofileimageurl = resp.Usermodel.Userprofileimageurl,
                                Usercurriculumvitae = resp.Usermodel.Usercurriculumvitae,
                                Idnumber = resp.Usermodel.Idnumber,
                                Updateprofile = resp.Usermodel.Updateprofile,
                                Accountnumber = resp.Usermodel.Accountnumber,
                                Walletbalance = resp.Usermodel.Walletbalance,
                                Rolename = resp.Usermodel.Rolename,
                                RoleDescription = resp.Usermodel.RoleDescription,
                                Tenantid = resp.Usermodel.Tenantid,
                                Createdby = resp.Usermodel.Createdby,
                                Modifiedby = resp.Usermodel.Modifiedby,
                                Lastlogin = resp.Usermodel.Lastlogin,
                                Datemodified = resp.Usermodel.Datemodified,
                                Datecreated = resp.Usermodel.Datecreated,
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

        public Task<List<string>> Getsystempermissiondatabyroleid(long Roleid)
        {
            return Task.Run(() =>
            {
                var Resp = db.AccountRepository.Getsystempermissiondatabyroleid(Roleid);
                return Resp;
            });
        }
        #endregion

        #region System Permissions
        public Task<IEnumerable<Systempermissions>> Getsystempermissiondata()
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Getsystempermissiondata();
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempermissiondata(string obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Registersystempermissiondata(obj);
                return Resp;
            });
        }
        public Task<Systempermissions> Getsystempermissiondatabyid(long Permissionid)
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Getsystempermissiondatabyid(Permissionid);
                return Resp;
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

        #region System Social Medias
        public Task<IEnumerable<SocialMediaSettings>> Getsystemsocialmediadata(long UserId)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Getsystemsocialmediadata(UserId);
                return Resp;
            });
        }
        //public async Task<Genericmodel> Registersystemsocialmediapagedata(SocialMediaSettings obj)
        //{
        //    Genericmodel Resp = new Genericmodel();
        //    // Retrieve the long-lived access token
        //    if (obj.PageType == "Facebook")
        //    {
        //        FacebookExchangeTokenResponse longLivedAccessToken = await facebook.ExchangeAccessTokenAsync(obj.Appid, obj.Appsecret, obj.UserAccessToken);
        //        if (longLivedAccessToken.access_token != null)
        //        {
        //            // Retrieve the never-expiring access token
        //            FacebookNeverExpiresResponse neverExpiresAccessToken = await facebook.GenerateNeverExpiresAccessTokenAsync(longLivedAccessToken.access_token);
        //            if (neverExpiresAccessToken.Data.Any())
        //            {
        //                var matchingPage = neverExpiresAccessToken.Data.FirstOrDefault(x => x.Name.Contains(obj.Socialpagename, StringComparison.OrdinalIgnoreCase));
        //                if (matchingPage != null)
        //                {
        //                    // Set the page access token and page ID
        //                    obj.PageAccessToken = matchingPage.AccessToken;
        //                    obj.PageId = matchingPage.Id;

        //                    // Save the data
        //                    Resp = db.SocialmediaRepository.Registersystemsocialmediapagedata(JsonConvert.SerializeObject(obj));
        //                }
        //                else
        //                {
        //                    // If the page name doesn't exist, return with an error message
        //                    Resp.RespStatus = 1;
        //                    Resp.RespMessage = "Failed to find the page with the specified name. Use correct facebook Page name";
        //                }
        //            }
        //            else
        //            {
        //                Resp.RespStatus = 1;
        //                Resp.RespMessage = "Failed to generate Facebook long-lived access token.";
        //            }
        //        }
        //        else
        //        {
        //            Resp.RespStatus = 1;
        //            Resp.RespMessage = "Failed to retrieve Facebook long-lived access token.";
        //        }
        //    }
        //    else if (obj.PageType == "Linkedin")
        //    {
        //        // Set the page access token and page ID
        //        obj.PageAccessToken = Guid.NewGuid().ToString();
        //        obj.PageId = Guid.NewGuid().ToString();

        //        // Save the data
        //        Resp = db.SocialmediaRepository.Registersystemsocialmediapagedata(JsonConvert.SerializeObject(obj));
        //    }


        //    return Resp;
        //}

        public Task<Genericmodel> Registersystemsocialmediapagedata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Registersystemsocialmediapagedata(Obj);
                return Resp;
            });
        }
        public Task<SocialMediaSettings> Getsystemsocialmediadatabyid(long Socialsettingid)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Getsystemsocialmediadatabyid(Socialsettingid);
                return Resp;
            });
        }
        public Task<IEnumerable<SocialMediaSettings>> Getsystemallsocialmediadata()
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Getsystemallsocialmediadata();
                return Resp;
            });
        }
        public Task<IEnumerable<SocialMediaSettings>> Getsystemalllinkedinsocialmediadata()
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Getsystemalllinkedinsocialmediadata();
                return Resp;
            });
        }
        public Task<SocialMediaSettings> Getsystemlinkedinsocialmediadata(string Pageid)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Getsystemlinkedinsocialmediadata(Pageid);
                return Resp;
            });
        }
        public Task<Genericmodel> Updatelinkedinpagetoken(long Socialsettingid, string Appid, string AccessToken, string RefreshToken, int ExpiresIn)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Updatelinkedinpagetoken(Socialsettingid, Appid, AccessToken, RefreshToken, ExpiresIn);
                return Resp;
            });
        }
        public Task<SocialMediaSettings> Getsystemlinkedinsocialmediadatabyappid(string Appid)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Getsystemlinkedinsocialmediadatabyappid(Appid);
                return Resp;
            });
        }
        public Task<Genericmodel> Updateaccesstokenonlinkedinpagetoken(long Socialsettingid, string Appid, string AccessToken)
        {
            return Task.Run(() =>
            {
                var Resp = db.SocialmediaRepository.Updateaccesstokenonlinkedinpagetoken(Socialsettingid, Appid, AccessToken);
                return Resp;
            });
        }
        #endregion

        #region System Orgnizations
        public Task<IEnumerable<SystemOrganization>> Getsystemorganizationdata()
        {
            return Task.Run(() =>
            {
                var Resp = db.OrganizationRepository.Getsystemorganizationdata();
                return Resp;
            });
        }
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
        public Task<Systemblog> Getsystemblogdatabyid(long Blogid)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Getsystemblogdatabyid(Blogid);
                return Resp;
            });
        }
        public Task<IEnumerable<Systemblog>> Getsystemallunpublishedblogdata()
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Getsystemallunpublishedblogdata();
                return Resp;
            });
        }
        public Task<Genericmodel> Updatepublishedblogdata(long Blogid)
        {
            return Task.Run(() =>
            {
                var Resp = db.BlogsRepository.Updatepublishedblogdata(Blogid);
                return Resp;
            });
        }
        #endregion

        #region System Job Opportunities
        public Task<Systemjobdata> Getsystemallopportunitydata(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.OpportunityRepository.Getsystemallopportunitydata(Page, PageSize);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemopportunitydata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.OpportunityRepository.Registersystemopportunitydata(Obj);
                return Resp;
            });
        }
        public Task<SystemJob> Getsystemopportunitydatabyid(long Opportunityid)
        {
            return Task.Run(() =>
            {
                var Resp = db.OpportunityRepository.Getsystemopportunitydatabyid(Opportunityid);
                return Resp;
            });
        }
        public Task<IEnumerable<SystemJob>> Getsystemallunpublishedopportunitydata()
        {
            return Task.Run(() =>
            {
                var Resp = db.OpportunityRepository.Getsystemallunpublishedopportunitydata();
                return Resp;
            });
        }
        public Task<Genericmodel> Updatepublishedopportunitydata(long Opportunityid)
        {
            return Task.Run(() =>
            {
                var Resp = db.OpportunityRepository.Updatepublishedopportunitydata(Opportunityid);
                return Resp;
            });
        }
        #endregion

        #region System Properties Houses
        public Task<PropertyHouseDetailData> Getallsystempropertyvacanthouses(int Page, int PageSize)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getallsystempropertyvacanthousesdata(Page, PageSize);
                return Resp;
            });
        }
        public Task<IEnumerable<Systemproperty>> Getsystempropertyhousedata()
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedata();
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempropertyhousedata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystempropertyhousedata(Obj);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystemagentpropertyhousedata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystemagentpropertyhousedata(Obj);
                return Resp;
            });
        }
        public Task<Systemproperty> Getsystempropertyhousedatabyid(long Propertyid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedatabyid(Propertyid);
                return Resp;
            });
        }
        public Task<Systempropertydata> Getsystempropertyhousedetaildatabyid(long Propertyid)
        {
            Systempropertydata Resp = new Systempropertydata();
            return Task.Run(() =>
            {
                Resp.Data = db.PropertyRepository.Getsystempropertyhousedatabyid(Propertyid);
                return Resp;
            });
        }

        public Task<SystemPropertyHouseCareTakerData> Getsystempropertyhousecaretakerdatabyownerid(long Ownerid)
        {
            return Task.Run(() =>
            {
                return db.PropertyRepository.Getsystempropertyhousecaretakerdatabyownerid(Ownerid); ;
            });
        }
        public Task<SystemStaffData> Getsystempropertyhousecaretakerdatabyid(long Caretakerhouseid)
        {
            return Task.Run(() =>
            {
                return db.PropertyRepository.Getsystempropertyhousecaretakerdatabyid(Caretakerhouseid); ;
            });
        }
        public Task<PropertyHouseSummaryDashboard> Getsystempropertyhousedashboardsummarydatabyowner(long OwnerId)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedashboardsummarydatabyowner(OwnerId);
                return Resp;
            });
        }
        public Task<PropertyHouseSummaryDashboard> Getsystempropertyhousedashboardsummarydatabyagent(long Agentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedashboardsummarydatabyagent(Agentid);
                return Resp;
            });
        }
        public Task<Systempropertyhousedata> Getsystempropertyhousedatabyowner(long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedatabyowner(Ownerid);
                return Resp;
            });
        }
        public Task<Systempropertyhousedata> Getsystempropertyhousedatabyowner(long Ownerid, string Designation)
        {
            return Task.Run(() =>
            {
                Systempropertyhousedata Resp = new Systempropertyhousedata();
                if (Designation == "System Admin")
                {
                    Resp = db.PropertyRepository.Getallsystempropertyhousedata();
                }
                else
                {
                    Resp = db.PropertyRepository.Getsystempropertyhousedatabyowner(Ownerid);
                }
                return Resp;
            });
        }
        public Task<Systempropertyhousedata> Getsystempropertyhousedatabyagent(long Agentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedatabyagent(Agentid);
                return Resp;
            });
        }

        public Task<PropertyHouseRoomTenantModel> Getsystempropertyhousetenantdatabytenantid(long TenantId)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousetenantdatabytenantid(TenantId);
                return Resp;
            });
        }

        public Task<PropertyHouseTenantData> Getsystempropertyhouseroomtenantsdata(long OwnerId)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseroomtenantsdata(OwnerId);
                return Resp;
            });
        }
        public Task<PropertyHouseTenantData> Getsystempropertyhouseroomtenantsdata(long OwnerId, string Designation)
        {
            return Task.Run(() =>
            {
                PropertyHouseTenantData Resp = new PropertyHouseTenantData();
                if (Designation == "System Admin")
                {
                    Resp = db.PropertyRepository.Getsystempropertyhouseroomtenantsdata();
                }
                else
                {
                    Resp = db.PropertyRepository.Getsystempropertyhouseroomtenantsdata(OwnerId);
                }
                return Resp;
            });
        }
        public Task<PropertyHouseTenantData> Getsystemagentpropertyhouseroomtenantsdata(long Agentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystemagentpropertyhouseroomtenantsdata(Agentid);
                return Resp;
            });
        }
        public Task<PropertyHouseDetailData> Getsystempropertyhousedetaildatabypropertyidandownerid(long Propertyid, long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedetaildatabypropertyidandownerid(Propertyid, Ownerid);
                return Resp;
            });
        }
        public Task<OwnerTenantAgreementDetailDataModel> Getsystempropertyhouseagreementdetaildatabyownerid(long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseagreementdetaildatabyownerid(Ownerid);
                return Resp;
            });
        }
        public Task<OwnerTenantAgreementDetailDataModel> Getsystempropertyhouseagreementdetaildatabyagentid(long Agentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseagreementdetaildatabyagentid(Agentid);
                return Resp;
            });
        }
        public Task<TenantAgreementDetailDataModel> Getsystempropertyhouseroomagreementdetaildatabytenantid(long Tenantid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseroomagreementdetaildatabytenantid(Tenantid);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempropertyhouseagreementdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystempropertyhouseagreementdata(Obj);
                return Resp;
            });
        }

        public Task<PropertyHouseDetailData> Getsystempropertyhousedetaildatabyownerid(long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedetaildatabyownerid(Ownerid);
                return Resp;
            });
        }

        public Task<PropertyHouseDetailData> Getsystempropertyhousedetaildatabyhouseid(long Propertyhouseid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhousedetaildatabyhouseid(Propertyhouseid);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempropertyhouseroomtenantdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystempropertyhouseroomtenantdata(Obj);
                return Resp;
            });
        }

        public Task<Systempropertyhouseroomdata> Getsystempropertyhouseroomdatabyid(long Houseroomid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseroomdatabyid(Houseroomid);
                return Resp;
            });
        }

        public Task<Genericmodel> Registerpropertyhouseroomdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registerpropertyhouseroomdata(Obj);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempropertyhouseroommeterdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystempropertyhouseroommeterdata(Obj);
                return Resp;
            });
        }
        public Task<Systempropertyhouseroomfixturesdata> Getsystempropertyhouseroomfixturesdatabyhouseroomid(long Houseroomid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseroomfixturesdatabyhouseroomid(Houseroomid);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempropertyhouseroomfixturedata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystempropertyhouseroomfixturedata(Obj);
                return Resp;
            });
        }
        public Task<Genericmodel> Registersystempropertyhouseroomimagedata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registersystempropertyhouseroomimagedata(Obj);
                return Resp;
            });
        }
        public Task<SystemPropertyHouseImageData> Getsystempropertyimagebyhouseroomid(long Houseroomid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyimagebyhouseroomid(Houseroomid);
                return Resp;
            });
        }
        public Task<SystemPropertyHouseImageData> Getsystempropertyimagebyhouseid(long Houseid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyimagebyhouseid(Houseid);
                return Resp;
            });
        }
        public Task<Genericmodel> Registerpropertyhousevacaterequestdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registerpropertyhousevacaterequestdata(Obj);
                return Resp;
            });
        }

        public Task<Systempropertyhouseroommeters> Getsystempropertyhouseroommeterdatabyid(long Houseroomid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyhouseroommeterdatabyid(Houseroomid);
                return Resp;
            });
        }
        public Task<SystemPropertyHouseVacatingRequestModel> Gettenantvacatingrequestsdatabyownerid(long Ownerid, string Designation)
        {
            return Task.Run(() =>
            {
                SystemPropertyHouseVacatingRequestModel Resp = new SystemPropertyHouseVacatingRequestModel();
                if (Designation == "System Admin")
                {
                    Resp = db.PropertyRepository.Gettenantvacatingrequestsdatabyownerid();
                }
                else
                {
                    Resp = db.PropertyRepository.Gettenantvacatingrequestsdatabyownerid(Ownerid);
                }
                return Resp;
            });
        }
        public Task<SystemPropertyHouseVacatingRequestModel> Gettenantvacatingrequestsdatabyownerid(long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantvacatingrequestsdatabyownerid(Ownerid);
                return Resp;
            });
        }
        public Task<Genericmodel> Approvepropertyhousevacatingrequest(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Approvepropertyhousevacatingrequest(Obj);
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoiceData> Gettenantmonthlyinvoicedatabyownerid(long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicedatabyownerid(Ownerid);
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoiceData> Gettenantmonthlyinvoicedatabyagentid(long Agentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicedatabyagentid(Agentid);
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoiceData> Gettenantmonthlyinvoicedatabytenantid(long Tenantid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicedatabytenantid(Tenantid);
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoiceDetailData> Gettenantmonthlyinvoicedetaildatabyinvoiceid(long InvoiceId)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicedetaildatabyinvoiceid(InvoiceId);
                return Resp;
            });
        }
        public Task<Genericmodel> Registerpropertyhouseroomrentpaymentrequestdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registerpropertyhouseroomrentpaymentrequestdata(Obj);
                return Resp;
            });
        }

        public Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabyownerid(long Ownerid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicepaymentdatabyownerid(Ownerid);
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabyownerid(long Ownerid, string Designation)
        {
            return Task.Run(() =>
            {
                TenantMonthlyInvoicePaymentData Resp = new TenantMonthlyInvoicePaymentData();
                if (Designation == "System Admin")
                {
                    Resp = db.PropertyRepository.Gettenantmonthlyinvoicepaymentdatabyownerid();
                }
                else
                {
                    Resp = db.PropertyRepository.Gettenantmonthlyinvoicepaymentdatabyownerid(Ownerid);
                }
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabyagentid(long Agentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicepaymentdatabyagentid(Agentid);
                return Resp;
            });
        }
        public Task<TenantMonthlyInvoicePaymentData> Gettenantmonthlyinvoicepaymentdatabytenantid(long Tenantid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Gettenantmonthlyinvoicepaymentdatabytenantid(Tenantid);
                return Resp;
            });
        }

        public Task<CustomerPaymentValidationData> Getsystempropertyroompaymentbypaymentid(long Paymentid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Getsystempropertyroompaymentbypaymentid(Paymentid);
                return Resp;
            });
        }
        public Task<Genericmodel> Registervalidatecustomerpaymentrequestdata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Registervalidatecustomerpaymentrequestdata(Obj);
                return Resp;
            });
        }

        //public Task<IEnumerable<SystemJob>> Getsystemallunpublishedopportunitydata()
        //{
        //    return Task.Run(() =>
        //    {
        //        var Resp = db.OpportunityRepository.Getsystemallunpublishedopportunitydata();
        //        return Resp;
        //    });
        //}
        //public Task<Genericmodel> Updatepublishedopportunitydata(long Opportunityid)
        //{
        //    return Task.Run(() =>
        //    {
        //        var Resp = db.OpportunityRepository.Updatepublishedopportunitydata(Opportunityid);
        //        return Resp;
        //    });
        //}
        #endregion

        #region System Dropdowns
        public Task<IEnumerable<ListModel>> GetListModel(ListModelType listType)
        {
            return Task.Run(() =>
            {
                return db.GeneralRepository.GetListModel(listType);
            });
        }
        public Task<IEnumerable<ListModel>> GetListModelById(ListModelType listType, long Id)
        {
            return Task.Run(() =>
            {
                return db.GeneralRepository.GetListModelbycode(listType, Id);
            });
        }
        #endregion

        #region System Other Methods
        public Task<Genericmodel> Logsystemuseractivitydata(string Obj)
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.Registersystemuseractivitydata(Obj);
                return Resp;
            });
        }
        #endregion

        #region Mpesa Processes

        #region Stk Call Back
        public void ProcessMPesaSTKCallback(int serviceCode, string content)
        {
            Task.Run(() =>
            {
                Genericmodel result = null;
                ExprCallbackModel dataModel = JsonConvert.DeserializeObject<ExprCallbackModel>(content);
                if (dataModel != null)
                {
                    ExprCallbackDataModel callbackData = new ExprCallbackDataModel
                    {
                        CheckoutRequestID = dataModel.CallbackBody.CallbackContent.CheckoutRequestID,
                        ResultCode = dataModel.CallbackBody.CallbackContent.ResultCode,
                        ResultDesc = dataModel.CallbackBody.CallbackContent.ResultDesc,
                        CustomerDets = ""
                    };

                    if (dataModel.CallbackBody.CallbackContent.ResultCode == 0)
                    {
                        var item = dataModel.CallbackBody.CallbackContent.CallbackData.CallbackValues
                        .Where(x => x.ItemName == "MpesaReceiptNumber").FirstOrDefault();
                        if (item != null)
                            callbackData.RefNo = item.ItemValue;

                        item = dataModel.CallbackBody.CallbackContent.CallbackData.CallbackValues
                        .Where(x => x.ItemName == "TransactionDate").FirstOrDefault();
                        if (item != null)
                            callbackData.TxnDate = item.ItemValue;

                        item = dataModel.CallbackBody.CallbackContent.CallbackData.CallbackValues
                        .Where(x => x.ItemName == "PhoneNumber").FirstOrDefault();
                        if (item != null)
                            callbackData.PhoneNo = item.ItemValue;

                        item = dataModel.CallbackBody.CallbackContent.CallbackData.CallbackValues
                        .Where(x => x.ItemName == "Amount").FirstOrDefault();
                        if (item != null)
                            callbackData.Amount = Convert.ToDecimal(item.ItemValue);
                    }

                    result = db.PaymentRepository.ProcessExprCallback(serviceCode, callbackData);
                    db.Reset();

                    if (result.RespStatus == 0)
                    {
                        //---- Notify 3rd party client
                        if (!string.IsNullOrEmpty(result.Data1))
                        {
                            PaymentNotificationData notificationData = new PaymentNotificationData
                            {
                                AccountBalance = callbackData.Balance,
                                PayAccountNo = result.Data4,
                                Amount = callbackData.Amount,
                                CustomerName = callbackData.CustomerDets,
                                CustomerNo = callbackData.PhoneNo,
                                ReferenceNo = callbackData.RefNo,
                                SourceRef = result.Data3
                            };

                            //---- Update 3rd party application
                            //SendPaymentNotifTo3P(result.Data1, notificationData, result.Data2);
                        }

                    }
                }
            });
        }
        #endregion


        #region Stk push
        public async Task<PayResponse> MakeExpressPayment(PesaAppRequestData requestData)
        {
            return await Task.Run(() =>
            {
                PayResponse resp = new PayResponse { Status = 1, Message = "Request was not processed!" };

                //---- Get service settings
                var settings = db.PaymentRepository.GetExprSettings(requestData.ServiceCode);
                if (settings.RespStatus != 0)
                {
                    resp.Message = settings.RespMessage;
                    return resp;
                }

                string payUrl = settings.Data1;
                string authUrl = settings.Data2;
                string consumerKey = settings.Data3;
                string consumerSecret = settings.Data4;
                string passKey = settings.Data5;
                string callbackUrl = settings.Data6 + requestData.ServiceCode;
                string shortCode = settings.Data7;
                string txnType = settings.Data8;
                string txnDescr = settings.Data9;
                string partyB = settings.Data10;

                string txnRef = "";
                string newRef = "";
                string message = "";
                int status = 0;

                //---- Get Mpesa auth token
                MPesaApi mpesaApi = new MPesaApi();
                var authToken = mpesaApi.GetMPesaAuthToken(authUrl, consumerKey, consumerSecret);
                if (string.IsNullOrEmpty(authToken))
                    return new PayResponse
                    {
                        Status = 1,
                        Message = "Failed to generate M-Pesa authorization details!"
                    };

                var paymentData = JsonConvert.DeserializeObject<MakePaymentData>(new JObject(requestData.Data).ToString());

                //---- Loop through payments
                Parallel.ForEach(paymentData.Payments, p =>
                {
                    string phoneNo = Util.FormatPhoneNo(p.AccountNo, "254").Replace("+", "");

                    //---- Save the payment to the DB
                    Payment payment = new Payment();
                    payment.ServiceCode = requestData.ServiceCode;
                    payment.AccountNo = phoneNo;
                    payment.AccountName = "";
                    payment.Amount = p.Amount;
                    payment.PType = (int)PaymentType.Express;
                    payment.TPRef = p.RefNo;
                    payment.TPStat = 2;
                    payment.ExtRef = "";
                    payment.Extra1 = p.AccountRef;
                    payment.Extra2 = paymentData.BatchNo;
                    payment.AppCode = requestData.AppCode;
                    payment.PStatus = 0;//--- Pending

                    //---- Create payment
                    var result = db.PaymentRepository.CreatePayment(payment);
                    db.Reset();

                    if (result.RespStatus == 0)
                    {
                        txnRef = result.Data1;

                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string password = shortCode + passKey + timestamp;

                        //----Encode the password to Base64
                        Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                        Encoding utf8 = Encoding.UTF8;
                        byte[] utfBytes = utf8.GetBytes(password);
                        byte[] isoBytes = Encoding.Convert(Encoding.UTF8, iso, utfBytes);
                        password = Convert.ToBase64String(isoBytes);

                        //---- Initiate payment to M-Pesa
                        ExprPaymentData exprData = new ExprPaymentData
                        {
                            Amount = Math.Round(p.Amount, 0).ToString(),
                            AccountReference = p.AccountRef,
                            BusinessShortCode = shortCode,
                            CallBackURL = callbackUrl,
                            PartyA = phoneNo,
                            PartyB = partyB,
                            Password = password,
                            PhoneNumber = phoneNo,
                            Timestamp = timestamp,
                            TransactionDesc = txnDescr,
                            TransactionType = txnType
                        };

                        //---- Log data
                        string myData = JsonConvert.SerializeObject(exprData);
                        Util.LogError(this.LogFile, "Bl.MakeExpressPayment", new Exception(myData), false);

                        var payResp = mpesaApi.MakeExprPayment(payUrl, exprData, authToken);
                        //---- Update DB
                        if (payResp.Status == ResponseStatus.Success)
                        {
                            status = 1;
                            newRef = (string)payResp.Data;

                            resp.Status = 0;
                            resp.Message = "Payment initiated successfully.";
                        }
                        else
                        {
                            status = 3;
                            message = payResp.Message.Replace("Error!", "").Trim();

                            resp.Message = "Initiating payment record failed!";
                        }

                        var updateResp = db.PaymentRepository.UpdateMPesa(txnRef, status, message, newRef);
                        db.Reset();
                    }
                    else
                    {
                        if (result.RespStatus == 1)
                            resp.Message = "Failed to create the payment record! " + result.RespMessage;
                        else
                            throw new Exception(result.RespMessage);
                    }
                });

                //---- Respond back to the caller

                return resp;
            });
        }
        #endregion


        public void ProcessC2BConfirmation(int serviceCode, string jsonData)
        {
            Task.Run(() =>
            {
                var results = JsonConvert.DeserializeObject<C2BConfirmData>(jsonData);

                if (results != null)
                {
                    Payment payment = new Payment();
                    payment.ServiceCode = serviceCode;
                    payment.AccountNo = results.MSISDN;
                    payment.AccountName = results.FirstName + " " + results.MiddleName + " " + results.LastName;
                    payment.Amount = results.TransAmount;
                    payment.PType = (int)PaymentType.C2B;
                    payment.TPRef = "";
                    payment.ExtRef = results.TransID;
                    payment.Extra1 = results.BillRefNumber;
                    payment.Extra2 = results.TransTime;
                    payment.Extra3 = results.BusinessShortCode;
                    payment.Extra4 = results.OrgAccountBalance + "";
                    payment.PStatus = 2;//--- Completed

                    //---- Create payment
                    var resp = db.PesaServiceRepository.CreatePayment(payment);
                    db.Reset();

                    if (resp.RespStatus == 0)
                    {
                        if (!string.IsNullOrEmpty(resp.Data1))
                        {
                            PaymentNotificationData notificationData = new PaymentNotificationData
                            {
                                AccountBalance = results.OrgAccountBalance,
                                PayAccountNo = results.BillRefNumber,
                                Amount = results.TransAmount,
                                CustomerName = results.FirstName + " " + results.MiddleName + " " + results.LastName,
                                CustomerNo = results.MSISDN,
                                ReferenceNo = results.TransID,
                                SourceRef = results.BusinessShortCode
                            };

                            //---- Update 3rd party application
                            SendPaymentNotifTo3P(resp.Data1, notificationData, resp.Data2);
                        }
                    }
                    else
                    {
                        throw new Exception(resp.RespMessage);
                    }
                }
                else
                {
                    throw new Exception("Invalid confirmation data!");
                }

            });
        }
        public void ProcessB2CResult(int serviceCode, string jsonData)
        {
            Task.Run(() =>
            {
                var results = JsonConvert.DeserializeObject<B2CResultModel>(jsonData);
                if (results != null)
                {
                    if (results.Result != null)
                    {

                        B2CResultData rData = new B2CResultData()
                        {
                            OrgRef = results.Result.ConversationID,
                            Receiver = "",
                            ReceiverReg = "",
                            ResultCode = results.Result.ResultCode.ToString(),
                            ResultDescr = results.Result.ResultDesc,
                            TxnID = results.Result.TransactionID
                        };

                        if (results.Result.ResultParameters != null)
                        {
                            var rParams = results.Result.ResultParameters.ResultParameter.ToList();

                            //----- Get charge
                            var d = rParams.Where(x => x.Key == "B2CChargesPaidAccountAvailableFunds").FirstOrDefault();
                            if (d != null)
                                rData.Charge = Convert.ToDecimal(d.Value);

                            //----- Get transaction amout
                            d = rParams.Where(x => x.Key == "TransactionAmount").FirstOrDefault();
                            if (d != null)
                                rData.TxnAmount = Convert.ToDecimal(d.Value);

                            //----- Get Receiver
                            d = rParams.Where(x => x.Key == "ReceiverPartyPublicName").FirstOrDefault();
                            if (d != null)
                                rData.Receiver = d.Value;

                            //----- Get Receiver Registered
                            d = rParams.Where(x => x.Key == "B2CRecipientIsRegisteredCustomer").FirstOrDefault();
                            if (d != null)
                                rData.ReceiverReg = d.Value;

                            //----- Get utility balance
                            d = rParams.Where(x => x.Key == "B2CUtilityAccountAvailableFunds").FirstOrDefault();
                            if (d != null)
                                rData.UtilityBalance = Convert.ToDecimal(d.Value);

                            //----- Get working balance
                            d = rParams.Where(x => x.Key == "B2CWorkingAccountAvailableFunds").FirstOrDefault();
                            if (d != null)
                                rData.WorkingBalance = Convert.ToDecimal(d.Value);

                        }

                        var resp = db.PaymentRepository.ProcessB2CResult(serviceCode, rData);
                        db.Reset();

                        if (resp.RespStatus == 0)
                        {
                            string url = resp.Data1;
                            if (!string.IsNullOrEmpty(url))
                            {
                                string custName = rData.Receiver;
                                string custPhone = "";
                                string[] custData = rData.Receiver.Split('-');
                                if (custData.Length >= 2)
                                {
                                    custPhone = custData[0].Trim();
                                    custName = rData.Receiver.Replace(custPhone, "").Trim();
                                }

                                PaymentNotificationData notificationData = new PaymentNotificationData
                                {
                                    AccountBalance = rData.WorkingBalance,
                                    PayAccountNo = "",
                                    Amount = rData.TxnAmount,
                                    CustomerName = custName,
                                    CustomerNo = custPhone,
                                    ReferenceNo = results.Result.TransactionID,
                                    SourceRef = resp.Data3
                                };

                                //---- Update 3rd party application
                                SendPaymentNotifTo3P(url, notificationData, resp.Data2);
                            }
                        }
                    }
                }
            });
        }
        private void SendPaymentNotifTo3P(string url, PaymentNotificationData notifData, string paymentRef)
        {
            //---- Update 3rd party application
            HttpClient httpClient = new HttpClient(url, HttpClient.RequestType.Post);
            string jsonData = JsonConvert.SerializeObject(notifData);

            Exception ex;
            int status = 0;
            string message = "";
            var notifResp = httpClient.SendRequest(jsonData, out ex);
            if (ex != null)
            {
                status = 2;
                message = ex.Message;
            }
            else
            {
                var respData = JsonConvert.DeserializeObject<ThirdPartyPaymentResponse>(notifResp);
                if (respData == null)
                {
                    status = 2;
                    message = "Failed to understand the received response!";
                }
                else
                {
                    status = respData.Status == 0 ? 1 : 2;
                    message = respData.Message;
                }
            }

            //---- Update payment status
            db.PesaServiceRepository.UpdatePayment3PStatus(paymentRef, status, message);
        }
        #endregion

        #region Email Template
        public string GenerateEmailBody(string logoUrl, string companyName, string companyEmail, string tableContent, string currentYear)
        {
            StringBuilder body = new StringBuilder();

            // Start of the email content
            body.AppendLine("<div style=\"font-family: Arial, sans-serif; color: #333; line-height: 1.6;\">");

            // Header section with logo and company details
            body.AppendLine("<table style=\"width: 100%; max-width: 600px; margin: 0 auto; border-collapse: collapse;\">");
            body.AppendLine("<thead style=\"background-color: #0a506c; color: #fff;\">");
            body.AppendLine("<tr>");
            body.AppendLine($"<th rowspan=\"2\" style=\"padding: 15px; text-align: left;\">");
            body.AppendLine($"<img src=\"{logoUrl}\" alt=\"{companyName} Logo\" style=\"max-width: 120px; max-height: 120px;\" />");
            body.AppendLine("</th>");
            body.AppendLine($"<th colspan=\"2\" style=\"padding: 15px; text-align: right; font-size: 18px;\">{companyName}</th>");
            body.AppendLine("</tr>");
            body.AppendLine($"<tr>");
            body.AppendLine($"<th colspan=\"2\" style=\"padding: 10px; text-align: right; font-size: 14px;\">Email: <a href=\"mailto:{companyEmail}\" style=\"color: #fff; text-decoration: none;\">{companyEmail}</a></th>");
            body.AppendLine("</tr>");
            body.AppendLine("</thead>");

            body.AppendLine(tableContent);

            // Footer section with company information and branding
            body.AppendLine("<tfoot style=\"background-color: #0a506c; color: #fff;\">");
            body.AppendLine("<tr>");
            body.AppendLine($"<td colspan=\"3\" style=\"padding: 15px; text-align: center; font-size: 14px;\">");
            body.AppendLine($"{companyName} &copy; 2022 - {currentYear}");
            body.AppendLine("</td>");
            body.AppendLine("</tr>");
            body.AppendLine("<tr>");
            body.AppendLine("<td colspan=\"3\" style=\"padding: 10px; text-align: center; font-size: 12px;\">Vision: Utilizing Technology To Automate Modern Business</td>");
            body.AppendLine("</tr>");
            body.AppendLine("<tr>");
            body.AppendLine("<td colspan=\"3\" style=\"padding: 10px; text-align: center; font-size: 12px;\">Mission: For Quality and Value</td>");
            body.AppendLine("</tr>");
            body.AppendLine("</tfoot>");
            body.AppendLine("</table>");

            // End of the email content
            body.AppendLine("</div>");

            return body.ToString();
        }
        public Task<Genericmodel> LogEmailMessage(EmailLogs Logs)
        {
            return Task.Run(() =>
            {
                var Resp = db.SettingsRepository.LogEmailMessage(JsonConvert.SerializeObject(Logs));
                return Resp;
            });
        }
        #endregion

        #region Usent Email Address
        public Task<Monthlyrentinvoicedata> Getsystemunsentemaildata()
        {
            return Task.Run(() =>
            {
                var Resp = db.CronjobsRepository.Getsystemunsentemaildata();
                return Resp;
            });
        }
        #endregion

        #region Cron Jobs
        public Task<Genericmodel> Generatemonthlyrentinvoicedata()
        {
            return Task.Run(() =>
            {
                var Resp = db.CronjobsRepository.Generatemonthlyrentinvoicedata();
                return Resp;
            });
        }
        public Task<Genericmodel> Updatemonthlyrentinvoicedata(long Invoiceid)
        {
            return Task.Run(() =>
            {
                var Resp = db.PropertyRepository.Updatemonthlyrentinvoicedata(Invoiceid);
                return Resp;
            });
        }
        #endregion

        #region Send Email 
        public Task<bool> Uttambsolutionssendemail(string Emailaddress, string Subject, string Message, bool Isbody, string idontknow, string idontknow1, string idontknow2)
        {
            return Task.Run(() =>
            {
                bool Resp = emlsnd.UttambsolutionssendemailAsync(Emailaddress, Subject, Message, Isbody, idontknow, idontknow1, idontknow2);
                return Resp;
            });
        }
        #endregion
    }
}
