namespace Faithlink.Models
{
    public class BibleLanguage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Script { get; set; }
        public string ScriptDirection { get; set; }
    }
    public class BibleBook
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<BibleChapter> Chapters { get; set; }
    }

    public class BibleChapter
    {
        public int Number { get; set; }
        public List<BibleVerse> Verses { get; set; }
    }

    public class BibleVerse
    {
        public int Number { get; set; }
        public string Text { get; set; }
    }
}
