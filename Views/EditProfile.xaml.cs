namespace CoreventApp.Views;

public partial class EditProfile : ContentPage
{
	public EditProfile(ViewModels.EditProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}