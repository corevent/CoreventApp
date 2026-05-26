using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventData), "EventData")]
public partial class EventDetailViewModel : ObservableObject
{
    private readonly AttractionStore _store;
    private EventSummary? _event;

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Category { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string EventDate { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Location { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Price { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ImageUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OrganizerName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string OrganizerAvatar { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsFavorite { get; set; }

    [ObservableProperty]
    public partial EventType Type { get; set; } = EventType.Presencial;

    [ObservableProperty]
    public partial string OnlineUrl { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool HasAttractions { get; set; }

    [ObservableProperty]
    public partial bool NoAttractions { get; set; } = true;

    public ObservableCollection<Attraction> Attractions { get; } = new();

    public EventDetailViewModel(AttractionStore store)
    {
        _store = store;
    }

    public EventSummary? EventData
    {
        set
        {
            if (value is not null)
            {
                _event = value;
                EventName = value.Name;
                EventDate = value.Date;
                ImageUrl = value.ImageUrl;
                Category = value.Category;
                Location = value.Location;
                Description = value.Description;
                Price = value.Price;
                OrganizerName = value.OrganizerName;
                OrganizerAvatar = value.OrganizerAvatar;
                Type = value.Type;
                OnlineUrl = value.OnlineUrl;
                IsFavorite = value.IsFavorite;

                LoadAttractions();
            }
        }
    }

    private void LoadAttractions()
    {
        var stored = _store.GetAttractions(EventName);
        Attractions.Clear();
        foreach (var a in stored)
            Attractions.Add(a);
        HasAttractions = Attractions.Count > 0;
        NoAttractions = !HasAttractions;
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
        if (_event is not null)
            _event.IsFavorite = IsFavorite;
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
