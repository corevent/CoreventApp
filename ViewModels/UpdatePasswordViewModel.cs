using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;
using System.Text.RegularExpressions;

namespace CoreventApp.ViewModels;

public partial class UpdatePasswordViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    public partial string CurrentPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ConfirmPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool IsNotBusy => !IsBusy;

    public UpdatePasswordViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task UpdatePassword()
    {
        if (string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword))
        {
            ErrorMessage = "Preencha todos os campos.";
            return;
        }

        if (NewPassword != ConfirmPassword)
        {
            ErrorMessage = "As senhas não coincidem.";
            return;
        }

        if (!ValidatePasswordComplexity(NewPassword))
        {
            ErrorMessage = "A senha deve ter 8+ caracteres, maiúscula, minúscula, número e símbolo.";
            return;
        }

        IsBusy = true;
        OnPropertyChanged(nameof(IsNotBusy));
        ErrorMessage = string.Empty;

        try
        {
            bool success = await _authService.UpdatePasswordAsync(CurrentPassword, NewPassword);
            if (success)
            {
                await Shell.Current.DisplayAlertAsync("Sucesso", "Senha atualizada com sucesso!", "OK");
                await GoBack();
            }
            else
            {
                ErrorMessage = "Senha atual incorreta.";
            }
        }
        catch (Exception)
        {
            ErrorMessage = "Ocorreu um erro ao atualizar.";
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }

    private bool ValidatePasswordComplexity(string password)
    {
        if (password.Length < 8) return false;
        if (!Regex.IsMatch(password, @"[A-Z]")) return false;
        if (!Regex.IsMatch(password, @"[a-z]")) return false;
        if (!Regex.IsMatch(password, @"[0-9]")) return false;
        if (!Regex.IsMatch(password, @"[!@#\$%\^&\*\(\),\.\?\"":\{\}\|<>]")) return false;
        return true;
    }
}
