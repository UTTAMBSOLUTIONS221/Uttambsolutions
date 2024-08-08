using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBL.Models
{
    public class BaseEntity
    {
        [NotMapped]
        [JsonIgnore]
        public static string IDColumn { get { return "Id"; } }

        [NotMapped]
        [JsonProperty("stat")]
        public int RespStatus { get; set; }

        [NotMapped]
        [JsonProperty("msg")]
        public string RespMessage { get; set; }
    }
}
