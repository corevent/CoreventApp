using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
  private readonly IAuthService _authService;

  public LoginViewModel(IAuthService authService)
  {
    _authService = authService;
  }

  [ObservableProperty]
  public partial LoginRequest Form { get; set; } = new();

  [RelayCommand]
  private async Task LoginAsync()
  {
    if (string.IsNullOrWhiteSpace(Form.Email) || string.IsNullOrWhiteSpace(Form.Password))
    {
      await Shell.Current.DisplayAlertAsync("Erro", "Preencha todos os campos.", "OK");
      return;
    }

    var user = await _authService.LoginAsync(Form.Email, Form.Password);

    if (user != null)
    {
      await Shell.Current.GoToAsync("//main/home");
    }
    else
    {
      await Shell.Current.DisplayAlertAsync("Erro", "E-mail ou senha incorretos.", "OK");
    }
  }

  [RelayCommand]
  private async Task GoToRegisterAsync()
  {
    await Shell.Current.GoToAsync(nameof(Register));
  }

  [RelayCommand]
  private async Task MissingPasswordAsync()
  {
    await Shell.Current.GoToAsync(nameof(ForgotPassword));
  }
}

public partial class LoginRequest : ObservableObject
{
  [ObservableProperty]
  public partial string Email { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Password { get; set; } = string.Empty;
}