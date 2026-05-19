using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EventData), "EventData")]
public partial class CheckInViewModel : ObservableObject
{
    private EventSummary? _eventData;

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

    public EventSummary? EventData
    {
        set
        {
            if (value is null) return;

            _eventData = value;
            EventName = value.Name;

            var canCheckIn = value.Status is EventStatus.Opened or EventStatus.Going;
            IsScannerBlocked = !canCheckIn;
            IsScanning = canCheckIn;

            if (!canCheckIn)
            {
                ResultMessage = "Este evento não está disponível para check-in no momento.";
                IsResultVisible = true;
                IsResultSuccess = false;
            }
        }
    }

    [RelayCommand]
    private async Task ProcessBarcodeAsync(string? barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
            return;

        if (_eventData?.Status is not (EventStatus.Opened or EventStatus.Going))
            return;

        IsScanning = false;

        // Simulated check-in logic
        await Task.Delay(500);

        // Simulate success for any valid barcode
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
