using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
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

    [ObservableProperty]
    public partial string? EditingAttractionId { get; set; }

    partial void OnEditingAttractionIdChanged(string? value)
    {
        OnPropertyChanged(nameof(IsEditing));
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(FormButtonText));
    }

    public bool IsEditing => EditingAttractionId is not null;
    public string FormTitle => IsEditing ? "Editar Atração" : "Adicionar Atração";
    public string FormButtonText => IsEditing ? "Salvar" : "Adicionar";

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
    private void EditAttraction(Attraction attraction)
    {
        EditingAttractionId = attraction.Id;
        NewTitle = attraction.Title;
        NewGuest = attraction.Guest;
        NewStartTime = attraction.StartDate.TimeOfDay;
        NewEndTime = attraction.EndDate.TimeOfDay;
    }

    [RelayCommand]
    private void CancelEdit()
    {
        EditingAttractionId = null;
        NewTitle = string.Empty;
        NewGuest = string.Empty;
        NewStartTime = new TimeSpan(14, 0, 0);
        NewEndTime = new TimeSpan(15, 0, 0);
    }

    [RelayCommand]
    private async Task AddAttractionAsync()
    {
        if (IsEditing && EditingAttractionId is not null)
        {
            await UpdateAttractionAsync();
            return;
        }

        if (string.IsNullOrWhiteSpace(NewTitle) || string.IsNullOrWhiteSpace(NewGuest))
            return;

        if (NewTitle.Trim().Length < 3)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "O título da atração deve ter pelo menos 3 caracteres.", "OK");
            return;
        }

        if (NewGuest.Trim().Length < 3)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "O nome do convidado deve ter pelo menos 3 caracteres.", "OK");
            return;
        }

        if (NewEndTime <= NewStartTime)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "O horário de término deve ser posterior ao horário de início.", "OK");
            return;
        }

        var baseDate = DateTime.Today;
        var startDt = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day,
            NewStartTime.Hours, NewStartTime.Minutes, NewStartTime.Seconds, DateTimeKind.Local);
        var endDt = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day,
            NewEndTime.Hours, NewEndTime.Minutes, NewEndTime.Seconds, DateTimeKind.Local);
        var dto = new CreateAttractionDto(
            NewTitle.Trim(),
            NewGuest.Trim(),
            startDt.ToUniversalTime(),
            endDt.ToUniversalTime());

        var result = await _attractionsService.CreateAsync(EventId, dto);
        if (result is null) return;

        Attractions.Add(MapToPresentation(result));
        HasAttractions = true;

        ResetForm();
    }

    private async Task UpdateAttractionAsync()
    {
        if (EditingAttractionId is null) return;

        if (NewEndTime <= NewStartTime)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "O horário de término deve ser posterior ao horário de início.", "OK");
            return;
        }

        var baseDate = DateTime.Today;
        var startDt = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day,
            NewStartTime.Hours, NewStartTime.Minutes, NewStartTime.Seconds, DateTimeKind.Local);
        var endDt = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day,
            NewEndTime.Hours, NewEndTime.Minutes, NewEndTime.Seconds, DateTimeKind.Local);
        var dto = new UpdateAttractionDto(
            NewTitle.Trim(),
            NewGuest.Trim(),
            startDt.ToUniversalTime(),
            endDt.ToUniversalTime());

        var result = await _attractionsService.UpdateAsync(EditingAttractionId, dto);
        if (result is null) return;

        var existing = Attractions.FirstOrDefault(a => a.Id == EditingAttractionId);
        if (existing is not null)
        {
            existing.Title = result.Title;
            existing.Guest = result.Guest;
            existing.StartDate = result.StartDate;
            existing.EndDate = result.EndDate;
        }

        ResetForm();
    }

    private void ResetForm()
    {
        EditingAttractionId = null;
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
        StartDate = dto.StartDate.ToLocalTime(),
        EndDate = dto.EndDate.ToLocalTime()
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
