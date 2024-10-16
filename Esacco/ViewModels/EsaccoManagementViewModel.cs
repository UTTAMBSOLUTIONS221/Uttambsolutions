using DBL;
using DBL.Models;
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

        public EsaccoManagementViewModel(BL bl)
        {
            _bl = bl;
            Saccosummarymodeldata = new Saccosummarydatamodel();
            LoadSaccoSummaryDataCommand = new Command(async () => await OnLoadSaccoSummaryData());
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

