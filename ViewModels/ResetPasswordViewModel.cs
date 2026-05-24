using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(Email), "Email")]
public partial class ResetPasswordViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public ResetPasswordViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ConfirmPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsError { get; set; }

    partial void OnNewPasswordChanged(string value)
    {
        if (IsError) ClearError();
    }

    partial void OnConfirmPasswordChanged(string value)
    {
        if (IsError) ClearError();
    }

    [RelayCommand]
    private async Task ResetAsync()
    {
        if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 6)
        {
            ShowError("A senha deve ter pelo menos 6 caracteres.");
            return;
        }

        if (NewPassword != ConfirmPassword)
        {
            ShowError("As senhas não conferem.");
            return;
        }

        IsLoading = true;
        ClearError();

        var success = await _authService.ResetPasswordAsync(Email, NewPassword);

        IsLoading = false;

        if (success)
        {
            await Shell.Current.DisplayAlertAsync("Sucesso",
                "Senha redefinida com sucesso!", "OK");
            await Shell.Current.GoToAsync("../..");
        }
        else
        {
            ShowError("Ocorreu um erro ao redefinir a senha. Tente novamente.");
        }
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private void ShowError(string message)
    {
        IsError = true;
        ErrorMessage = message;
    }

    private void ClearError()
    {
        IsError = false;
        ErrorMessage = string.Empty;
    }
}
