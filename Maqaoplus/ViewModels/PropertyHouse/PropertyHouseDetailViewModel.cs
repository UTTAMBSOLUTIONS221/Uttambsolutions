﻿using DBL.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseDetailViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private long _propertyId;

        public ObservableCollection<PropertyHouseDetails> Rooms { get; }
        public ICommand LoadRoomsCommand { get; }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public PropertyHouseDetailViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Rooms = new ObservableCollection<PropertyHouseDetails>();
            LoadRoomsCommand = new Command(async () => await LoadRooms());
        }

        public void SetPropertyId(long propertyId)
        {
            _propertyId = propertyId;
            LoadRoomsCommand.Execute(null);
        }

        private async Task LoadRooms()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data != null)
                {
                    Rooms.Clear();
                    foreach (var room in response.Data)
                    {
                        Rooms.Add(room);
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}