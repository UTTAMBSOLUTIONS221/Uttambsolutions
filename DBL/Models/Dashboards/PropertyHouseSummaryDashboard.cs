namespace DBL.Models.Dashboards
{
    public class PropertyHouseSummaryDashboard
    {
        public PropertyHouseSummary? Data { get; set; }
    }
    public class PropertyHouseSummary
    {
        public int Propertyhouseunits { get; set; }
        public int Systempropertyoccupiedroom { get; set; }
        public int Systempropertyvacantroom { get; set; }
        public decimal Expectedcollections { get; set; }
        public decimal Collectedcollections { get; set; }
        public decimal Rentarrears { get; set; }
        public decimal Uncollectedpayments { get; set; }
        public decimal Consumedmeters { get; set; }
        public List<PropertySummary>? Propertybysummary { get; set; }
    }
    public class PropertySummary
    {
        public string? Propertyhousename { get; set; }
        public int Propertyhouseunits { get; set; }
        public int Systempropertyoccupiedroom { get; set; }
        public int Systempropertyvacantroom { get; set; }
        public decimal Expectedcollections { get; set; }
        public decimal Collectedcollections { get; set; }
        public decimal Rentarrears { get; set; }
        public decimal Uncollectedpayments { get; set; }
        public decimal Consumedmeters { get; set; }
    }

}
