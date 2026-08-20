using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class EventAttractions : ContentPage
{
    public EventAttractions(EventAttractionsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
