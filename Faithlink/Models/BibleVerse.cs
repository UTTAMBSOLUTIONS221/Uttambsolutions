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
    public class BibleData
    {
        public List<Bible> Data { get; set; }
    }

    public class Bible
    {
        public string Id { get; set; }
        public string DblId { get; set; }
        public string Abbreviation { get; set; }
        public string AbbreviationLocal { get; set; }
        public BibleLanguage Language { get; set; }
        public List<Country> Countries { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public string DescriptionLocal { get; set; }
        public string RelatedDbl { get; set; }
        public string Type { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<AudioBible> AudioBibles { get; set; }
    }

    public class Country
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
    }

    public class AudioBible
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameLocal { get; set; }
        public string Description { get; set; }
        public string DescriptionLocal { get; set; }
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
