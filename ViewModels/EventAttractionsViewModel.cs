using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class EventAttractionsViewModel : ObservableObject
{
    private readonly AttractionsService _attractionsService;

    [ObservableProperty]
    public partial string EventId { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewGuest { get; set; } = string.Empty;

    [ObservableProperty]
    public partial TimeSpan NewStartTime { get; set; } = new(14, 0, 0);

    [ObservableProperty]
    public partial TimeSpan NewEndTime { get; set; } = new(15, 0, 0);

    [ObservableProperty]
    public partial bool HasAttractions { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<Attraction> Attractions { get; } = new();

    public EventAttractionsViewModel(AttractionsService attractionsService)
    {
        _attractionsService = attractionsService;
    }

    partial void OnEventIdChanged(string value)
    {
        if (!string.IsNullOrEmpty(value))
            _ = LoadAttractionsAsync();
    }

    private async Task LoadAttractionsAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var result = await _attractionsService.GetAllAsync(EventId);
            Attractions.Clear();
            foreach (var item in result.Data)
                Attractions.Add(MapToPresentation(item));
            HasAttractions = Attractions.Count > 0;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private async Task AddAttractionAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTitle) || string.IsNullOrWhiteSpace(NewGuest))
            return;

        var baseDate = DateTime.Today;
        var dto = new CreateAttractionDto(
            NewTitle.Trim(),
            NewGuest.Trim(),
            baseDate + NewStartTime,
            baseDate + NewEndTime);

        var result = await _attractionsService.CreateAsync(EventId, dto);
        if (result is null) return;

        Attractions.Add(MapToPresentation(result));
        HasAttractions = true;

        NewTitle = string.Empty;
        NewGuest = string.Empty;
        NewStartTime = new TimeSpan(14, 0, 0);
        NewEndTime = new TimeSpan(15, 0, 0);
    }

    [RelayCommand]
    private async Task RemoveAttraction(Attraction attraction)
    {
        var success = await _attractionsService.DeleteAsync(attraction.Id);
        if (!success) return;

        Attractions.Remove(attraction);
        HasAttractions = Attractions.Count > 0;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private static Attraction MapToPresentation(AttractionDto dto) => new()
    {
        Id = dto.Id,
        Title = dto.Title,
        Guest = dto.Guest,
        StartDate = dto.StartDate,
        EndDate = dto.EndDate
    };
}

public partial class Attraction : ObservableObject
{
    [ObservableProperty]
    public partial string Id { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Guest { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DateTime StartDate { get; set; }

    [ObservableProperty]
    public partial DateTime EndDate { get; set; }

    public string FormattedTimeRange => $"{StartDate:HH:mm} - {EndDate:HH:mm}";
}
