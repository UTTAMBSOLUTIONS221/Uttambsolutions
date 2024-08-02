namespace DBL.Models
{
    public class PropertyHouseDetailData
    {
        public List<PropertyHouseDetails>? Data { get; set; }
    }
    public class PropertyHouseDetails
    {
        public int Systempropertyhouseroomid { get; set; }
        public string? Primaryimageurl { get; set; }
        public string? Propertyhouseownername { get; set; }
        public string? Propertyhousename { get; set; }
        public string? Countyname { get; set; }
        public string? Subcountyname { get; set; }
        public string? Subcountywardname { get; set; }
        public string? Streetorlandmark { get; set; }
        public string? Propertyhousestatusdata { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public string? Systemhousesizename { get; set; }
        public decimal Systempropertyhousesizerent { get; set; }
        public bool Systempropertyhousesizedeposit { get; set; }
        public bool Isvacant { get; set; }
        public string? Propertyhousevacant { get; set; }
        public string? Propertyhouseunderrenovation { get; set; }
        public string? Propertyhouseshop { get; set; }
        public string? Propertyhousegroundfloor { get; set; }
    }

}
