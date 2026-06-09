namespace CoreventApp.Views;

public partial class Tickets : ContentPage
{
    private readonly ViewModels.TicketsViewModel _viewModel;

    public Tickets(ViewModels.TicketsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.ProximosTickets.Count == 0 && _viewModel.PassadosTickets.Count == 0)
            _viewModel.LoadTicketsCommand.Execute(null);
    }
}
