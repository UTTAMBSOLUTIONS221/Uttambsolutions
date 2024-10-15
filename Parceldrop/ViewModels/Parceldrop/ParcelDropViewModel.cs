using DBL;
using DBL.Entities;
using Newtonsoft.Json;
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
        public ICommand JoinThisCollectionCenterCommand { get; }



        private Parcelcollectioncenters _parcelcollectioncenterdata;
        public Parcelcollectioncenters Parcelcollectioncenterdata
        {
            get => _parcelcollectioncenterdata;
            set
            {
                _parcelcollectioncenterdata = value;
                OnPropertyChanged();
            }
        }
        private Collectioncentercouriers _collectioncentercourierdata;
        public Collectioncentercouriers Collectioncentercourierdata
        {
            get => _collectioncentercourierdata;
            set
            {
                _collectioncentercourierdata = value;
                OnPropertyChanged();
            }
        }


        public ParcelDropViewModel(BL bl)
        {
            _bl = bl;
            Collectioncentersdata = new ObservableCollection<Parcelcollectioncenters>();
            Parcelcollectioncenterdata = new Parcelcollectioncenters();
            LoadCurrentParcelDropCommand = new Command(async () => await OnLoadCurrentParcelDrop());
            JoinThisCollectionCenterCommand = new Command<Parcelcollectioncenters>(async (collectionCenter) => { var collectioncenterId = collectionCenter?.Collectioncenterid ?? 0; await OnJoinThisCollectionCenter(collectioncenterId); });

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
        private async Task OnJoinThisCollectionCenter(int Collectioncenterid)
        {
            IsProcessing = true;
            Collectioncentercourierdata = new Collectioncentercouriers
            {
                Collectioncenterid = Collectioncenterid,
                Courierid = App.UserDetails.Usermodel.Userid
            };

            var response = await _bl.Registercollectioncentercourierdata(JsonConvert.SerializeObject(Collectioncentercourierdata));
            if (response != null)
            {
                await OnLoadCurrentParcelDrop();
            }
            IsProcessing = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
