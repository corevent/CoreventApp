using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class AddPixKeyViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string SelectedKeyType { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string KeyValue { get; set; } = string.Empty;

    public ObservableCollection<string> KeyTypes { get; } = new()
    {
        "Email",
        "CPF",
        "CNPJ",
        "Telefone",
        "Chave Aleatória"
    };

    public AddPixKeyViewModel()
    {
        SelectedKeyType = KeyTypes[0];
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(KeyValue))
        {
            await Shell.Current.DisplayAlertAsync("Erro", "Por favor, informe a chave Pix.", "OK");
            return;
        }

        // Basic validation based on selected type
        bool isValid = true;
        string errorMessage = "";

        switch (SelectedKeyType)
        {
            case "Email":
                if (!KeyValue.Contains("@") || !KeyValue.Contains("."))
                {
                    isValid = false;
                    errorMessage = "E-mail inválido.";
                }
                break;
            case "CPF":
                if (KeyValue.Length < 11)
                {
                    isValid = false;
                    errorMessage = "CPF deve ter pelo menos 11 dígitos.";
                }
                break;
            case "Telefone":
                if (KeyValue.Length < 10)
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

        // Logic to save would go here
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
