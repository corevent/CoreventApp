using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

public partial class TicketsViewModel : ObservableObject
{
    [ObservableProperty]
    public partial bool IsProximosVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsPassadosVisible { get; set; } = false;

    [RelayCommand]
    public void SelectProximos()
    {
        IsProximosVisible = true;
        IsPassadosVisible = false;
    }

    [RelayCommand]
    public void SelectPassados()
    {
        IsProximosVisible = false;
        IsPassadosVisible = true;
    }
}