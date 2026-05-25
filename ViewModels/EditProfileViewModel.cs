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
  public partial string UserName { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string UserCpf { get; set; } = string.Empty;

  [ObservableProperty]
  public partial DateTime BirthDate { get; set; } = DateTime.Today;

  private async void LoadUserAsync()
  {
    var user = await _authService.GetCurrentUserAsync();
    if (user != null)
    {
      UserName = user.Name;
      UserCpf = user.CPF ?? string.Empty;
      if (DateTime.TryParse(user.BirthDate, out var parsed))
        BirthDate = parsed;
    }
  }


  [RelayCommand]
  private async Task Save()
  {
    var success = await _authService.UpdateProfileAsync(UserName, UserCpf, BirthDate.ToString("dd/MM/yyyy"));
    if (success)
    {
      await Shell.Current.GoToAsync("..");
    }
  }

  [RelayCommand]
  private async Task GoBack()
  {
    await Shell.Current.GoToAsync("..");
  }
}