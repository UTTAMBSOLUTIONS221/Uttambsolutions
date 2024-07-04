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
        Task<BibleDataResponse> GetBiblesAsync(string languageCode);
        Task<BibleBookDataResponse> GetBooksAsync(string bibleId);
        Task<BibleChapterDataResponse> GetChaptersAsync(string bibleId, string bookId);
        Task<BibleVersesResponse> GetVersesAsync(string bibleId, string chapterId);
        Task<BibleVerseResponse> GetVerseAsync(string bibleId, string verseId);
    }
}
