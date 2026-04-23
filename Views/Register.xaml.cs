namespace CoreventApp.Views;

public partial class Register : ContentPage
{
    private int _currentStep = 1;
    private const int TotalSteps = 3;

    public Register()
    {
        InitializeComponent();
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

    private async void OnBackClicked(object sender, EventArgs e)
    {
        if (_currentStep > 1)
        {
            _currentStep--;
            await UpdateUI();
        }
    }

    private async Task UpdateUI()
    {
        // 1. Atualiza Visibilidade das Etapas
        Step1.IsVisible = _currentStep == 1;
        Step2.IsVisible = _currentStep == 2;
        Step3.IsVisible = _currentStep == 3;

        // 2. Atualiza Botões (Voltar e Texto do Próximo)
        BtnBack.IsVisible = _currentStep > 1;
        BtnNext.Text = _currentStep == TotalSteps ? "Finalizar" : "Próximo";

        // 3. Atualiza Títulos e Descrições
        switch (_currentStep)
        {
            case 1:
                StepTitle.Text = "Conte-nos sobre você";
                StepDescription.Text = "Vamos começar com o básico para criar o seu perfil.";
                break;
            case 2:
                StepTitle.Text = "Segurança";
                StepDescription.Text = "Crie uma senha segura.";
                break;
            case 3:
                StepTitle.Text = "Termos de Uso";
                StepDescription.Text = "Por favor, leia e aceite os termos de uso.";
                break;
        }

        double progress = (double)_currentStep / TotalSteps;
        await MainProgressBar.ProgressTo(progress, 300, Easing.CubicOut);
    }
}