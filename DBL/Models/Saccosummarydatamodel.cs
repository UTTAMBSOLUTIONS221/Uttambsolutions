using DBL.Entities;

namespace DBL.Models
{
    public class Saccosummarydatamodel
    {
        public int Saccoid { get; set; }
        public int Memberid { get; set; }
        public List<Esaccosaccos>? Esaccosaccosdata { get; set; }
    }
}
