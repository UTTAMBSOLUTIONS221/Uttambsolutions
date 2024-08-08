﻿using Newtonsoft.Json;

namespace DBL.Models.Mpesa
{
    public class C2BConfirmData
    {
        [JsonProperty("TransactionType")]
        public string TransactionType { get; set; }

        [JsonProperty("TransID")]
        public string TransID { get; set; }

        [JsonProperty("TransTime")]
        public string TransTime { get; set; }

        [JsonProperty("TransAmount")]
        public decimal TransAmount { get; set; }

        [JsonProperty("BusinessShortCode")]
        public string BusinessShortCode { get; set; }

        [JsonProperty("BillRefNumber")]
        public string BillRefNumber { get; set; }

        [JsonProperty("InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("OrgAccountBalance")]
        public decimal OrgAccountBalance { get; set; }

        [JsonProperty("ThirdPartyTransID")]
        public string ThirdPartyTransID { get; set; }

        [JsonProperty("MSISDN")]
        public string MSISDN { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("MiddleName")]
        public string MiddleName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }
    }
}
