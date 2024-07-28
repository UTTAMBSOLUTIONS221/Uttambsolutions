﻿using DBL.Models;

namespace Mainapp.Models.Startup
{
    public class UsermodelResponce : BaseResponse
    {
        public int RespStatus { get; set; }
        public string? RespMessage { get; set; }
        public string? Token { get; set; }
        public UsermodeldataResponce? Usermodel { get; set; }
    }
    public class UsermodeldataResponce
    {
        public long Userid { get; set; }
        public long Tenantid { get; set; }
        public string? Tenantname { get; set; }
        public string? Tenantsubdomain { get; set; }
        public string? TenantLogo { get; set; }
        public string? Currencyname { get; set; }
        public string? Utcname { get; set; }
        public string? Firstname { get; set; }
        public string? Fullname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Username { get; set; }
        public string? Emailaddress { get; set; }
        public int Roleid { get; set; }
        public string? Rolename { get; set; }
        public string? Passharsh { get; set; }
        public string? Passwords { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public int Loginstatus { get; set; }
        public DateTime Passwordresetdate { get; set; }
        public long Createdby { get; set; }
        public long Modifiedby { get; set; }
        public DateTime Lastlogin { get; set; }
        public DateTime Datemodified { get; set; }
        public DateTime Datecreated { get; set; }
    }
    public enum RoleDetails
    {
        Student = 1,
        Teacher,
        Admin,
    }
}
