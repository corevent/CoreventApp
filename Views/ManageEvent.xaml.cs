namespace CoreventApp.Views;

public partial class ManageEvent : ContentPage
{
    public ManageEvent(ViewModels.ManageEventViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
