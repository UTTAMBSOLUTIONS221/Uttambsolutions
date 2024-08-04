namespace DBL.Entities
{
    public class Systempropertyhouserooms
    {
        public long Systempropertyhouseroomid { get; set; }
        public long Systempropertyhousesizeid { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public bool Isvacant { get; set; }
        public bool Isunderrenovation { get; set; }
        public bool Isshop { get; set; }
        public bool Isgroundfloor { get; set; }
        public bool Hasbalcony { get; set; }
        public bool Forcaretaker { get; set; }
        public bool Kitchentypeid { get; set; }
    }
}
