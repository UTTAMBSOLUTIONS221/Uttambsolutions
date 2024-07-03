namespace Faithlink.Models
{
    public class OpenForum
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public DateTime OpenTime { get; set; }
        public bool IsOpen { get; set; }
    }
}
