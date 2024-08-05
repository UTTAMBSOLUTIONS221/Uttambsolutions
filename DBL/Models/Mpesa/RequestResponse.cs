using DBL.Enum;

namespace DBL.Models.Mpesa
{
    public class RequestResponse
    {
        public ResponseStatus Status { get; set; }
        public string StatusNo { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
