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
            SoldCount = "1240 VENDIDOS",
            Status = EventStatus.Going,
            StartDate = new DateTime(2026, 10, 15),
            EndDate = new DateTime(2026, 10, 16)
        });
        MyEvents.Add(new EventSummary
        {
            Name = "Workshop de Design Grátis",
            Date = "20 Nov, 2026",
            ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400&h=400&fit=crop",
            SalesProgress = 0.85,
            TotalRevenue = "R$ 42.500",
            SoldCount = "850 VENDIDOS",
            Status = EventStatus.Opened,
            StartDate = new DateTime(2026, 11, 20),
            EndDate = new DateTime(2026, 11, 20)
        });
        MyEvents.Add(new EventSummary
        {
            Name = "Conferência de Tecnologia",
            Date = "10 Mar, 2026",
            ImageUrl = "https://images.unsplash.com/photo-1505373877841-8d25f7d46678?w=400&h=400&fit=crop",
            SalesProgress = 1.0,
            TotalRevenue = "R$ 312.000",
            SoldCount = "2000 VENDIDOS",
            Status = EventStatus.Finished,
            StartDate = new DateTime(2026, 3, 10),
            EndDate = new DateTime(2026, 3, 12)
        });
        MyEvents.Add(new EventSummary
        {
            Name = "Hackathon de Inovação",
            Date = "5 Jan, 2027",
            ImageUrl = "https://images.unsplash.com/photo-1504384308090-c894fdcc538d?w=400&h=400&fit=crop",
            SalesProgress = 0.0,
            TotalRevenue = "R$ 0",
            SoldCount = "0 VENDIDOS",
            Status = EventStatus.Draft,
            StartDate = new DateTime(2027, 1, 5),
            EndDate = new DateTime(2027, 1, 7)
        });
        MyEvents.Add(new EventSummary
        {
            Name = "Feira de Carreiras",
            Date = "12 Fev, 2026",
            ImageUrl = "https://images.unsplash.com/photo-1540575467063-178a50c2df87?w=400&h=400&fit=crop",
            SalesProgress = 0.3,
            TotalRevenue = "R$ 0",
            SoldCount = "0 VENDIDOS",
            Status = EventStatus.Canceled,
            StartDate = new DateTime(2026, 2, 12),
            EndDate = new DateTime(2026, 2, 12)
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

    [RelayCommand]
    private async Task SelectEventAsync(EventSummary eventSummary)
    {
        await Shell.Current.GoToAsync(nameof(Views.ManageEvent), new Dictionary<string, object>
        {
            ["EventData"] = eventSummary
        });
    }
}

public enum EventStatus
{
    Draft,
    Opened,
    Going,
    Canceled,
    Finished
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

    [ObservableProperty]
    public partial EventStatus Status { get; set; } = EventStatus.Draft;

    [ObservableProperty]
    public partial DateTime StartDate { get; set; } = DateTime.Today.AddDays(30);

    [ObservableProperty]
    public partial DateTime EndDate { get; set; } = DateTime.Today.AddDays(31);

    public string StatusDisplayText => Status switch
    {
        EventStatus.Draft => "RASCUNHO",
        EventStatus.Opened => "ATIVO",
        EventStatus.Going => "EM ANDAMENTO",
        EventStatus.Canceled => "CANCELADO",
        EventStatus.Finished => "ENCERRADO",
        _ => "ATIVO"
    };
}
