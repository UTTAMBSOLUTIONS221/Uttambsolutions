using Faithlink.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Faithlink.Services
{
    public class BibleApiService : IBibleApiService
    {
        private readonly HttpClient _httpClient;

        public BibleApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.scripture.api.bible/v1/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("api-key", "387c5d1adb54e0afd3fd11e605d16f97");
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
        public async Task<BibleData> GetBooksAsync(string languageCode)
        {
            var response = await _httpClient.GetAsync($"bibles?language={languageCode}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                return JsonSerializer.Deserialize<BibleData>(content, options);
            }
            else
            {
                // Handle unsuccessful response
                // For example, throw exception or return empty list
                return new List<BibleBook>();
            }
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
