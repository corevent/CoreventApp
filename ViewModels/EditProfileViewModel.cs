using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class EditProfileViewModel : ObservableObject
{
  private readonly IAuthService _authService;
  private readonly StorageService _storageService;
  private Stream? _avatarStream;
  private string? _avatarContentType;

  public EditProfileViewModel(IAuthService authService, StorageService storageService)
  {
    _authService = authService;
    _storageService = storageService;

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

  [ObservableProperty]
  public partial string UserAvatar { get; set; } = string.Empty;

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
      await Shell.Current.DisplayAlertAsync("Erro", $"EditProfileViewModel.LoadUserAsync failed: {ex.Message}", "OK");
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
    UserAvatar = user.AvatarUrl;
  }

  [RelayCommand]
  private async Task PickAvatar()
  {
    try
    {
      var photo = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
      {
        Title = "Selecionar foto"
      });

      if (photo is null) return;

      var contentType = photo.ContentType?.ToLowerInvariant() ?? string.Empty;
      if (contentType != "image/jpeg" && contentType != "image/png" && contentType != "image/webp" && contentType != "image/jpg")
      {
        await Shell.Current.DisplayAlertAsync("Formato inválido", "Selecione uma imagem nos formatos JPEG, PNG ou WebP.", "OK");
        return;
      }

      _avatarStream?.Dispose();
      _avatarStream = await photo.OpenReadAsync();
      _avatarContentType = contentType;

      _avatarStream.Position = 0;
      UserAvatar = photo.FullPath;
    }
    catch (Exception ex)
    {
      Debug.WriteLine($"PickAvatar failed: {ex.Message}");
    }
  }

  [RelayCommand]
  private async Task Save()
  {
    if (string.IsNullOrWhiteSpace(UserName) || UserName.Trim().Length < 3)
    {
      await Shell.Current.DisplayAlertAsync("Erro", "O nome deve ter pelo menos 3 caracteres.", "OK");
      return;
    }

    if (!string.IsNullOrWhiteSpace(UserPhone) && !ValidationHelper.IsValidPhone(UserPhone))
    {
      await Shell.Current.DisplayAlertAsync("Erro", "Telefone inválido. Use o formato (11) 91234-5678.", "OK");
      return;
    }

    string? avatarUrl = null;

    if (_avatarStream is not null)
    {
      IsBusy = true;
      _avatarStream.Position = 0;
      avatarUrl = await _storageService.UploadAvatarAsync(_avatarStream, _avatarContentType!);
      IsBusy = false;

      if (avatarUrl is null)
      {
        await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível fazer upload da imagem. Verifique sua conexão.", "OK");
        return;
      }
    }

    var phone = string.IsNullOrWhiteSpace(UserPhone) ? null : UserPhone;
    var success = await _authService.UpdateProfileAsync(UserName.Trim(), phone);
    if (success)
    {
      await Shell.Current.GoToAsync("..");
    }
  }

  [RelayCommand]
  private async Task GoBack()
  {
    _avatarStream?.Dispose();
    _avatarStream = null;
    await Shell.Current.GoToAsync("..");
  }
}
