namespace DBL.Entities
{
    public class Parcelcollectioncenters
    {
        public int Collectioncenterid { get; set; }
        public string? Collectionname { get; set; }
        public int Countyid { get; set; }
        public string? Countyname { get; set; }
        public int Subcountyid { get; set; }
        public string? Subcountyname { get; set; }
        public int Subcountywardid { get; set; }
        public string? Subcountywardname { get; set; }
        public string? Streetorlandmark { get; set; }
        public string? Phonenumber { get; set; }
        public string? Operatinghours { get; set; }
        public int Collectionstatus { get; set; }
        public int Managerid { get; set; }
        public string? Managername { get; set; }
    }
}
