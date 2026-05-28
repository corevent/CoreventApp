using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class Explore : ContentPage
{
	public Explore(ExploreViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		if (BindingContext is ExploreViewModel vm)
		{
			_ = vm.SearchCommand.ExecuteAsync(null);
		}
	}
}
