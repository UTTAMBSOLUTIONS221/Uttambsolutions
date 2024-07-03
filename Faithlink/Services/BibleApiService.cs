using Faithlink.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faithlink.Services
{
    public class BibleApiService : IBibleApiService
    {
        private const string ApiKey = "387c5d1adb54e0afd3fd11e605d16f97"; // Replace with your API key
        private const string BaseUrl = "https://api.scripture.api.bible/v1/";
        private readonly HttpClient _httpClient;

        public BibleApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Add("api-key", ApiKey);
        }

        public async Task<List<string>> GetAllLanguagesAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("metadata/languages");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var languages = JsonConvert.DeserializeObject<List<string>>(json);
                return languages;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error fetching languages: {ex.Message}");
                return new List<string>();
            }
        }

        public async Task<string> GetVerseOfTheDayAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("votd");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var votd = JsonConvert.DeserializeObject<BibleVerse>(json);
                return votd.Text;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error fetching verse of the day: {ex.Message}");
                return $"Error: {ex.Message}";
            }
        }

        public async Task<List<BibleVerse>> GetVersesByReferenceAsync(string bibleId, string reference)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"bibles/{bibleId}/passages/{reference}");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                var verses = JsonConvert.DeserializeObject<List<BibleVerse>>(json);
                return verses;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"Error fetching verses {reference}: {ex.Message}");
                return new List<BibleVerse>();
            }
        }
    }
}
