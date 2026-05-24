namespace CoreventApp.Views;

public partial class EmailVerification : ContentPage
{
    public EmailVerification(ViewModels.EmailVerificationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
