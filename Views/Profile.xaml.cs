using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class Profile : ContentPage
{
    public Profile(ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProfileViewModel vm)
            _ = vm.LoadUserAsync();
    }
}
