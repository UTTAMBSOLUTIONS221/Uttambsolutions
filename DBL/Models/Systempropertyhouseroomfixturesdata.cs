namespace DBL.Models
{
    public class Systempropertyhouseroomfixturesdata
    {
        public Systempropertyhouseroomfixtures? Data { get; set; }
    }
    public class Systempropertyhouseroomfixtures
    {
        public int Systempropertyhouseroomid { get; set; }
        public int Systempropertyhouseid { get; set; }
        public List<RoomFixture>? Roomfixtures { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
    }
    public class RoomFixture
    {
        public int Propertychecklistid { get; set; }
        public int Propertyhouseroomid { get; set; }
        public int Fixturestatusid { get; set; }
        public int Fixtureunits { get; set; }
        public string? Fixturestatus { get; set; }
        public int Createdby { get; set; }
        public DateTime Datecreated { get; set; }
        public int Fixtureid { get; set; }
        public string? Fixturetype { get; set; }
        public string? Descriptions { get; set; }
        public string? Category { get; set; }
        public ListModel? SelectedFixture { get; set; }
    }

}
