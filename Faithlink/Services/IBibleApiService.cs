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
        Task<List<BibleBook>> GetBooksAsync(string languageCode);
        Task<List<BibleLanguage>> GetLanguagesAsync();
        Task<BibleChapter> GetChapterAsync(string bookId, int chapterNumber);
    }
}
