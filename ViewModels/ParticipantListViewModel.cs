using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventName), "EventName")]
public partial class ParticipantListViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    public ObservableCollection<ParticipantSummary> Participants { get; } = new();

    public ParticipantListViewModel()
    {
        LoadMockData();
    }

    private void LoadMockData()
    {
        Participants.Add(new ParticipantSummary { FullName = "Lucas Alencar", Email = "lucas@email.com", TicketType = "VIP", PurchaseDate = "28 Mar, 2026" });
        Participants.Add(new ParticipantSummary { FullName = "Ana Beatriz", Email = "ana@email.com", TicketType = "PISTA", PurchaseDate = "28 Mar, 2026" });
        Participants.Add(new ParticipantSummary { FullName = "Pedro Santos", Email = "pedro@email.com", TicketType = "VIP", PurchaseDate = "28 Mar, 2026" });
        Participants.Add(new ParticipantSummary { FullName = "Maria Oliveira", Email = "maria@email.com", TicketType = "PISTA", PurchaseDate = "28 Mar, 2026" });
        Participants.Add(new ParticipantSummary { FullName = "João Costa", Email = "joao@email.com", TicketType = "VIP", PurchaseDate = "28 Mar, 2026" });
        Participants.Add(new ParticipantSummary { FullName = "Carla Souza", Email = "carla@email.com", TicketType = "PISTA", PurchaseDate = "28 Mar, 2026" });
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ExportCsvAsync()
    {
        await Shell.Current.DisplayAlertAsync("Exportar", "Lista exportada com sucesso!", "OK");
    }
}

public partial class ParticipantSummary : ObservableObject
{
    [ObservableProperty]
    public partial string FullName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TicketType { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string PurchaseDate { get; set; } = string.Empty;

    public string Initial => string.IsNullOrEmpty(FullName) ? "?" : FullName[..1].ToUpper();
}
