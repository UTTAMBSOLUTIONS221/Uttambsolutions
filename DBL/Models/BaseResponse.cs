namespace DBL.Models
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
