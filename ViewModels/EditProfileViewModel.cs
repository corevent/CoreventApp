using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;
using CoreventApp.Models;

namespace CoreventApp.ViewModels;

public partial class EditProfileViewModel : ObservableObject
{
  public EditProfileViewModel()
  {
    FakeUser.Name = "João da Silva";
    FakeUser.Email = "joao.silva@email.com";
    FakeUser.CPF = "123.456.789-00";
    FakeUser.BirthDate = "15/02/2005";
    FakeUser.Cellphone = "(11)9999-9999";
    FakeUser.AvatarUrl = "https://www.pngall.com/wp-content/uploads/5/Profile-PNG-High-Quality-Image.png";
    FakeUser.CreatedAt = DateTime.Now;
  }

  public User FakeUser { get; set; } = new();

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