namespace DBL.Entities
{
    public class Collectioncenterparcels
    {
        public int Parcelid { get; set; }
        public int Senderid { get; set; }
        public string? Sendername { get; set; }
        public int Receiverid { get; set; }
        public string? Recievername { get; set; }
        public int Pickupcenterid { get; set; }
        public string? Pickupcentername { get; set; }
        public int Deliverycenterid { get; set; }
        public string? Deliverycentername { get; set; }
        public int Parceltypeid { get; set; }
        public string? Parceltypename { get; set; }
        public decimal Parcelweight { get; set; }
        public string? Dimensions { get; set; }
        public int Parcelstatusid { get; set; }
        public string? Parcelstatusname { get; set; }
        public int Transitdays { get; set; }
        public decimal Deliveryfee { get; set; }
        public string? Trackingnumber { get; set; }
        public DateTime Pickupdate { get; set; }
        public DateTime Dropoffdate { get; set; }
        public DateTime Createddate { get; set; }
        public int Createdby { get; set; }
    }
}
