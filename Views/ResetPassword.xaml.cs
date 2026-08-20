namespace CoreventApp.Views;

public partial class ResetPassword : ContentPage
{
    public ResetPassword(ViewModels.ResetPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
