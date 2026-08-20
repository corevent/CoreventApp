using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class EditProfile : ContentPage
{
	public EditProfile(EditProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is EditProfileViewModel vm)
            _ = vm.LoadUserAsync();
    }
}
