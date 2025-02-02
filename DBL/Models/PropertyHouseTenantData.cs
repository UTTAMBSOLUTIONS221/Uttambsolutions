﻿namespace DBL.Models
{
    public class PropertyHouseTenantData
    {
        public List<PropertyHouseTenant>? Data { get; set; }
    }
    public class PropertyHouseTenant
    {
        public int Systempropertyhousetenantentryid { get; set; }
        public int Systempropertyhousetenantid { get; set; }
        public string? Tenantname { get; set; }
        public int Idnumber { get; set; }
        public string? Propertyhousename { get; set; }
        public string? Propertyprimaryimage { get; set; }
        public string? Systempropertyhousesizename { get; set; }
        public bool Isoccupant { get; set; }
        public string? Occupationalstatus { get; set; }
        public DateTime Datecreated { get; set; }
        public DateTime Datemodified { get; set; }
        public decimal Walletbalance { get; set; }
    }
}
