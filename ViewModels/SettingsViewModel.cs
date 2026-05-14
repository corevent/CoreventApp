using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;

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
    private async Task NavigateToPrivacy()
    {
        await Shell.Current.GoToAsync(nameof(Privacy));
    }

    [RelayCommand]
    private async Task NavigateToEmail()
    {
        // Placeholder for Email change navigation
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task NavigateToPassword()
    {
        // Placeholder for Password change navigation
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task NavigateToLanguage()
    {
        await Task.CompletedTask;
    }
}
