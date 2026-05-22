using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;
using CoreventApp.Services;
using CoreventApp.Models;

namespace CoreventApp.ViewModels;

public partial class EditProfileViewModel : ObservableObject
{

  private readonly IAuthService _authService;

  public EditProfileViewModel(IAuthService authService)
    {
        _authService = authService;
        LoadUserAsync();
    }

  [ObservableProperty]
  private string _userName = string.Empty;

  [ObservableProperty]
  private string _userEmail = string.Empty;

  [ObservableProperty]
  private string _userCellphone = string.Empty;

  [ObservableProperty]
  private string _userCpf = string.Empty;

  [ObservableProperty]
  private string _userBirthDate = string.Empty;

  [ObservableProperty]
  private string _userCreatedAt = string.Empty;

    private async void LoadUserAsync()
    {
        var user = await _authService.GetCurrentUserAsync();
        if (user != null)
        {
            UserName = user.Name;
            UserEmail = user.Email;
            UserCellphone = user.Cellphone ?? string.Empty;
            UserCpf = user.CPF ?? string.Empty;
            UserBirthDate = user.BirthDate ?? string.Empty;
            UserCreatedAt = user.CreatedAt.ToString("dd/MM/yyyy");
        }
    }


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