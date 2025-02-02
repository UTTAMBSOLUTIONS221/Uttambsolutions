﻿using DBL;
using DBL.Entities;
using DBL.Models;
using Maqaoplus.Views.Startup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly BL _bl;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ICommand LoadMaqaoplussummaryCommand { get; }
        public ICommand JoinMaqaoPlusCommand { get; }
        public ICommand SubscribeCommand { get; }
        public ICommand FacebookCommand { get; }
        public ICommand InstagramCommand { get; }
        public ICommand TwitterCommand { get; }
        public ICommand CloseLiveStreamCommand { get; }
        public ICommand ViewMoreDetailsCommand { get; }

        private bool _isLiveStreamVisible;
        public bool IsLiveStreamVisible
        {
            get => _isLiveStreamVisible;
            set
            {
                _isLiveStreamVisible = value;
                OnPropertyChanged();
            }
        }
        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged();
            }
        }

        private bool _isMoreDetailVisible;
        public bool IsMoreDetailVisible
        {
            get => _isMoreDetailVisible;
            set
            {
                _isMoreDetailVisible = value;
                OnPropertyChanged();
            }
        }
        private Maqaoplussummary _Maqaoplussummarydata;
        public Maqaoplussummary Maqaoplussummarydata
        {
            get => _Maqaoplussummarydata;
            set
            {
                if (_Maqaoplussummarydata != value)
                {
                    _Maqaoplussummarydata = value;
                    OnPropertyChanged(nameof(Maqaoplussummarydata));
                }
            }
        }
        private Vacanthousesdata _Vacanthousesdata;
        public Vacanthousesdata Vacanthousesdata
        {
            get => _Vacanthousesdata;
            set
            {
                if (_Vacanthousesdata != value)
                {
                    _Vacanthousesdata = value;
                    OnPropertyChanged(nameof(Vacanthousesdata));
                }
            }
        }
        public MainPageViewModel(BL bl)
        {
            _bl = bl;
            // Initialize commands
            Maqaoplussummarydata = new Maqaoplussummary();
            LoadMaqaoplussummaryCommand = new Command(async () => await OnLoadChurchSummary());
            JoinMaqaoPlusCommand = new Command(OnJoinMaqaoPlus);
            SubscribeCommand = new Command(OnSubscribe);
            FacebookCommand = new Command(OnFacebook);
            InstagramCommand = new Command(OnInstagram);
            TwitterCommand = new Command(OnTwitter);
            CloseLiveStreamCommand = new Command(OnCloseLiveStream);
            ViewMoreDetailsCommand = new Command<Vacanthousesdata>(async (param) => { var houseroomid = param?.SystempropertyhouseroomId ?? 0; await OnViewMoreDetails(houseroomid); });

            IsLiveStreamVisible = false;
            IsMoreDetailVisible = false;
        }

        private async Task OnLoadChurchSummary()
        {
            IsProcessing = true;
            IsDataLoaded = false;
            try
            {
                Maqaoplussummarydata = await _bl.Getmaqaoplussummarydata();
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private async void OnJoinMaqaoPlus()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async void OnSubscribe()
        {
            var channelId = "UCq8GdLxjnvvvpA-AuTemPYA";
            var subscribeUrl = $"https://www.youtube.com/channel/{channelId}?sub_confirmation=1";
            await Launcher.OpenAsync(subscribeUrl);
        }
        private async void OnFacebook()
        {
            var facebookUrl = "https://www.facebook.com/YOUR_FACEBOOK_PAGE";
            await Launcher.OpenAsync(facebookUrl);
        }

        private async void OnInstagram()
        {
            var instagramUrl = "https://www.instagram.com/YOUR_INSTAGRAM_PROFILE";
            await Launcher.OpenAsync(instagramUrl);
        }

        private async void OnTwitter()
        {
            var twitterUrl = "https://twitter.com/YOUR_TWITTER_PROFILE";
            await Launcher.OpenAsync(twitterUrl);
        }


        private void OnCloseLiveStream()
        {
            IsLiveStreamVisible = false;
        }
        private async Task OnViewMoreDetails(int houseroomid)
        {
            IsProcessing = true;
            IsDataLoaded = false;
            IsMoreDetailVisible = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
