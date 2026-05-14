using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class UpdateEmail : ContentPage
{
	public UpdateEmail(UpdateEmailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
