using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class PanelOrganizerViewModel : ObservableObject
{
    private readonly EventsService _eventsService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    public partial string TotalRevenue { get; set; } = "R$ 0";

    [ObservableProperty]
    public partial string TicketsSold { get; set; } = "0";

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<EventListItemDto> MyEvents { get; } = new();

    public PanelOrganizerViewModel(EventsService eventsService, IAuthService authService)
    {
        _eventsService = eventsService;
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var currentUserId = _authService.CurrentCachedUser?.Id;
            // Load a reasonable batch (max allowed by API)
            var result = await _eventsService.GetAllAsync(page: 1, limit: 100);

            MyEvents.Clear();

            int totalSold = 0;
            decimal totalRev = 0;

            foreach (var item in result.Data)
            {
                // Filter by current user's organizer ID
                if (item.Organizer.Id == currentUserId)
                {
                    MyEvents.Add(item);
                }
            }

            TicketsSold = totalSold > 0 ? $"{totalSold}" : "0";
            TotalRevenue = totalRev > 0
                ? $"R$ {totalRev:N0}"
                : "R$ 0";
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"PanelOrganizer LoadAsync failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
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
    private async Task SelectEventAsync(EventListItemDto? eventItem)
    {
        if (eventItem is null) return;

        await Shell.Current.GoToAsync(nameof(Views.ManageEvent), new Dictionary<string, object>
        {
            ["EventId"] = eventItem.Id
        });
    }
}
