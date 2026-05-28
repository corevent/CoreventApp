using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventId), "EventId")]
public partial class CheckInViewModel : ObservableObject
{
    private readonly EventsService _eventsService;
    private string? _eventId;

    [ObservableProperty]
    public partial string EventName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string CheckInCount { get; set; } = "0/0";

    [ObservableProperty]
    public partial string ResultMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsResultVisible { get; set; }

    [ObservableProperty]
    public partial bool IsResultSuccess { get; set; }

    [ObservableProperty]
    public partial bool IsScannerBlocked { get; set; } = true;

    [ObservableProperty]
    public partial bool IsScanning { get; set; }

    private string _eventStatus = string.Empty;

    public string? EventId
    {
        set
        {
            _eventId = value;
            if (value is not null) _ = LoadEventAsync(value);
        }
    }

    public CheckInViewModel(EventsService eventsService)
    {
        _eventsService = eventsService;
    }

    private async Task LoadEventAsync(string eventId)
    {
        try
        {
            var evt = await _eventsService.GetByIdAsync(eventId);
            if (evt is null) return;

            EventName = evt.Title;
            _eventStatus = evt.Status;

            var canCheckIn = evt.Status is "opened" or "going";
            IsScannerBlocked = !canCheckIn;
            IsScanning = canCheckIn;

            if (!canCheckIn)
            {
                ResultMessage = "Este evento não está disponível para check-in no momento.";
                IsResultVisible = true;
                IsResultSuccess = false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CheckIn LoadEventAsync failed: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task ProcessBarcodeAsync(string? barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode)) return;

        if (_eventStatus is not ("opened" or "going"))
            return;

        IsScanning = false;

        // Simulated check-in logic
        await Task.Delay(500);

        IsResultSuccess = true;
        ResultMessage = $"Check-in confirmado para o ingresso #{barcode}";
        IsResultVisible = true;
    }

    [RelayCommand]
    private void DismissResult()
    {
        IsResultVisible = false;
        ResultMessage = string.Empty;
        IsScanning = true;
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
