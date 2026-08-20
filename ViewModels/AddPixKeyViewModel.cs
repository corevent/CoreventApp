using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class AddPixKeyViewModel : ObservableObject
{
    private readonly PaymentInfoService _paymentInfoService;

    private static readonly Dictionary<string, string> UiToApiPixType = new()
    {
        ["Email"] = "email",
        ["CPF"] = "cpf",
        ["CNPJ"] = "cnpj",
        ["Telefone"] = "phone",
        ["Chave Aleatória"] = "random"
    };

    public AddPixKeyViewModel(PaymentInfoService paymentInfoService)
    {
        _paymentInfoService = paymentInfoService;
        SelectedKeyType = KeyTypes[0];
    }

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string SelectedKeyType { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string KeyValue { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    public ObservableCollection<string> KeyTypes { get; } = new()
    {
        "Email",
        "CPF",
        "CNPJ",
        "Telefone",
        "Chave Aleatória"
    };

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Description))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "A descrição é obrigatória.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(KeyValue))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "Por favor, informe a chave Pix.", "OK");
            return;
        }

        bool isValid = true;
        string errorMessage = "";

        switch (SelectedKeyType)
        {
            case "Email":
                if (!ValidationHelper.IsValidEmail(KeyValue))
                {
                    isValid = false;
                    errorMessage = "E-mail inválido.";
                }
                break;
            case "CPF":
                if (!ValidationHelper.IsValidCpf(KeyValue))
                {
                    isValid = false;
                    errorMessage = "CPF inválido.";
                }
                break;
            case "Telefone":
                if (!ValidationHelper.IsValidPhone(KeyValue))
                {
                    isValid = false;
                    errorMessage = "Telefone inválido.";
                }
                break;
        }

        if (!isValid)
        {
            await Shell.Current.DisplayAlertAsync("Erro", errorMessage, "OK");
            return;
        }

        var pixType = UiToApiPixType.GetValueOrDefault(SelectedKeyType);

        var dto = new CreateOrganizerPaymentInfoDto(
            Description,
            null,
            null,
            null,
            null,
            KeyValue,
            pixType,
            null);

        IsLoading = true;
        var result = await _paymentInfoService.CreateAsync(dto);
        IsLoading = false;

        if (result != null)
            await Shell.Current.GoToAsync("..");
        else
            await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível salvar a chave Pix. Tente novamente.", "OK");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
