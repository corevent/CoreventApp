using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

public partial class ReviewsViewModel : ObservableObject
{
  [RelayCommand]
  private async Task GoBack()
  {
      await Shell.Current.GoToAsync("..");
  }
}