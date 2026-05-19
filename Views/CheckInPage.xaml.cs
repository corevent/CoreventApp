using CoreventApp.ViewModels;
using ZXing.Net.Maui;

namespace CoreventApp.Views;

public partial class CheckInPage : ContentPage
{
    public CheckInPage(CheckInViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        cameraReader.BarcodesDetected += OnBarcodesDetected;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        cameraReader.BarcodesDetected -= OnBarcodesDetected;
    }

    private async void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        var barcode = e.Results?.FirstOrDefault()?.Value;
        if (string.IsNullOrWhiteSpace(barcode)) return;

        if (BindingContext is CheckInViewModel vm)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await vm.ProcessBarcodeCommand.ExecuteAsync(barcode);
            });
        }
    }
}
