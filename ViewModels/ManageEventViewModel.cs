using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventData), "EventData")]
public partial class ManageEventViewModel : ObservableObject
{
    private EventSummary? _currentEvent;

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EventDate { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EventImage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TotalRevenue { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string TicketsSoldLabel { get; set; } = string.Empty;

    [ObservableProperty]
    public partial double SalesProgress { get; set; }

    [ObservableProperty]
    public partial EventStatus Status { get; set; } = EventStatus.Draft;

    [ObservableProperty]
    public partial string StatusDisplayText { get; set; } = "RASCUNHO";

    [ObservableProperty]
    public partial bool CanEdit { get; set; }

    [ObservableProperty]
    public partial bool CanPublish { get; set; }

    [ObservableProperty]
    public partial bool CanCancel { get; set; }

    public EventSummary? EventData
    {
        set
        {
            if (value is not null)
            {
                _currentEvent = value;
                EventName = value.Name;
                EventDate = value.Date;
                EventImage = value.ImageUrl;
                TotalRevenue = value.TotalRevenue;
                TicketsSoldLabel = value.SoldCount;
                SalesProgress = value.SalesProgress;
                Status = value.Status;
                StatusDisplayText = value.StatusDisplayText;
                UpdatePermissions();
            }
        }
    }

    private void UpdatePermissions()
    {
        CanEdit = Status == EventStatus.Draft;
        CanPublish = Status == EventStatus.Draft;
        CanCancel = Status == EventStatus.Opened || Status == EventStatus.Going;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task PublishEventAsync()
    {
        Status = EventStatus.Opened;
        StatusDisplayText = "ATIVO";
        UpdatePermissions();
        await Shell.Current.DisplayAlertAsync("Evento Publicado", "Agora seu evento está visível para o público.", "OK");
    }

    [RelayCommand]
    private async Task CancelEventAsync()
    {
        bool confirm = await Shell.Current.DisplayAlertAsync("Cancelar Evento",
            "Tem certeza que deseja cancelar este evento? Esta ação não pode ser desfeita.", "Sim, Cancelar", "Voltar");

        if (!confirm) return;

        Status = EventStatus.Canceled;
        StatusDisplayText = "CANCELADO";
        UpdatePermissions();
        await Shell.Current.DisplayAlertAsync("Evento Cancelado", "O evento foi cancelado com sucesso.", "OK");
    }

    [RelayCommand]
    private async Task EditEventAsync()
    {
        if (!CanEdit) return;

        if (_currentEvent is null) return;

        await Shell.Current.GoToAsync(nameof(Views.CreateEvent), new Dictionary<string, object>
        {
            ["EventData"] = _currentEvent
        });
    }

    [RelayCommand]
    private async Task ManageTicketsAsync()
    {
        await Task.CompletedTask;
    }

    [RelayCommand]
    private async Task CheckInAsync()
    {
        if (_currentEvent is null) return;

        await Shell.Current.GoToAsync(nameof(Views.CheckInPage), new Dictionary<string, object>
        {
            ["EventData"] = _currentEvent
        });
    }

    [RelayCommand]
    private async Task ParticipantListAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ParticipantList), new Dictionary<string, object>
        {
            ["EventName"] = EventName
        });
    }

    [RelayCommand]
    private async Task TeamAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.EventTeam), new Dictionary<string, object>
        {
            ["EventName"] = EventName
        });
    }
}
