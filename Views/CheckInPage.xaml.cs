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
        CameraReader.BarcodesDetected += OnBarcodesDetected;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CameraReader.BarcodesDetected -= OnBarcodesDetected;
    }

    private void OnBarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
    {
        if (BindingContext is not CheckInViewModel vm) return;

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            if (vm.IsResultVisible || !vm.IsScanning) return;

            var barcode = e.Results?.FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(barcode)) return;

            await vm.ProcessBarcodeCommand.ExecuteAsync(barcode);
        });
    }
}
