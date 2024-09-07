namespace DBL.Entities
{
    public class DeviceInfoModel
    {
        public long Userid { get; set; }
        public string? Androidid { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public string? Osversion { get; set; }
        public string? Platforms { get; set; }
        public string? Devicename { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
    }
}
