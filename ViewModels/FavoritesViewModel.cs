using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;
using CoreventApp.Views;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    private readonly EventsService _eventsService;
    private readonly FavoritesService _favoritesService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    public ObservableCollection<EventListItemDto> FavoriteEvents { get; } = new();

    public FavoritesViewModel(EventsService eventsService, FavoritesService favoritesService)
    {
        _eventsService = eventsService;
        _favoritesService = favoritesService;
    }

    [RelayCommand]
    private async Task LoadFavoritesAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var result = await _eventsService.GetMyFavoriteEventsAsync();
            FavoriteEvents.Clear();
            foreach (var item in result.Data)
                FavoriteEvents.Add(item);

            _favoritesService.SetFavorites(result.Data);
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsRefreshing = true;

        try
        {
            var result = await _eventsService.GetMyFavoriteEventsAsync();
            FavoriteEvents.Clear();
            foreach (var item in result.Data)
                FavoriteEvents.Add(item);

            _favoritesService.SetFavorites(result.Data);
        }
        finally
        {
            IsRefreshing = false;
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

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
