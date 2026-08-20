namespace CoreventApp.Views;

public partial class ParticipantList : ContentPage
{
    public ParticipantList(ViewModels.ParticipantListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
