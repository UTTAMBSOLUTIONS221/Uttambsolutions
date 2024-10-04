namespace DBL.Entities
{
    public class Propertyhouselisting
    {
        public int Houselistingid { get; set; }
        public string Title { get; set; }
        public string Propertytype { get; set; }
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Ownershiptype { get; set; }
        public DateTime Availabilitydate { get; set; }
        public string Imageurl { get; set; }
        public string Agentcontact { get; set; }
    }

}
