namespace DBL.Models.Mpesa
{
    public class TransactionStatusResponseModel
    {
        public Result Result { get; set; }
    }

    public class Result
    {
        public string ConversationID { get; set; }
        public string OriginatorConversationID { get; set; }
        public ReferenceData ReferenceData { get; set; }
        public int ResultCode { get; set; }
        public string ResultDesc { get; set; }
        public ResultParameters ResultParameters { get; set; }
        public int ResultType { get; set; }
        public string TransactionID { get; set; }
    }

    public class ReferenceData
    {
        public ReferenceItem ReferenceItem { get; set; }
    }

    public class ReferenceItem
    {
        public string Key { get; set; }
    }

    public class ResultParameters
    {
        public List<ResultParameter> ResultParameter { get; set; }
    }

    public class ResultParameter
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

}
