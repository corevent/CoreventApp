using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class ManageEventViewModel : ObservableObject
{
    private readonly EventsService _eventsService;
    private string? _eventId;
    private EventDetailDto? _currentEvent;

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
    public partial string Status { get; set; } = "draft";

    [ObservableProperty]
    public partial string StatusDisplayText { get; set; } = "RASCUNHO";

    [ObservableProperty]
    public partial bool CanEdit { get; set; }

    [ObservableProperty]
    public partial bool CanPublish { get; set; }

    [ObservableProperty]
    public partial bool CanCancel { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public string? EventId
    {
        set
        {
            _eventId = value;
            if (value is not null) _ = LoadEventAsync(value);
        }
    }

    public ManageEventViewModel(EventsService eventsService)
    {
        _eventsService = eventsService;
    }

    private async Task LoadEventAsync(string eventId)
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var evt = await _eventsService.GetByIdAsync(eventId);
            if (evt is null) return;

            _currentEvent = evt;
            EventName = evt.Title;
            EventDate = $"{evt.StartDate:dd MMM, yyyy - HH:mm}";
            EventImage = evt.BannerUrl ?? string.Empty;
            Status = evt.Status;
            StatusDisplayText = evt.Status switch
            {
                "draft" => "RASCUNHO",
                "opened" => "ATIVO",
                "going" => "EM ANDAMENTO",
                "canceled" => "CANCELADO",
                "finished" => "ENCERRADO",
                _ => "ATIVO"
            };
            UpdatePermissions();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ManageEvent LoadEventAsync failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void UpdatePermissions()
    {
        CanEdit = Status == "draft";
        CanPublish = Status == "draft";
        CanCancel = Status == "opened" || Status == "going";
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task PublishEventAsync()
    {
        if (!CanPublish || _eventId is null) return;

        var result = await _eventsService.UpdateAsync(_eventId,
            new UpdateEventDto(
                null, null, null, null, null, null,
                null, null, null, null, null,
                null, null, null, null, null, "opened"));

        if (result is not null)
        {
            Status = "opened";
            StatusDisplayText = "ATIVO";
            UpdatePermissions();
            await Shell.Current.DisplayAlertAsync("Evento Publicado",
                "Agora seu evento está visível para o público.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Erro",
                "Não foi possível publicar o evento.", "OK");
        }
    }

    [RelayCommand]
    private async Task CancelEventAsync()
    {
        if (!CanCancel || _eventId is null) return;

        bool confirm = await Shell.Current.DisplayAlertAsync("Cancelar Evento",
            "Tem certeza que deseja cancelar este evento? Esta ação não pode ser desfeita.",
            "Sim, Cancelar", "Voltar");

        if (!confirm) return;

        var success = await _eventsService.CancelAsync(_eventId);
        if (success)
        {
            Status = "canceled";
            StatusDisplayText = "CANCELADO";
            UpdatePermissions();
            await Shell.Current.DisplayAlertAsync("Evento Cancelado",
                "O evento foi cancelado com sucesso.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Erro",
                "Não foi possível cancelar o evento.", "OK");
        }
    }

    [RelayCommand]
    private async Task EditEventAsync()
    {
        if (!CanEdit || _eventId is null) return;

        await Shell.Current.GoToAsync(nameof(Views.CreateEvent), new Dictionary<string, object>
        {
            ["EventId"] = _eventId
        });
    }

    [RelayCommand]
    private async Task CheckInAsync()
    {
        if (_eventId is null) return;

        await Shell.Current.GoToAsync(nameof(Views.CheckInPage), new Dictionary<string, object>
        {
            ["EventId"] = _eventId
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

    [RelayCommand]
    private async Task AttractionsAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.EventAttractions), new Dictionary<string, object>
        {
            ["EventName"] = EventName
        });
    }

    [RelayCommand]
    private async Task ManageTicketsAsync()
    {
        await Task.CompletedTask;
    }
}
