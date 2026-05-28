using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Views;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    public ObservableCollection<EventListItemDto> FavoriteEvents { get; } = new();

    public FavoritesViewModel()
    {
        // Favorites are not yet backed by API — kept as local-only for now
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

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
