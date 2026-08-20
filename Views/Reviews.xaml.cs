namespace CoreventApp.Views;

public partial class Reviews : ContentPage
{
    private readonly ViewModels.ReviewsViewModel _viewModel;

    public Reviews(ViewModels.ReviewsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.LoadItemsCommand.ExecuteAsync(null);
    }
}
