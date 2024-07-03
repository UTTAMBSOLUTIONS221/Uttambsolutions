using Faithlink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Faithlink.Services
{
    public interface IBibleApiService
    {
        Task<List<string>> GetAllLanguagesAsync();
        Task<string> GetVerseOfTheDayAsync();
        Task<List<BibleVerse>> GetVersesByReferenceAsync(string bibleId, string reference);
    }
}
