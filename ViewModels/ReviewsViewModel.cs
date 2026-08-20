using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class ReviewsViewModel : ObservableObject
{
    private readonly EventRatingsService _ratingsService;

    public ReviewsViewModel(EventRatingsService ratingsService)
    {
        _ratingsService = ratingsService;
    }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool IsEmpty { get; set; }

    [ObservableProperty]
    public partial bool IsRefreshing { get; set; }

    public ObservableCollection<MyRatingItemDto> Items { get; } = [];

    [RelayCommand]
    public async Task LoadItems()
    {
        IsLoading = true;

        var result = await _ratingsService.GetMyRatingsAsync(page: 1, limit: 100);

        Items.Clear();
        foreach (var item in result.Data)
            Items.Add(item);

        IsEmpty = Items.Count == 0;
        IsLoading = false;
    }

    [RelayCommand]
    private async Task Refresh()
    {
        var result = await _ratingsService.GetMyRatingsAsync(page: 1, limit: 100);

        Items.Clear();
        foreach (var item in result.Data)
            Items.Add(item);

        IsEmpty = Items.Count == 0;
        IsRefreshing = false;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
