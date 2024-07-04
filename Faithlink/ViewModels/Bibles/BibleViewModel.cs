using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Faithlink.Models;
using Faithlink.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Faithlink.ViewModels.Bibles
{
    public partial class BibleViewModel : ObservableObject
    {
        private readonly IBibleApiService _bibleApiService;
        private bool _isLoading;

        public ObservableCollection<Bible> Bibles { get; } = new();
        public ObservableCollection<BibleBook> Books { get; } = new();
        public ObservableCollection<BibleChapter> Chapters { get; } = new();
        public ObservableCollection<BibleVerses> Verses { get; } = new();
        public ObservableCollection<BibleLanguage> Languages { get; } = new();

        private Bible _selectedBible;
        public Bible SelectedBible
        {
            get => _selectedBible;
            set
            {
                if (SetProperty(ref _selectedBible, value))
                {
                    LoadBooksAsync();
                    ClearVerseDetail();
                }
            }
        }

        private BibleBook _selectedBook;
        public BibleBook SelectedBook
        {
            get => _selectedBook;
            set
            {
                if (SetProperty(ref _selectedBook, value))
                {
                    LoadChaptersAsync();
                    ClearVerseDetail();
                }
            }
        }

        private BibleChapter _selectedChapter;
        public BibleChapter SelectedChapter
        {
            get => _selectedChapter;
            set
            {
                if (SetProperty(ref _selectedChapter, value))
                {
                    LoadVersesAsync();
                    ClearVerseDetail();
                }
            }
        }

        private BibleLanguage _selectedLanguage;
        public BibleLanguage SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    LoadBiblesAsync();
                    ClearVerseDetail();
                }
            }
        }

        private BibleVerses _selectedVerse;
        public BibleVerses SelectedVerse
        {
            get => _selectedVerse;
            set
            {
                if (SetProperty(ref _selectedVerse, value))
                {
                    LoadVerseAsync();
                }
            }
        }

        private string _verseContent;
        public string VerseContent
        {
            get => _verseContent;
            set => SetProperty(ref _verseContent, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public BibleViewModel()
        {
            _bibleApiService = new BibleApiService(); // Initialize with default constructor

            // Initialize commands
            LoadLanguagesCommand = new AsyncRelayCommand(LoadLanguagesAsync);
            LoadBiblesCommand = new AsyncRelayCommand(LoadBiblesAsync);
            LoadBooksCommand = new AsyncRelayCommand(LoadBooksAsync);
            LoadChaptersCommand = new AsyncRelayCommand(LoadChaptersAsync);
            LoadVersesCommand = new AsyncRelayCommand(LoadVersesAsync);
            LoadVerseCommand = new AsyncRelayCommand(LoadVerseAsync);

            // Load initial data
            LoadLanguagesAsync();
        }

        public IAsyncRelayCommand LoadLanguagesCommand { get; }
        public IAsyncRelayCommand LoadBiblesCommand { get; }
        public IAsyncRelayCommand LoadBooksCommand { get; }
        public IAsyncRelayCommand LoadChaptersCommand { get; }
        public IAsyncRelayCommand LoadVersesCommand { get; }
        public IAsyncRelayCommand LoadVerseCommand { get; }

        private async Task LoadLanguagesAsync()
        {
            IsLoading = true;

            var languages = await _bibleApiService.GetLanguagesAsync();

            Languages.Clear();
            foreach (var language in languages)
            {
                Languages.Add(language);
            }

            if (Languages.Count > 0)
            {
                SelectedLanguage = Languages[0]; // Select the first language by default
            }

            IsLoading = false;
        }

        private async Task LoadBiblesAsync()
        {
            IsLoading = true;

            if (SelectedLanguage != null)
            {
                var bibleData = await _bibleApiService.GetBiblesAsync(SelectedLanguage.Id);

                Bibles.Clear();
                foreach (var bible in bibleData.Data)
                {
                    Bibles.Add(bible);
                }

                // Clear other collections
                Books.Clear();
                Chapters.Clear();
                Verses.Clear();
            }

            IsLoading = false;
        }

        private async Task LoadBooksAsync()
        {
            IsLoading = true;

            if (SelectedBible != null)
            {
                var books = await _bibleApiService.GetBooksAsync(SelectedBible.Id);

                Books.Clear();
                foreach (var book in books.Data)
                {
                    Books.Add(book);
                }

                // Clear other collections
                Chapters.Clear();
                Verses.Clear();
            }

            IsLoading = false;
        }

        private async Task LoadChaptersAsync()
        {
            IsLoading = true;

            if (SelectedBook != null)
            {
                var chapters = await _bibleApiService.GetChaptersAsync(SelectedBook.BibleId, SelectedBook.Id);

                Chapters.Clear();
                foreach (var chapter in chapters.Data)
                {
                    Chapters.Add(chapter);
                }

                // Clear verses collection
                Verses.Clear();
            }

            IsLoading = false;
        }

        private async Task LoadVersesAsync()
        {
            IsLoading = true;

            if (SelectedChapter != null)
            {
                var verses = await _bibleApiService.GetVersesAsync(SelectedChapter.BibleId, SelectedChapter.Id);

                Verses.Clear();
                foreach (var verse in verses.Data)
                {
                    Verses.Add(verse);
                }
            }

            IsLoading = false;
        }

        private async Task LoadVerseAsync()
        {
            IsLoading = true;

            if (SelectedVerse != null)
            {
                ClearVerseDetail();
                var verseData = await _bibleApiService.GetVerseAsync(SelectedVerse.BibleId, SelectedVerse.Id);
                VerseContent = verseData.Data.Content;
            }
            IsLoading = false;
        }
        private void ClearVerseDetail()
        {
            VerseContent = string.Empty;
        }
    }
}
