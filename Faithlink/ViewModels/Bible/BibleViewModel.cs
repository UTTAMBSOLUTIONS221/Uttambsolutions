using Faithlink.Models;
using Faithlink.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Faithlink.ViewModels.Bible
{
    public class BibleViewModel : INotifyPropertyChanged
    {
        private readonly IBibleApiService _bibleApiService;
        private List<string> _languages;
        private string _verseOfTheDay;
        private ObservableCollection<BibleVerse> _verses;

        public List<string> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                OnPropertyChanged();
            }
        }

        public string VerseOfTheDay
        {
            get => _verseOfTheDay;
            set
            {
                _verseOfTheDay = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BibleVerse> Verses
        {
            get => _verses;
            set
            {
                _verses = value;
                OnPropertyChanged();
            }
        }

        public BibleViewModel(IBibleApiService bibleApiService)
        {
            _bibleApiService = bibleApiService;
            LoadData();
        }

        private async void LoadData()
        {
            Languages = await _bibleApiService.GetAllLanguagesAsync();
            VerseOfTheDay = await _bibleApiService.GetVerseOfTheDayAsync();
            Verses = new ObservableCollection<BibleVerse>(); // Initialize or load verses as needed
        }

        public async Task LoadVersesByReference(string bibleId, string reference)
        {
            var verses = await _bibleApiService.GetVersesByReferenceAsync(bibleId, reference);
            Verses = new ObservableCollection<BibleVerse>(verses);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}