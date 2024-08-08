namespace DBL.Models.Mpesa
{
    public class B2CResultModel
    {
        public B2CResultDataModel Result { get; set; }
    }

    public class B2CResultDataModel
    {
        public int ResultType { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public string OriginatorConversationID { get; set; }
        public string ConversationID { get; set; }
        public string TransactionID { get; set; }
        public B2CResultReferenceDataModel ReferenceData { get; set; }
        public B2CResultParametersModel ResultParameters { get; set; }
    }

    public class B2CResultReferenceDataModel
    {
        public B2CResultDatItemModel ReferenceItem { get; set; }
    }

    public class B2CResultParametersModel
    {
        public B2CResultDatItemModel[] ResultParameter { get; set; }
    }

    public class B2CResultDatItemModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
