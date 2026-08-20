using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class TransferSettings : ContentPage
{
    private readonly TransferSettingsViewModel _viewModel;

    public TransferSettings(TransferSettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadCommand.ExecuteAsync(null);
    }
}
