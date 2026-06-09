namespace CoreventApp.Views;

public partial class PurchaseHistory : ContentPage
{
    private readonly ViewModels.PurchaseHistoryViewModel _viewModel;

    public PurchaseHistory(ViewModels.PurchaseHistoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.Orders.Count == 0)
            _viewModel.LoadOrdersCommand.Execute(null);
    }
}
