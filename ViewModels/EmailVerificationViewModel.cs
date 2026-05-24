using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(Email), "Email")]
[QueryProperty(nameof(Password), "Password")]
[QueryProperty(nameof(Mode), "Mode")]
public partial class EmailVerificationViewModel : ObservableObject
{
    private readonly IAuthService _authService;
    private IDispatcherTimer? _resendTimer;
    private const int ResendCooldownSeconds = 30;

    public EmailVerificationViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Mode { get; set; } = "register";

    [ObservableProperty]
    public partial string Code { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsError { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial bool CanResend { get; set; } = true;

    [ObservableProperty]
    public partial string ResendText { get; set; } = "Reenviar código";

    [ObservableProperty]
    public partial int ResendCountdown { get; set; }

    partial void OnCodeChanged(string value)
    {
        if (IsError)
        {
            IsError = false;
            ErrorMessage = string.Empty;
        }
    }

    [RelayCommand]
    private async Task VerifyAsync()
    {
        if (string.IsNullOrWhiteSpace(Code) || Code.Length != 6 || !Code.All(char.IsDigit))
        {
            IsError = true;
            ErrorMessage = "Insira o código de 6 dígitos enviado por e-mail.";
            return;
        }

        IsLoading = true;
        IsError = false;
        ErrorMessage = string.Empty;

        bool isValid;

        if (Mode == "reset")
        {
            isValid = await _authService.VerifyResetCodeAsync(Email, Code);
        }
        else
        {
            isValid = await _authService.VerifyCodeAsync(Email, Code);
        }

        IsLoading = false;

        if (isValid)
        {
            if (Mode == "reset")
            {
                await Shell.Current.GoToAsync(
                    $"ResetPassword?Email={Uri.EscapeDataString(Email)}");
            }
            else
            {
                var user = await _authService.LoginAsync(Email, Password);
                if (user != null)
                {
                    await Shell.Current.GoToAsync("//main/home");
                }
                else
                {
                    await Shell.Current.GoToAsync("//welcome");
                }
            }
        }
        else
        {
            IsError = true;
            ErrorMessage = "Código inválido. Verifique o código recebido e tente novamente.";
        }
    }

    [RelayCommand]
    private async Task ResendAsync()
    {
        if (!CanResend) return;

        await _authService.SendVerificationEmailAsync(Email);

        CanResend = false;
        ResendCountdown = ResendCooldownSeconds;
        ResendText = $"Reenviar código ({ResendCountdown}s)";

        _resendTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_resendTimer == null) return;

        _resendTimer.Interval = TimeSpan.FromSeconds(1);
        _resendTimer.Tick += (s, e) =>
        {
            ResendCountdown--;
            ResendText = $"Reenviar código ({ResendCountdown}s)";

            if (ResendCountdown <= 0)
            {
                _resendTimer?.Stop();
                CanResend = true;
                ResendText = "Reenviar código";
            }
        };
        _resendTimer.Start();
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        _resendTimer?.Stop();
        await Shell.Current.GoToAsync("..");
    }
}
