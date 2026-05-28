using CoreventApp.ViewModels;

namespace CoreventApp.Views;

public partial class Home : ContentPage
{
  public Home(HomeViewModel viewModel)
  {
    InitializeComponent();
    BindingContext = viewModel;
  }

  protected override void OnAppearing()
  {
    base.OnAppearing();
    if (BindingContext is HomeViewModel vm)
      _ = vm.LoadCommand.ExecuteAsync(null);
  }
}
