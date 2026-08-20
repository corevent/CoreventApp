namespace CoreventApp.Views;

public partial class EventTeam : ContentPage
{
    public EventTeam(ViewModels.EventTeamViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
