﻿using DBL;
using DBL.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Parceldrop.ViewModels.Parceldrop
{
    public class ParcelDropViewModel : INotifyPropertyChanged
    {
        private readonly BL _bl;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
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
        public ObservableCollection<Parcelcollectioncenters> Collectioncentersdata { get; }

        public ICommand LoadCurrentParcelDropCommand { get; }


        public ParcelDropViewModel(BL bl)
        {
            _bl = bl;
            Collectioncentersdata = new ObservableCollection<Parcelcollectioncenters>();
            LoadCurrentParcelDropCommand = new Command(async () => await OnLoadCurrentParcelDrop());

        }



        private async Task OnLoadCurrentParcelDrop()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _bl.Getparcelcollectioncentersnotindata(App.UserDetails.Usermodel.Userid);
                if (response != null && response != null)
                {
                    Collectioncentersdata.Clear();
                    foreach (var item in response)
                    {
                        Collectioncentersdata.Add(item);
                    }
                }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
