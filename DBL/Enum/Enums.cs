using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBL.Enum
{
    public enum UserLoginStatus { Ok = 0, VerifyAccount = 1, ChangePassword = 2, AccountClosed = 3, AccountLocked = 3 }
    public enum ListModelType
    {
        SystemRoles = 0,
        SystemPhoneCodes = 1,
        SystemOutlets = 2,
        SystemBlogModule = 3,
        SystemCategory = 4,
        SystemSubCategory = 5,
        Systemproductbrand = 6,
        SystemProductSubCategory = 7,
        Systempermission = 8,
        SystemCounty = 9,
        SystemSubCounty = 10,
        SystemChurch = 11,
        SystemChurchBranch = 12,
    }
}
