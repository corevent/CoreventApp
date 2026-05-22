namespace CoreventApp.Views;

public partial class EventDetail : ContentPage
{
    public EventDetail(ViewModels.EventDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
