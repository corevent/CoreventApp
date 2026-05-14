using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

public partial class EditProfileViewModel : ObservableObject
{
  [RelayCommand]
  private async Task DisplayMessage()
  {
    await Shell.Current.DisplayAlertAsync("Oba", "Perfil salvo com sucesso", "OK");
    await Shell.Current.GoToAsync("..");
  }
}