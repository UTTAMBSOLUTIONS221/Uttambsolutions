using Faithlink.Models;
using System.Net.Http.Json;

namespace Faithlink.Services
{ //private const string ApiKey = "387c5d1adb54e0afd3fd11e605d16f97"; // Replace with your API key
  //private const string BaseUrl = "https://api.scripture.api.bible/v1/";
    public class BibleApiService : IBibleApiService
    {
        private readonly HttpClient _httpClient;

        public BibleApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://mainapi.uttambsolutions.com/");
            // Optionally configure timeout, headers, etc. for HttpClient
        }

        public async Task<List<BibleBook>> GetBooksAsync(string languageCode)
        {
            return await _httpClient.GetFromJsonAsync<List<BibleBook>>($"https://api.bible.com/v1/books?language={languageCode}");
        }

        public async Task<List<BibleLanguage>> GetLanguagesAsync()
        {
            var languages = new List<BibleLanguage>
            {
                new BibleLanguage { Id = "kik", Name = "Gikuyu", NameLocal = "Gikuyu", Script = "Latin", ScriptDirection = "LTR" },
                new BibleLanguage { Id = "eng", Name = "English", NameLocal = "English", Script = "Latin", ScriptDirection = "LTR" },
                // Add more languages as needed
            };

            return languages;
        }

        public async Task<BibleChapter> GetChapterAsync(string bookId, int chapterNumber)
        {
            var verses = await _httpClient.GetFromJsonAsync<List<BibleVerse>>($"https://api.bible.com/v1/books/{bookId}/chapters/{chapterNumber}");
            return new BibleChapter
            {
                Number = chapterNumber,
                Verses = verses
            };
        }
    }
}
