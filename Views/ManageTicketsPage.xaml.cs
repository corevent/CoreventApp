using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class ManageTicketsPage : ContentPage
{
    public ManageTicketsPage(ManageTicketsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
