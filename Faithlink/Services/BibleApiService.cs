using Faithlink.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<BibleDataResponse> GetBiblesAsync(string languageCode)
        {
            var response = await _httpClient.GetAsync($"bibles?language={languageCode}");
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<BibleDataResponse>(content, options);
        }

        public async Task<BibleBookDataResponse> GetBooksAsync(string bibleId)
        {
            var response = await _httpClient.GetAsync($"bibles/{bibleId}/books");
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<BibleBookDataResponse>(content, options);
        }

        public async Task<BibleChapterDataResponse> GetChaptersAsync(string bibleId, string bookId)
        {
            var response = await _httpClient.GetAsync($"bibles/{bibleId}/books/{bookId}/chapters");
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<BibleChapterDataResponse>(content, options);
        }

        public async Task<BibleVersesResponse> GetVersesAsync(string bibleId, string chapterId)
        {
            var response = await _httpClient.GetAsync($"bibles/{bibleId}/chapters/{chapterId}/verses");
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<BibleVersesResponse>(content, options);
        }
        public async Task<BibleVerseResponse> GetVerseAsync(string bibleId, string verseId)
        {
            var response = await _httpClient.GetAsync($"bibles/{bibleId}/verses/{verseId}?content-type=text");
            var content = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<BibleVerseResponse>(content, options);
        }
    }
}
