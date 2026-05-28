using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class EditProfileViewModel : ObservableObject
{
  private readonly IAuthService _authService;

  public EditProfileViewModel(IAuthService authService)
  {
    _authService = authService;

    var cached = _authService.CurrentCachedUser;
    if (cached != null)
      ApplyUser(cached);
  }

  [ObservableProperty]
  public partial bool IsBusy { get; set; }

  [ObservableProperty]
  public partial string UserName { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string UserPhone { get; set; } = string.Empty;

  public async Task LoadUserAsync()
  {
    if (IsBusy)
      return;

    try
    {
      IsBusy = true;
      var user = await _authService.GetCurrentUserAsync();
      if (user != null)
        ApplyUser(user);
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"EditProfileViewModel.LoadUserAsync failed: {ex.Message}");
    }
    finally
    {
      IsBusy = false;
    }
  }

  private void ApplyUser(Models.User user)
  {
    UserName = user.Name;
    UserPhone = user.PhoneNumber ?? string.Empty;
  }

  [RelayCommand]
  private async Task Save()
  {
    if (!string.IsNullOrWhiteSpace(UserPhone) && !ValidationHelper.IsValidPhone(UserPhone))
    {
      await Shell.Current.DisplayAlertAsync("Erro", "Telefone inválido. Use o formato (11) 91234-5678.", "OK");
      return;
    }

    var success = await _authService.UpdateProfileAsync(UserName, UserPhone, null);
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
