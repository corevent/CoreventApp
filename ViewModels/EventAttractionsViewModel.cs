using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventName), "EventName")]
public partial class EventAttractionsViewModel : ObservableObject
{
    private readonly AttractionStore _store;

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewGuest { get; set; } = string.Empty;

    [ObservableProperty]
    public partial TimeSpan NewStartTime { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0).TimeOfDay;

    [ObservableProperty]
    public partial TimeSpan NewEndTime { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 15, 0, 0).TimeOfDay;

    [ObservableProperty]
    public partial bool HasAttractions { get; set; }

    public ObservableCollection<Attraction> Attractions { get; } = new();

    public EventAttractionsViewModel(AttractionStore store)
    {
        _store = store;
    }

    partial void OnEventNameChanged(string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        var stored = _store.GetAttractions(value);

        Attractions.CollectionChanged -= OnAttractionsChanged;
        Attractions.Clear();
        foreach (var a in stored)
            Attractions.Add(a);
        Attractions.CollectionChanged += OnAttractionsChanged;

        HasAttractions = Attractions.Count > 0;
    }

    private void OnAttractionsChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        HasAttractions = Attractions.Count > 0;
    }

    [RelayCommand]
    private async Task AddAttractionAsync()
    {
        if (string.IsNullOrWhiteSpace(NewTitle) || string.IsNullOrWhiteSpace(NewGuest))
            return;

        var baseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

        var attraction = new Attraction
        {
            Title = NewTitle.Trim(),
            Guest = NewGuest.Trim(),
            StartTime = NewStartTime,
            EndTime = NewEndTime
        };

        var stored = _store.GetAttractions(EventName);
        stored.Add(attraction);
        Attractions.Add(attraction);

        NewTitle = string.Empty;
        NewGuest = string.Empty;
        NewStartTime = baseTime.AddHours(14).TimeOfDay;
        NewEndTime = baseTime.AddHours(15).TimeOfDay;

        await Task.CompletedTask;
    }

    [RelayCommand]
    private void RemoveAttraction(Attraction attraction)
    {
        var stored = _store.GetAttractions(EventName);
        stored.Remove(attraction);
        Attractions.Remove(attraction);
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}

public partial class Attraction : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Guest { get; set; } = string.Empty;

    [ObservableProperty]
    public partial TimeSpan StartTime { get; set; }

    [ObservableProperty]
    public partial TimeSpan EndTime { get; set; }

    public string FormattedTimeRange => $"{StartTime.Hours:D2}:{StartTime.Minutes:D2} - {EndTime.Hours:D2}:{EndTime.Minutes:D2}";
}
