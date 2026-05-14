using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    [ObservableProperty]
    bool isProximosVisible = true;

    [ObservableProperty]
    bool isPassadosVisible = false;

    [RelayCommand]
    void SelectProximos()
    {
        IsProximosVisible = true;
        IsPassadosVisible = false;
    }

    [RelayCommand]
    void SelectPassados()
    {
        IsProximosVisible = false;
        IsPassadosVisible = true;
    }
}