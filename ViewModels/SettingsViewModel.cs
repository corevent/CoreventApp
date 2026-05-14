using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsPushNotificationsEnabled { get; set; } = true;

    [ObservableProperty]
    public partial bool IsDarkModeEnabled { get; set; } = false;

    [ObservableProperty]
    public partial string CurrentLanguage { get; set; } = "Português (BR)";

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task NavigateToDetail(string destination)
    {
        // Placeholder for navigation to details like Privacy, Payment, etc.
        // await Shell.Current.GoToAsync(destination);
        await Task.CompletedTask;
    }
}
