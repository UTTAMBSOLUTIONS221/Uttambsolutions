namespace DBL.Enum
{
    public enum UserLoginStatus { Ok = 0, VerifyAccount = 1, ChangePassword = 2, AccountClosed = 3, AccountLocked = 3 }
    public enum PaymentType { B2C = 1, C2B = 2, Express = 3 }

    public static class Systemmodulelogos
    {
        public const string Maqaoplus = "/path/to/logo.png";
    }

    public static class Systemmoduleemails
    {
        public const string Maqaoplus = "support@uttambsolutions.com";
    }


    public enum ResponseStatus
    {
        Success = 0,
        Warning = 1,
        Error = 2,
        Unknown = 3
    }
    public enum ListModelType
    {
        SystemRoles = 0,
        SystemPhoneCodes = 1,
        SystemOutlets = 2,
        SystemModules = 3,
        SystemCategory = 4,
        SystemBlogCategory = 5,
        Systemproductbrand = 6,
        SystemProductSubCategory = 7,
        Systempermission = 8,
        SystemCounty = 9,
        SystemSubCounty = 91,
        SystemSubCountyWard = 911,
        SystemChurch = 11,
        SystemChurchBranch = 12,
        Systemjobfunction = 13,
        Systemjobindustry = 14,
        Systemjoblocation = 15,
        Systemjobexperience = 16,
        Systemjobtypeid = 17,
        SystemOrganization = 18,
        SystemHouseBenefits = 19,
        SystemHouseItems = 20,
        SystemHouseDepositfees = 21,
        Systempropertyhousesizes = 22,
        Systemhousewatertype = 23,
        Systemkitchentype = 24,
        Systempaymentmodetype = 25,
        Systemcashpaymentmodetype = 215,
        Systemgender = 26,
        Systemmaritalstatus = 27,
        Systemkinrelationship = 28,
        Systempropertyhouses = 29,
        Systempropertyfixtures = 30,
        Systemservices = 31,
        Systemtokens = 32,
        SystemParcelStaffss = 33,
        ParcelSenderReciever = 34,
        Parceltypes = 35,
        Parcelstatus = 36,
        Collectioncenter = 37,
        Collectioncentercouriers = 38,
    }
}
