using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Helpers;
using CoreventApp.Services;

namespace CoreventApp.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
  private readonly IAuthService _authService;
  private const int TotalSteps = 4;

  public RegisterViewModel(IAuthService authService)
  {
    _authService = authService;
  }

  [ObservableProperty]
  public partial bool IsBusy { get; set; }

  [ObservableProperty]
  public partial int CurrentStep { get; set; } = 1;

  [ObservableProperty]
  public partial double Progress { get; set; } = 0.25;

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
    if (IsBusy) return;

    if (CurrentStep < TotalSteps)
    {
      if (!ValidateStep(CurrentStep)) return;
      CurrentStep++;
      UpdateUI();
    }
    else
    {
      if (!ValidateAll()) return;

      IsBusy = true;

      await _authService.SendVerificationEmailAsync(Form.Email);

      IsBusy = false;

      var document = Form.AccountType == "pj"
        ? System.Text.RegularExpressions.Regex.Replace(Form.Cnpj, @"\D", "")
        : System.Text.RegularExpressions.Regex.Replace(Form.Cpf, @"\D", "");
      var documentType = Form.AccountType == "pj" ? "cnpj" : "cpf";

      await Shell.Current.GoToAsync(
        $"EmailVerification?Name={Uri.EscapeDataString(Form.Nome)}" +
        $"&Email={Uri.EscapeDataString(Form.Email)}" +
        $"&Password={Uri.EscapeDataString(Form.Senha)}" +
        $"&Document={Uri.EscapeDataString(document)}" +
        $"&DocumentType={Uri.EscapeDataString(documentType)}" +
        $"&BirthDate={Uri.EscapeDataString(Form.DataNascimento.ToString("yyyy-MM-dd"))}");
    }
  }

  [RelayCommand]
  private async Task SelectAccountTypeAsync(string type)
  {
    if (Form.AccountType == type) return;
    Form.AccountType = type;
    Form.Cpf = string.Empty;
    Form.Cnpj = string.Empty;
    await NextAsync();
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
      1 => !string.IsNullOrWhiteSpace(Form.Nome) && Form.Nome.Trim().Length >= 3,
      2 => true,
      3 => ValidateDocument(),
      4 => ValidateCredentials(),
      _ => true
    };
  }

  private bool ValidateDocument()
  {
    var doc = Form.AccountType == "pj" ? Form.Cnpj : Form.Cpf;
    if (Form.AccountType == "pj")
    {
      if (!ValidationHelper.IsValidCnpj(doc))
      {
        Shell.Current.DisplayAlertAsync("Erro", "CNPJ inválido. Informe um CNPJ com 14 dígitos.", "OK");
        return false;
      }
    }
    else
    {
      if (!ValidationHelper.IsValidCpf(doc))
      {
        Shell.Current.DisplayAlertAsync("Erro", "CPF inválido. Informe um CPF com 11 dígitos.", "OK");
        return false;
      }
    }
    return true;
  }

  private bool ValidateCredentials()
  {
    if (!ValidationHelper.IsValidEmail(Form.Email))
    {
      Shell.Current.DisplayAlertAsync("Erro", "Informe um e-mail válido.", "OK");
      return false;
    }
    if (!ValidationHelper.IsValidPassword(Form.Senha))
    {
      Shell.Current.DisplayAlertAsync("Erro", "A senha deve ter 8+ caracteres, com maiúscula, minúscula, número e símbolo.", "OK");
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
    if (string.IsNullOrWhiteSpace(Form.Nome) || Form.Nome.Trim().Length < 3)
    {
      Shell.Current.DisplayAlertAsync("Erro", "O nome deve ter pelo menos 3 caracteres.", "OK");
      return false;
    }
    if (!ValidateDocument())
      return false;
    return ValidateCredentials();
  }

  private void UpdateUI()
  {
    Progress = (double)CurrentStep / TotalSteps;
    ButtonNextText = CurrentStep == TotalSteps ? "Finalizar" : "Próximo";

    StepTitle = CurrentStep switch
    {
      1 => "Conte-nos sobre você",
      2 => "Pessoa ou Empresa?",
      3 => "Seu documento",
      4 => "Credenciais de acesso",
      _ => string.Empty
    };

    StepDescription = CurrentStep switch
    {
      1 => "Vamos começar com o básico para criar seu perfil.",
      2 => "Você é pessoa física ou jurídica?",
      3 => "Informe o documento da sua conta.",
      4 => "Informe seu e-mail e crie uma senha para entrar no app.",
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
  public partial string Cnpj { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string Senha { get; set; } = string.Empty;

  [ObservableProperty]
  public partial string ConfirmarSenha { get; set; } = string.Empty;

  [ObservableProperty]
  public partial DateTime DataNascimento { get; set; } = new(2000, 1, 1);

  [ObservableProperty]
  [NotifyPropertyChangedFor(nameof(IsPessoaFisica))]
  [NotifyPropertyChangedFor(nameof(IsPessoaJuridica))]
  public partial string AccountType { get; set; } = "pf";

  public bool IsPessoaFisica => AccountType == "pf";
  public bool IsPessoaJuridica => AccountType == "pj";
}