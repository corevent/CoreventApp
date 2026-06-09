using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class OrderDetailPage : ContentPage
{
    public OrderDetailPage(OrderDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
