namespace CoreventApp.Views;

public partial class Register : ContentPage
{
    public Register(ViewModels.RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
