using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventData), "EventData")]
public partial class EventDetailViewModel : ObservableObject
{
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
            }
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
        await Shell.Current.DisplayAlertAsync("Ingresso",
            $"Você será redirecionado para compra de:\n{EventName}\n{Price}", "OK");
    }
}
