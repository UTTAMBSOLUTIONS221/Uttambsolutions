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

        private Saccosummarydatamodel _Saccosummarydatamodeldata;
        public Saccosummarydatamodel Saccosummarydatamodeldata
        {
            get => _Saccosummarydatamodeldata;
            set
            {
                if (_Saccosummarydatamodeldata != value)
                {
                    _Saccosummarydatamodeldata = value;
                    OnPropertyChanged(nameof(Saccosummarydatamodel));
                }
            }
        }



        public EsaccoManagementViewModel(BL bl)
        {
            _bl = bl;
            Saccosummarydatamodeldata = new Saccosummarydatamodel();
            LoadSaccoSummaryDataCommand = new Command(async () => await OnLoadSaccoSummaryData());
        }
        private async Task OnLoadSaccoSummaryData()
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

