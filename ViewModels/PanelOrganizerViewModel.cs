using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class PanelOrganizerViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string TotalRevenue { get; set; } = "R$ 199.500";

    [ObservableProperty]
    public partial string TicketsSold { get; set; } = "1.285";

    public ObservableCollection<EventSummary> MyEvents { get; } = new();

    public PanelOrganizerViewModel()
    {
        LoadMockData();
    }

    private void LoadMockData()
    {
        MyEvents.Add(new EventSummary
        {
            Name = "Festival de Verão 2026",
            Date = "15 Out, 2026",
            ImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=400&h=400&fit=crop",
            SalesProgress = 0.62,
            TotalRevenue = "R$ 186.000",
            SoldCount = "1240 VENDIDOS"
        });
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ConfigureTransferAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.TransferSettings));
    }

    [RelayCommand]
    private async Task NewEventAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CreateEvent));
    }
}

public partial class EventSummary : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Date { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ImageUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial double SalesProgress { get; set; }

    [ObservableProperty]
    public partial string TotalRevenue { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SoldCount { get; set; } = string.Empty;
}
