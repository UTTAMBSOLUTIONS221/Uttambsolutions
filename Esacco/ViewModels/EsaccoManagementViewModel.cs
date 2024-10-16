using DBL;
using DBL.Entities;
using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Esacco.ViewModels
{
    public class EsaccoManagementViewModel : INotifyPropertyChanged
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
        public ICommand LoadSaccoSummaryDataCommand { get; }
        public ICommand JoinEsaccoSaccoCommand { get; }

        private Saccosummarydatamodel _Saccosummarymodeldata;
        public Saccosummarydatamodel Saccosummarymodeldata
        {
            get => _Saccosummarymodeldata;
            set
            {
                if (_Saccosummarymodeldata != value)
                {
                    _Saccosummarymodeldata = value;
                    OnPropertyChanged(nameof(Saccosummarymodeldata));
                }
            }
        }
        private Esaccosaccos _Esaccosaccosdata;
        public Esaccosaccos Esaccosaccosdata
        {
            get => _Esaccosaccosdata;
            set
            {
                if (_Esaccosaccosdata != value)
                {
                    _Esaccosaccosdata = value;
                    OnPropertyChanged(nameof(Esaccosaccosdata));
                }
            }
        }
        private Saccodrivers _Saccodriversdata;
        public Saccodrivers Saccodriversdata
        {
            get => _Saccodriversdata;
            set
            {
                if (_Saccodriversdata != value)
                {
                    _Saccodriversdata = value;
                    OnPropertyChanged(nameof(Saccodriversdata));
                }
            }
        }

        public EsaccoManagementViewModel(BL bl)
        {
            _bl = bl;
            Saccosummarymodeldata = new Saccosummarydatamodel();
            Esaccosaccosdata = new Esaccosaccos();
            LoadSaccoSummaryDataCommand = new Command(async () => await OnLoadSaccoSummaryData());
            JoinEsaccoSaccoCommand = new Command<Esaccosaccos>(async (sacco) => { var saccoId = sacco?.Saccoid ?? 0; await OnJoinEsaccoSacco(saccoId); });
        }
        private async Task OnLoadSaccoSummaryData()
        {
            IsProcessing = true;
            IsDataLoaded = false;
            try
            {
                Saccosummarymodeldata = await _bl.Getsaccosummarymodeldata(App.UserDetails.Usermodel.Userid);
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

        private async Task OnJoinEsaccoSacco(int SaccoId)
        {
            IsProcessing = true;
            Saccodriversdata = new Saccodrivers
            {
                Saccoid = SaccoId,
                Userid = App.UserDetails.Usermodel.Userid
            };

            var response = await _bl.Registercollectioncentercourierdata(JsonConvert.SerializeObject(Saccodriversdata));
            if (response != null)
            {
                await OnLoadSaccoSummaryData();
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

