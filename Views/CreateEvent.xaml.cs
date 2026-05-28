namespace CoreventApp.Views;

public partial class CreateEvent : ContentPage
{
    private readonly ViewModels.CreateEventViewModel _vm;

    public CreateEvent(ViewModels.CreateEventViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _vm = viewModel;
        StatePicker.SelectedIndexChanged += OnStatePickerChanged;
    }

    private async void OnStatePickerChanged(object? sender, EventArgs e)
    {
        if (_vm.Form.SelectedState is not null)
            await _vm.LoadCitiesCommand.ExecuteAsync(null);
    }
}
