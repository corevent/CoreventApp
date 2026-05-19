using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

public partial class AddBankAccountViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string BankCode { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Agency { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string AgencyDigit { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string AccountNumber { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string AccountDigit { get; set; } = string.Empty;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "A descrição é obrigatória.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(BankCode) || BankCode.Length < 3)
        {
            await Shell.Current.DisplayAlertAsync("Erro", "O código do banco é inválido.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Agency))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "A agência é obrigatória.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(AccountNumber))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "O número da conta é obrigatório.", "OK");
            return;
        }

        // Logic to save would go here (or passing back to TransferSettings)
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
