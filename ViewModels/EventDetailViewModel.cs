using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class EventDetailViewModel : ObservableObject
{
    private readonly EventsService _eventsService;
    private string? _eventId;

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Category { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EventDate { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Location { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ImageUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OrganizerName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OrganizerAvatar { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Price { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFavorite { get; set; }

    [ObservableProperty]
    public partial string OnlineUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string LocationTypeDisplay { get; set; } = "Presencial";

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

    public EventDetailViewModel(EventsService eventsService)
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

            EventName = evt.Title;
            EventDate = $"{evt.StartDate:dd MMM, yyyy - HH:mm}";
            ImageUrl = evt.BannerUrl ?? string.Empty;
            Category = evt.Category;

            // Combine locationName + cityName for display
            var cityPart = !string.IsNullOrEmpty(evt.CityName) ? $", {evt.CityName}" : "";
            if (!string.IsNullOrEmpty(evt.StateAcronym))
                cityPart += $" - {evt.StateAcronym}";
            Location = $"{evt.LocationName}{cityPart}";

            OrganizerName = evt.Organizer.Name ?? string.Empty;
            OrganizerAvatar = evt.Organizer.AvatarUrl ?? "profile_default_icon.png";
            OnlineUrl = evt.LocationType == "online" ? evt.LocationName : string.Empty;
            LocationTypeDisplay = evt.LocationType switch
            {
                "online" => "Online",
                "in_person" => "Presencial",
                "hybrid" => "Híbrido",
                _ => "Presencial"
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"EventDetail LoadEventAsync failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void ToggleFavorite()
    {
        IsFavorite = !IsFavorite;
    }

    [RelayCommand]
    private async Task ShareEvent()
    {
        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Title = EventName,
            Text = $"Confira o evento: {EventName}"
        });
    }

    [RelayCommand]
    private async Task BuyTicket()
    {
        await Shell.Current.GoToAsync(nameof(Views.CheckoutPage));
    }
}
