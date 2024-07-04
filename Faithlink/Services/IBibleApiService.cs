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
        Task<List<BibleLanguage>> GetLanguagesAsync();
        Task<BibleData> GetBiblesAsync(string languageCode);
        Task<BibleChapter> GetChapterAsync(string bookId, int chapterNumber);
    }
}
