using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

public partial class EditProfileViewModel : ObservableObject
{
  public EditProfileViewModel()
  {
    FakeUser.Name = "João da Silva";
    FakeUser.Email = "joao.silva@email.com";
    FakeUser.Cellphone = "(11)9999-9999";
    FakeUser.BornDate = "15/02/2005";
    FakeUser.CPF = "123.456.789-00";
  }

  [ObservableProperty]
  public partial ProfileRequest FakeUser { get; set; } = new();

  [RelayCommand]
  private async Task DisplayMessage()
  {
    await Shell.Current.DisplayAlertAsync("Oba", "Perfil salvo com sucesso", "OK");
    await Shell.Current.GoToAsync("..");
  }

  [RelayCommand]
  private async Task GoBack()
  {
      await Shell.Current.GoToAsync("..");
  }
}

public partial class ProfileRequest : ObservableObject
{
  [ObservableProperty]
  public partial string Name { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Email { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Cellphone { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string BornDate { get; set; } = string.Empty;
  
  [ObservableProperty]
  public partial string CPF { get; set; } = string.Empty;
}