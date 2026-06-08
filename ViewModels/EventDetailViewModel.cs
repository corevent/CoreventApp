using System.Collections.ObjectModel;
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
    private readonly AttractionsService _attractionsService;
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
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsAdultOnlyVisible { get; set; }

    [ObservableProperty]
    public partial bool IsPhysicalLocation { get; set; } = true;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool HasAttractions { get; set; }

    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    partial void OnDescriptionChanged(string value) => OnPropertyChanged(nameof(HasDescription));

    partial void OnLocationTypeDisplayChanged(string value) => OnPropertyChanged(nameof(IsPhysicalLocation));

    public string? EventId
    {
        set
        {
            _eventId = value;
            if (value is not null) _ = LoadEventAsync(value);
        }
    }

    public ObservableCollection<AttractionDto> Attractions { get; } = new();

    public EventDetailViewModel(EventsService eventsService, AttractionsService attractionsService)
    {
        _eventsService = eventsService;
        _attractionsService = attractionsService;
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
            EventDate = $"{evt.StartDate:dd MMM, yyyy} • {evt.StartDate:HH:mm} - {evt.EndDate:HH:mm}";
            ImageUrl = evt.BannerUrl ?? string.Empty;
            Category = evt.Category;
            Description = evt.Description ?? string.Empty;
            IsAdultOnlyVisible = evt.IsAdultOnly;
            IsPhysicalLocation = evt.LocationType != "online";

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

            _ = LoadAttractionsAsync(eventId);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"EventDetail LoadEventAsync failed: {ex.Message}", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadAttractionsAsync(string eventId)
    {
        try
        {
            var result = await _attractionsService.GetAllAsync(eventId);
            Attractions.Clear();
            foreach (var item in result.Data)
                Attractions.Add(item);
            HasAttractions = Attractions.Count > 0;
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", $"EventDetail LoadAttractionsAsync failed: {ex.Message}", "OK");
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
        if (_eventId is null) return;
        await Shell.Current.GoToAsync($"{nameof(Views.CheckoutPage)}?EventId={_eventId}");
    }
}
