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
		_viewModel.LoadTicketsCommand.Execute(null);
	}
}
