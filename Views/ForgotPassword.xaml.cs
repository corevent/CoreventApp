namespace CoreventApp.Views;

public partial class ForgotPassword : ContentPage
{
    public ForgotPassword(ViewModels.ForgotPasswordViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
