using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class BankAccount : ObservableObject
{
    [ObservableProperty]
    public partial string BankName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string BankDetails { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string AccountNumber { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsDefault { get; set; }
}

public partial class PixKey : ObservableObject
{
    [ObservableProperty]
    public partial string Key { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Type { get; set; } = string.Empty;
}

public partial class TransferSettingsViewModel : ObservableObject
{
    [ObservableProperty]
    public partial BankAccount PrimaryAccount { get; set; } = new();

    public ObservableCollection<PixKey> PixKeys { get; } = new();

    public TransferSettingsViewModel()
    {
        LoadMockData();
    }

    private void LoadMockData()
    {
        PrimaryAccount = new BankAccount
        {
            BankName = "Conta Principal (Nubank)",
            BankDetails = "CÓD 260 • AG 0001",
            AccountNumber = "Conta 424242-2",
            IsDefault = true
        };

        PixKeys.Add(new PixKey { Key = "financeiro@empresa.com", Type = "EMAIL" });
        PixKeys.Add(new PixKey { Key = "123.***.***-00", Type = "CPF" });
    }

    [RelayCommand]
    private async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task AddAccountAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddBankAccount));
    }

    [RelayCommand]
    private async Task AddPixKeyAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AddPixKey));
    }

    [RelayCommand]
    private async Task DeletePixKeyAsync(PixKey key)
    {
        bool answer = await Shell.Current.DisplayAlertAsync("Confirmar", "Deseja excluir esta chave Pix?", "Sim", "Não");
        if (answer)
        {
            PixKeys.Remove(key);
        }
    }
}
