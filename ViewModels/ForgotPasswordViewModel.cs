using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class ForgotPasswordViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    public ForgotPasswordViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [RelayCommand]
    private async Task SendResetCodeAsync()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            await Shell.Current.DisplayAlertAsync("Aviso", "Informe seu e-mail para recuperar a senha.", "OK");
            return;
        }

        IsLoading = true;

        await _authService.SendResetCodeAsync(Email);

        IsLoading = false;

        await Shell.Current.DisplayAlertAsync("E-mail enviado",
            "Se o e-mail estiver cadastrado, enviaremos um código de verificação.", "OK");

        await Shell.Current.GoToAsync(
            $"EmailVerification?Email={Uri.EscapeDataString(Email)}&Mode=reset");
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
