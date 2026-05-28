using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly EventsService _eventsService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<EventListItemDto> HighlightedEvents { get; } = new();

    public HomeViewModel(EventsService eventsService)
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
            var result = await _eventsService.GetAllAsync(page: 1, limit: 10, status: "opened");
            HighlightedEvents.Clear();
            foreach (var item in result.Data)
                HighlightedEvents.Add(item);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Home LoadAsync failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task SelectEventAsync(EventListItemDto? eventItem)
    {
        if (eventItem is null) return;

        await Shell.Current.GoToAsync(nameof(Views.EventDetail), new Dictionary<string, object>
        {
            ["EventId"] = eventItem.Id
        });
    }
}
