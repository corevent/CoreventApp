using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public record StatusFilterChip(string Label, string? StatusValue, bool IsSelected);

public partial class PanelOrganizerViewModel : ObservableObject
{
    private readonly EventsService _eventsService;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<EventListItemDto> AllEvents { get; } = new();
    public ObservableCollection<EventListItemDto> FilteredEvents { get; } = new();
    public ObservableCollection<StatusFilterChip> FilterChips { get; } = new();

    public PanelOrganizerViewModel(EventsService eventsService)
    {
        _eventsService = eventsService;

        FilterChips.Add(new StatusFilterChip("Todos", null, true));
        FilterChips.Add(new StatusFilterChip("Rascunho", "draft", false));
        FilterChips.Add(new StatusFilterChip("Ativo", "opened", false));
        FilterChips.Add(new StatusFilterChip("Andamento", "going", false));
        FilterChips.Add(new StatusFilterChip("Cancelado", "canceled", false));
        FilterChips.Add(new StatusFilterChip("Encerrado", "finished", false));
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var result = await _eventsService.GetMyOrganizerEventsAllAsync(page: 1, limit: 100);

            AllEvents.Clear();
            foreach (var item in result.Data)
                AllEvents.Add(item);

            ApplyFilter(null);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível carregar seus eventos.", "OK");
            Debug.WriteLine($"PanelOrganizer LoadAsync failed: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void FilterByStatus(StatusFilterChip? chip)
    {
        if (chip is null) return;

        foreach (var c in FilterChips.ToArray())
        {
            if (c == chip)
                FilterChips[FilterChips.IndexOf(c)] = c with { IsSelected = true };
            else
                FilterChips[FilterChips.IndexOf(c)] = c with { IsSelected = false };
        }

        ApplyFilter(chip?.StatusValue);
    }

    private void ApplyFilter(string? statusValue)
    {
        FilteredEvents.Clear();

        var filtered = statusValue is null
            ? AllEvents
            : AllEvents.Where(e => e.Status == statusValue);

        foreach (var item in filtered)
            FilteredEvents.Add(item);
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task ConfigureTransferAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.TransferSettings));
    }

    [RelayCommand]
    private async Task NewEventAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CreateEvent));
    }

    [RelayCommand]
    private async Task SelectEventAsync(EventListItemDto? eventItem)
    {
        if (eventItem is null) return;

        await Shell.Current.GoToAsync(nameof(Views.ManageEvent), new Dictionary<string, object>
        {
            ["EventId"] = eventItem.Id
        });
    }
}
