using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
  private readonly IAuthService _authService;
  private const int TotalSteps = 3;

  public RegisterViewModel(IAuthService authService)
  {
    _authService = authService;
  }

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
      if (!ValidateStep(CurrentStep)) return;
      CurrentStep++;
      UpdateUI();
    }
    else
    {
      if (!ValidateAll()) return;

      await _authService.RegisterUserAsync(
        Form.Nome,
        Form.Email,
        Form.Senha,
        Form.Cpf,
        Form.DataNascimento.ToString("dd/MM/yyyy"));

      await Shell.Current.GoToAsync(
        $"EmailVerification?Email={Uri.EscapeDataString(Form.Email)}&Password={Uri.EscapeDataString(Form.Senha)}");
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

  private bool ValidateStep(int step)
  {
    return step switch
    {
      1 => !string.IsNullOrWhiteSpace(Form.Nome),
      2 => !string.IsNullOrWhiteSpace(Form.Cpf),
      3 => ValidateStep3(),
      _ => true
    };
  }

  private bool ValidateStep3()
  {
    if (string.IsNullOrWhiteSpace(Form.Email))
    {
      Shell.Current.DisplayAlertAsync("Erro", "Informe seu e-mail.", "OK");
      return false;
    }
    if (string.IsNullOrWhiteSpace(Form.Senha) || Form.Senha.Length < 6)
    {
      Shell.Current.DisplayAlertAsync("Erro", "A senha deve ter pelo menos 6 caracteres.", "OK");
      return false;
    }
    if (Form.Senha != Form.ConfirmarSenha)
    {
      Shell.Current.DisplayAlertAsync("Erro", "As senhas não conferem.", "OK");
      return false;
    }
    return true;
  }

  private bool ValidateAll()
  {
    if (string.IsNullOrWhiteSpace(Form.Nome))
    {
      Shell.Current.DisplayAlertAsync("Erro", "Preencha seu nome.", "OK");
      return false;
    }
    if (string.IsNullOrWhiteSpace(Form.Cpf))
    {
      Shell.Current.DisplayAlertAsync("Erro", "Preencha seu CPF.", "OK");
      return false;
    }
    return ValidateStep3();
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