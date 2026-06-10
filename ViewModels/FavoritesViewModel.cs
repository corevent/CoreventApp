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
    private readonly IAuthService _authService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    public ObservableCollection<EventListItemDto> FavoriteEvents { get; } = new();

    public FavoritesViewModel(EventsService eventsService, FavoritesService favoritesService, IAuthService authService)
    {
        _eventsService = eventsService;
        _favoritesService = favoritesService;
        _authService = authService;
    }

    [RelayCommand]
    private async Task LoadFavoritesAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var result = await _eventsService.GetMyFavoriteEventsAsync();
            var filtered = _authService.CurrentCachedUser?.IsAdult == false
                ? result.Data.Where(e => !e.IsAdultOnly).ToList()
                : result.Data;
            FavoriteEvents.Clear();
            foreach (var item in filtered)
                FavoriteEvents.Add(item);

            _favoritesService.SetFavorites(filtered);
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
            var filtered = _authService.CurrentCachedUser?.IsAdult == false
                ? result.Data.Where(e => !e.IsAdultOnly).ToList()
                : result.Data;
            FavoriteEvents.Clear();
            foreach (var item in filtered)
                FavoriteEvents.Add(item);

            _favoritesService.SetFavorites(filtered);
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
