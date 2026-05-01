namespace CoreventApp.Views;

public partial class Register : ContentPage
{
    private int _currentStep = 1;
    private const int TotalSteps = 3;

    public Register()
    {
        InitializeComponent();
        DateNascimento.MaximumDate = DateTime.Today;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await UpdateUI();
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (_currentStep < TotalSteps)
        {
            _currentStep++;
            await UpdateUI();
        }
        else
        {
            await DisplayAlertAsync("Sucesso", "Cadastro realizado!", "OK");
        }
    }

    private async void OnHeaderBackClicked(object sender, EventArgs e)
    {
        if (_currentStep > 1)
        {
            _currentStep--;
            await UpdateUI();
            return;
        }

        await Navigation.PopAsync();
    }

    private async Task UpdateUI()
    {
        Step1.IsVisible = _currentStep == 1;
        Step2.IsVisible = _currentStep == 2;
        Step3.IsVisible = _currentStep == 3;

        BtnNext.Text = _currentStep == TotalSteps ? "Finalizar" : "Próximo";

        var progress = (double)_currentStep / TotalSteps;
        await MainProgressBar.ProgressTo(progress, 180, Easing.CubicOut);

        switch (_currentStep)
        {
            case 1:
                StepTitle.Text = "Conte-nos sobre você";
                StepDescription.Text = "Vamos começar com o básico para criar seu perfil.";
                break;
            case 2:
                StepTitle.Text = "Seu documento";
                StepDescription.Text = "Precisamos do seu CPF para identificar sua conta.";
                break;
            case 3:
                StepTitle.Text = "Credenciais de acesso";
                StepDescription.Text = "Informe seu e-mail e crie uma senha para entrar no app.";
                break;
        }

    }
}