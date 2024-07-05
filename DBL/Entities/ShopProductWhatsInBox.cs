namespace DBL.Entities
{
    public class ShopProductWhatsInBox
    {
        public int ProductWhatsInBoxId { get; set; }
        public int ShopProductId { get; set; }
        public string? ProductWhatsInBoxItem { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
