using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
  private readonly IAuthService _authService;

  public ProfileViewModel(IAuthService authService)
  {
    _authService = authService;

    LoadUserAsync();
  }

  [ObservableProperty]
  public partial string UserName { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string UserEmail { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string UserAvatar { get; set; } = "profile_default_icon.png";

  private async void LoadUserAsync()
  {
    var user = await _authService.GetCurrentUserAsync();
    if (user != null)
    {
      UserName = user.Name;
      UserEmail = user.Email;
      
      if (!string.IsNullOrWhiteSpace(user.AvatarUrl))
      {
        UserAvatar = user.AvatarUrl;
      }
    }
  }

  [RelayCommand]
  private async Task EditProfileAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.EditProfile));
  }

  [RelayCommand]
  private async Task PurchaseHistoryAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.PurchaseHistory));
  }

  [RelayCommand]
  private async Task FavoritesAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.Favorites));
  }

  [RelayCommand]
  private async Task ReviewsAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.Reviews));
  }

  [RelayCommand]
  private async Task SettingsAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.Settings));
  }

  [RelayCommand]
  private async Task LogoutAsync()
  {
    bool confirm = await Shell.Current.DisplayAlertAsync("Aviso", "Deseja mesmo encerrar a sua sessão?", "Sim", "Cancelar");

    if (confirm)
    {
      await _authService.LogoutAsync();
      await Shell.Current.GoToAsync(nameof(Views.Login));
    }
  }

  [RelayCommand]
  private async Task PanelOrganizerAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.PanelOrganizer));
  }

  [RelayCommand]
  private async Task PanelCollaboratorAsync()
  {
    await Shell.Current.GoToAsync(nameof(Views.PanelCollaborator));
  }
}