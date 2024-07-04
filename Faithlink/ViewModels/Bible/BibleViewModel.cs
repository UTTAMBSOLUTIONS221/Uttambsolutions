using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Faithlink.Models;
using Faithlink.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Faithlink.ViewModels.Bible
{
    public class BibleViewModel : ObservableObject
    {
        private readonly IBibleApiService _bibleApiService;

        public ObservableCollection<Bible> Bibles { get; } = new();
        public ObservableCollection<BibleBook> Books { get; } = new();
        public ObservableCollection<BibleLanguage> Languages { get; } = new();

        private BibleBook _selectedBook;
        public BibleBook SelectedBook
        {
            get => _selectedBook;
            set => SetProperty(ref _selectedBook, value);
        }

        private BibleChapter _selectedChapter;
        public BibleChapter SelectedChapter
        {
            get => _selectedChapter;
            set => SetProperty(ref _selectedChapter, value);
        }

        private BibleLanguage _selectedLanguage;
        public BibleLanguage SelectedLanguage
        {
            get => _selectedLanguage;
            set => SetProperty(ref _selectedLanguage, value);
        }

        public BibleViewModel()
        {
            _bibleApiService = new BibleApiService(); // Initialize with default constructor

            // Initialize commands
            LoadLanguagesCommand = new AsyncRelayCommand(LoadLanguagesAsync);
            LoadBooksCommand = new AsyncRelayCommand(LoadBooksAsync);
            LoadChapterCommand = new AsyncRelayCommand(LoadChapterAsync);

            // Load initial data
            LoadLanguagesAsync();
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
        private async Task LoadBiblesAsync()
        {
            if (SelectedLanguage != null)
            {
                var bibleData = await _bibleApiService.GetBiblesAsync(SelectedLanguage.Id);
                Bibles.Clear();
                foreach (var bible in bibleData.Data)
                {
                    Bibles.Add(bible);
                }
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
