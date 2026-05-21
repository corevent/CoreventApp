using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models;

namespace CoreventApp.ViewModels;

public partial class PanelCollaboratorViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isAgendaVisible = true;

    [ObservableProperty]
    private bool _isHistoricoVisible = false;

    public ObservableCollection<CollaboratorEvent> EventsToday { get; } = new();
    public ObservableCollection<CollaboratorEvent> UpcomingEvents { get; } = new();
    public ObservableCollection<CollaboratorEvent> PastEvents { get; } = new();

    [ObservableProperty]
    private bool _hasEventsToday;

    [ObservableProperty]
    private bool _hasUpcomingEvents;

    [ObservableProperty]
    private bool _hasPastEvents;

    public PanelCollaboratorViewModel()
    {
        LoadSampleData();
    }

    private void LoadSampleData()
    {
        EventsToday.Add(new CollaboratorEvent
        {
            ImageUrl = "https://images.unsplash.com/photo-1516450360452-9312f5e86fc7?w=400&amp;auto=format&amp;fit=crop",
            Title = "Techno Night Under",
            Date = "21 Mai, 2026",
            Role = "CREDENCIAMENTO",
            RoleColor = "#E0F2FE",
            RoleTextColor = "#0284C7",
            HasActionButton = true,
            ParticipantCount = 128
        });

        UpcomingEvents.Add(new CollaboratorEvent
        {
            ImageUrl = "https://images.unsplash.com/photo-1514525253161-7a46d19cd819?w=400&amp;auto=format&amp;fit=crop",
            Title = "Jazz & Wine Sunset",
            Date = "25 Out, 2026",
            Role = "ORGANIZAÇÃO",
            RoleColor = "#F3E8FF",
            RoleTextColor = "#9333EA",
            HasActionButton = false,
            ParticipantCount = 67
        });

        UpcomingEvents.Add(new CollaboratorEvent
        {
            ImageUrl = "https://images.unsplash.com/photo-1470229722913-7c0e2dbbafd3?w=400&amp;auto=format&amp;fit=crop",
            Title = "Festival de Verão",
            Date = "12 Dez, 2026",
            Role = "PRODUÇÃO",
            RoleColor = "#FEF3C7",
            RoleTextColor = "#D97706",
            HasActionButton = false,
            ParticipantCount = 312
        });

        PastEvents.Add(new CollaboratorEvent
        {
            ImageUrl = "https://images.unsplash.com/photo-1459749411175-04bf5292ceea?w=400&amp;auto=format&amp;fit=crop",
            Title = "Samba Fest",
            Date = "02 Set, 2026",
            Role = "PRODUÇÃO",
            RoleColor = "#F3F4F6",
            RoleTextColor = "#6B7280",
            HasActionButton = false,
            ParticipantCount = 89
        });

        PastEvents.Add(new CollaboratorEvent
        {
            ImageUrl = "https://images.unsplash.com/photo-1429962714451-bb934ecdc4ec?w=400&amp;auto=format&amp;fit=crop",
            Title = "Rock in Rio",
            Date = "15 Ago, 2026",
            Role = "CREDENCIAMENTO",
            RoleColor = "#F3F4F6",
            RoleTextColor = "#6B7280",
            HasActionButton = false,
            ParticipantCount = 450
        });

        HasEventsToday = EventsToday.Count > 0;
        HasUpcomingEvents = UpcomingEvents.Count > 0;
        HasPastEvents = PastEvents.Count > 0;
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
