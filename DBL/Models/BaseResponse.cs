namespace DBL.Models
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public dynamic Data { get; set; }
    }

}
