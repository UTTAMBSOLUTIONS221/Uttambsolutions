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
    public class BibleDataResponse
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
    public class BibleBookDataResponse
    {
        public List<BibleBook> Data { get; set; }
    }

    public class BibleBook
    {
        public string Id { get; set; }
        public string BibleId { get; set; }
        public string Abbreviation { get; set; }
        public string Name { get; set; }
        public string NameLong { get; set; }
    }

    public class BibleChapterDataResponse
    {
        public List<BibleChapter> Data { get; set; }
    }

    public class BibleChapter
    {
        public string Id { get; set; }
        public string BibleId { get; set; }
        public string Number { get; set; }
        public string BookId { get; set; }
        public string Reference { get; set; }
    }
    public class BibleVersesResponse
    {
        public List<BibleVerses> Data { get; set; }
    }

    public class BibleVerses
    {
        public string Id { get; set; }
        public string OrgId { get; set; }
        public string BibleId { get; set; }
        public string BookId { get; set; }
        public string ChapterId { get; set; }
        public string Reference { get; set; }
    }
    public class BibleVerseResponse
    {
        public BibleVerse Data { get; set; }
        public BibleMeta Meta { get; set; }
    }

    public class BibleVerse
    {
        public string Id { get; set; }
        public string OrgId { get; set; }
        public string BibleId { get; set; }
        public string BookId { get; set; }
        public string ChapterId { get; set; }
        public string Content { get; set; }
        public string Reference { get; set; }
        public int VerseCount { get; set; }
        public string Copyright { get; set; }
        public BibleVerseReference Next { get; set; }
        public BibleVerseReference Previous { get; set; }
    }

    public class BibleMeta
    {
        public string Fums { get; set; }
        public string FumsId { get; set; }
        public string FumsJsInclude { get; set; }
        public string FumsJs { get; set; }
        public string FumsNoScript { get; set; }
    }

    public class BibleVerseReference
    {
        public string Id { get; set; }
        public string BookId { get; set; }
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
}
