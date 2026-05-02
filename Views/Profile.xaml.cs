namespace CoreventApp.Views;

public partial class Profile : ContentPage
{
    public Profile(ViewModels.ProfileViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
