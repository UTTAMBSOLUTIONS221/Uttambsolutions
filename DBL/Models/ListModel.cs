namespace DBL.Models
{
    public class ListModel
    {
        public ListModel()
        {
        }

        public ListModel(string text, string value, string? groupId, string? groupName)
        {
            Text = text;
            Value = value;
            GroupId = groupId;
            GroupName = groupName;
        }

        public string Text { get; set; }
        public string Value { get; set; }
        public string? GroupId { get; set; }
        public string? GroupName { get; set; }
    }
}
