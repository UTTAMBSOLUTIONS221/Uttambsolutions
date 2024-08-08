namespace DBL.Models.Mpesa
{
    public class B2CResp
    {
        public string ResponseCode { get; set; }
        public string ResponseDesc { get; set; }

        public B2CResp()
        {
            this.ResponseCode = "00000000";
            this.ResponseDesc = "success";
        }
    }
}
