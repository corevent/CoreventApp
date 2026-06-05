using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class UpdateEmailViewModel : ObservableObject
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    public partial string CurrentEmail { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewEmail { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ConfirmEmail { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsBusy { get; set; }

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool IsNotBusy => !IsBusy;

    public UpdateEmailViewModel(IAuthService authService)
    {
        _authService = authService;
        _ = LoadCurrentEmail();
    }

    private async Task LoadCurrentEmail()
    {
        var user = await _authService.GetCurrentUserAsync();
        CurrentEmail = user?.Email ?? string.Empty;
    }

    [RelayCommand]
    private async Task GoBack() => await Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task UpdateEmail()
    {
        if (string.IsNullOrWhiteSpace(NewEmail) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Preencha todos os campos.";
            return;
        }

        if (!ValidationHelper.IsValidEmail(NewEmail))
        {
            ErrorMessage = "Informe um e-mail válido.";
            return;
        }

        if (NewEmail != ConfirmEmail)
        {
            ErrorMessage = "Os e-mails não coincidem.";
            return;
        }

        IsBusy = true;
        OnPropertyChanged(nameof(IsNotBusy));
        ErrorMessage = string.Empty;

        try
        {
            bool success = await _authService.UpdateEmailAsync(NewEmail, Password);
            if (success)
            {
                await Shell.Current.DisplayAlertAsync("Sucesso", "E-mail atualizado com sucesso!", "OK");
                await GoBack();
            }
            else
            {
                ErrorMessage = "Senha incorreta.";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"UpdateEmail failed: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Erro", "Ocorreu um erro ao atualizar o e-mail.", "OK");
        }
        finally
        {
            IsBusy = false;
            OnPropertyChanged(nameof(IsNotBusy));
        }
    }
}
