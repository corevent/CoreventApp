using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
  private const int TotalSteps = 3;

  [ObservableProperty]
  public partial int CurrentStep { get; set; } = 1;

  [ObservableProperty]
  public partial double Progress { get; set; } = 0.33;

  [ObservableProperty]
  public partial string StepTitle { get; set; } = "Conte-nos sobre você";

  [ObservableProperty]
  public partial string StepDescription { get; set; } = "Vamos começar com o básico para criar seu perfil.";

  [ObservableProperty]
  public partial string ButtonNextText { get; set; } = "Próximo";

  [ObservableProperty]
  public partial RegisterRequest Form { get; set; } = new();

  [ObservableProperty]
  public partial DateTime DataNascimentoMax { get; set; } = DateTime.Today;


  [RelayCommand]
  private async Task NextAsync()
  {
    if (CurrentStep < TotalSteps)
    {
      CurrentStep++;
      UpdateUI();
    }
    else
    {
      await Shell.Current.DisplayAlertAsync("Sucesso", "Cadastro realizado!", "OK");
    }
  }

  [RelayCommand]
  private async Task HeaderBackAsync()
  {
    if (CurrentStep > 1)
    {
      CurrentStep--;
      UpdateUI();
      return;
    }

    await Shell.Current.GoToAsync("..");
  }

  private void UpdateUI()
  {
    Progress = (double)CurrentStep / TotalSteps;
    ButtonNextText = CurrentStep == TotalSteps ? "Finalizar" : "Próximo";

    StepTitle = CurrentStep switch
    {
      1 => "Conte-nos sobre você",
      2 => "Seu documento",
      3 => "Credenciais de acesso",
      _ => string.Empty
    };

    StepDescription = CurrentStep switch
    {
      1 => "Vamos começar com o básico para criar seu perfil.",
      2 => "Precisamos do seu CPF para identificar sua conta.",
      3 => "Informe seu e-mail e crie uma senha para entrar no app.",
      _ => string.Empty
    };
  }
}

public partial class RegisterRequest : ObservableObject
{
  [ObservableProperty]
  public partial string Nome { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Email { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Cpf { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Senha { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string ConfirmarSenha { get; set; } = string.Empty;

  [ObservableProperty]
  public partial DateTime DataNascimento { get; set; } = new(2000, 1, 1);
}