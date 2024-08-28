using CommunityToolkit.Mvvm.Input;
using Maqaoplus.Views.Startup;
using System.ComponentModel;
namespace Maqaoplus.ViewModels
{
    public partial class AppShellViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [RelayCommand]
        async void SignOut()
        {
            if (Preferences.ContainsKey(nameof(App.UserDetails)))
            {
                Preferences.Remove(nameof(App.UserDetails));
            }
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
