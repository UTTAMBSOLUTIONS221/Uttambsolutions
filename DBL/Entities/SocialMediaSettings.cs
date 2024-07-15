namespace DBL.Entities
{
    public class SocialMediaSettings
    {
        public long SocialSettingId { get; set; }
        public long SocialOwner { get; set; }
        public string Socialpagename { get; set; }
        public string Appid { get; set; }
        public string Appsecret { get; set; }
        public string UserAccessToken { get; set; }
        public string PageAccessToken { get; set; }
        public string PageId { get; set; }
        public string PageType { get; set; }
        public string Extra { get; set; }
        public string Extra1 { get; set; }
        public string Extra2 { get; set; }
        public string Extra3 { get; set; }
        public string Extra4 { get; set; }
        public string Extra5 { get; set; }
        public string Extra6 { get; set; }
        public string Extra7 { get; set; }
        public string Extra8 { get; set; }
        public string Extra9 { get; set; }
        public string Extra10 { get; set; }
        public long CreatedBy { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
