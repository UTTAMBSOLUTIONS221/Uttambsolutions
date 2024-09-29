namespace DBL.Entities
{
    public class ServiceOfferings
    {
        public int Staffserviceid { get; set; }
        public bool Iswork { get; set; }
        public int Staffid { get; set; }
        public int Servicetypeid { get; set; }
        public int Countyid { get; set; }
        public int Subcountyid { get; set; }
        public int Subcountywardid { get; set; }
        public string? Streedorlandmark { get; set; }
        public string? Contacts { get; set; }
        public string? Servicedescription { get; set; }
        public int Servicestatus { get; set; }
        public bool Isactive { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
        public List<Servicetypeitem>? Serviceitem { get; set; }
        // Constructor to initialize the Serviceitem list
        public ServiceOfferings()
        {
            Serviceitem = new List<Servicetypeitem>();
        }

        // Method to clear and add new service items
        public void UpdateServiceItems(List<dynamic> items)
        {
            // Ensure the list is initialized before performing operations
            if (Serviceitem == null)
            {
                Serviceitem = new List<Servicetypeitem>();
            }

            // Clear the existing items
            Serviceitem.Clear();

            // Add new items to the list
            foreach (var item in items)
            {
                var serviceItem = item.ToObject<Servicetypeitem>();
                Serviceitem.Add(serviceItem);
            }
        }
    }
    public class Servicetypeitem
    {
        public int Staffserviceid { get; set; }
        public int Servicetypeitemid { get; set; }
        public decimal Servicefee { get; set; }
        public bool Isfixed { get; set; }

        public int Serviceitemid { get; set; }
        public int Serviceid { get; set; }
        public string? Serviceitemname { get; set; }
        public string? Serviceitemimageurl { get; set; }
    }
}
