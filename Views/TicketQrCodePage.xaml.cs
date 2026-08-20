using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class TicketQrCodePage : ContentPage
{
    public TicketQrCodePage(TicketQrCodeViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
