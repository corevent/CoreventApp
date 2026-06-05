using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class PanelCollaboratorViewModel : ObservableObject
{
    private readonly EventsService _eventsService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsAgendaVisible { get; set; } = true;

    [ObservableProperty]
    public partial bool IsHistoricoVisible { get; set; } = false;

    public ObservableCollection<CollaboratorEvent> EventsToday { get; } = new();
    public ObservableCollection<CollaboratorEvent> UpcomingEvents { get; } = new();
    public ObservableCollection<CollaboratorEvent> PastEvents { get; } = new();

    [ObservableProperty]
    public partial bool HasEventsToday { get; set; }

    [ObservableProperty]
    public partial bool HasUpcomingEvents { get; set; }

    [ObservableProperty]
    public partial bool HasPastEvents { get; set; }

    public PanelCollaboratorViewModel(EventsService eventsService)
    {
        _eventsService = eventsService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var result = await _eventsService.GetMyStaffEventsAllAsync(page: 1, limit: 100);

            EventsToday.Clear();
            UpcomingEvents.Clear();
            PastEvents.Clear();

            foreach (var item in result.Data)
            {
                var ce = MapToCollaboratorEvent(item);

                if (item.StartDate.Date == DateTime.Today)
                    EventsToday.Add(ce);
                else if (item.StartDate.Date > DateTime.Today)
                    UpcomingEvents.Add(ce);
                else
                    PastEvents.Add(ce);
            }

            HasEventsToday = EventsToday.Count > 0;
            HasUpcomingEvents = UpcomingEvents.Count > 0;
            HasPastEvents = PastEvents.Count > 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"PanelCollaborator LoadAsync failed: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static CollaboratorEvent MapToCollaboratorEvent(StaffEventListItemDto item)
    {
        var isCheckin = item.AccessLevel == "checkin";
        var role = isCheckin ? "CREDENCIAMENTO" : "ORGANIZAÇÃO";
        var roleColor = isCheckin ? "#E0F2FE" : "#F3E8FF";
        var roleTextColor = isCheckin ? "#0284C7" : "#9333EA";

        return new CollaboratorEvent
        {
            ImageUrl = "https://images.unsplash.com/photo-1516450360452-9312f5e86fc7?w=400&auto=format&fit=crop",
            Title = item.Title,
            Date = item.StartDate.ToString("dd MMM, yyyy"),
            Role = role,
            RoleColor = roleColor,
            RoleTextColor = roleTextColor,
            HasActionButton = item.StartDate.Date == DateTime.Today && isCheckin,
            ParticipantCount = 0
        };
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void SelectAgenda()
    {
        IsAgendaVisible = true;
        IsHistoricoVisible = false;
    }

    [RelayCommand]
    private void SelectHistorico()
    {
        IsAgendaVisible = false;
        IsHistoricoVisible = true;
    }

    [RelayCommand]
    private async Task RealizarCredenciamentoAsync()
    {
        await Shell.Current.DisplayAlertAsync("Credenciamento", "Abrir câmera para leitura de QR Code", "OK");
    }

    [RelayCommand]
    private async Task OpenEventDetailAsync(CollaboratorEvent evt)
    {
        await Shell.Current.GoToAsync(
            $"CollaboratorEventDetail?EventTitle={Uri.EscapeDataString(evt.Title)}&EventDate={Uri.EscapeDataString(evt.Date)}&EventImage={Uri.EscapeDataString(evt.ImageUrl)}&EventRole={Uri.EscapeDataString(evt.Role)}&EventRoleColor={Uri.EscapeDataString(evt.RoleColor)}&EventRoleTextColor={Uri.EscapeDataString(evt.RoleTextColor)}&ParticipantCount={evt.ParticipantCount}");
    }
}
