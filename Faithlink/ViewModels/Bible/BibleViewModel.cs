using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Faithlink.Models;
using Faithlink.Services;
using System.Collections.ObjectModel;
using System.Net.Http;

namespace Faithlink.ViewModels.Bible
{
    public partial class BibleViewModel : ObservableObject
    {
        private readonly IBibleApiService _bibleApiService;

        public ObservableCollection<BibleBook> Books { get; } = new();
        public ObservableCollection<BibleLanguage> Languages { get; } = new();

        [ObservableProperty]
        private BibleBook _selectedBook;

        [ObservableProperty]
        private BibleChapter _selectedChapter;

        [ObservableProperty]
        private BibleLanguage _selectedLanguage;

        public BibleViewModel(IBibleApiService bibleApiService)
        {
            _bibleApiService = bibleApiService;
            LoadLanguagesCommand = new AsyncRelayCommand(LoadLanguagesAsync);
            LoadBooksCommand = new AsyncRelayCommand(LoadBooksAsync);
            LoadChapterCommand = new AsyncRelayCommand(LoadChapterAsync);
        }

        public IAsyncRelayCommand LoadLanguagesCommand { get; }
        public IAsyncRelayCommand LoadBooksCommand { get; }
        public IAsyncRelayCommand LoadChapterCommand { get; }

        private async Task LoadLanguagesAsync()
        {
            var languages = await _bibleApiService.GetLanguagesAsync();
            Languages.Clear();
            foreach (var language in languages)
            {
                Languages.Add(language);
            }
        }

        private async Task LoadBooksAsync()
        {
            if (SelectedLanguage != null)
            {
                var books = await _bibleApiService.GetBooksAsync(SelectedLanguage.Id);
                Books.Clear();
                foreach (var book in books)
                {
                    Books.Add(book);
                }
            }
        }

        private async Task LoadChapterAsync()
        {
            if (SelectedBook != null)
            {
                var chapter = await _bibleApiService.GetChapterAsync(SelectedBook.Id, 1); // Assuming chapter 1 for now
                SelectedChapter = chapter;
            }
        }
    }
}