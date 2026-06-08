namespace CoreventApp.Views;

public partial class Favorites : ContentPage
{
	private readonly ViewModels.FavoritesViewModel _viewModel;

	public Favorites(ViewModels.FavoritesViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_ = _viewModel.LoadFavoritesCommand.ExecuteAsync(null);
	}
}