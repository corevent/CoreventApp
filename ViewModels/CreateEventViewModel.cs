using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

public partial class CreateEventViewModel : ObservableObject
{
    private const int TotalSteps = 3;

    [ObservableProperty]
    public partial int CurrentStep { get; set; } = 1;

    [ObservableProperty]
    public partial double Progress { get; set; } = 0.33;

    [ObservableProperty]
    public partial string StepTitle { get; set; } = "Informações Básicas";

    [ObservableProperty]
    public partial string StepDescription { get; set; } = "Conte os detalhes principais do seu evento.";

    [ObservableProperty]
    public partial string ButtonNextText { get; set; } = "Próximo";

    [ObservableProperty]
    public partial CreateEventRequest Form { get; set; } = new();

    public DateTime Today => DateTime.Today;

    public List<string> Categories { get; } = new()
    {
        "Festival", "Show", "Teatro", "Esporte", "Corporativo", "Workshop", "Outro"
    };

    public List<string> LocationTypes { get; } = new()
    {
        "Online", "Presencial", "Híbrido"
    };

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
            await Shell.Current.DisplayAlertAsync("Sucesso", "Evento criado com sucesso!", "OK");
        }
    }

    [RelayCommand]
    private async Task BackAsync()
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
        ButtonNextText = CurrentStep == TotalSteps ? "Criar Evento" : "Próximo";

        StepTitle = CurrentStep switch
        {
            1 => "Informações Básicas",
            2 => "Data e Participação",
            3 => "Localização",
            _ => string.Empty
        };

        StepDescription = CurrentStep switch
        {
            1 => "Conte os detalhes principais do seu evento.",
            2 => "Defina quando será e quantos participantes poderão participar.",
            3 => "Onde o evento vai acontecer?",
            _ => string.Empty
        };
    }
}

public partial class CreateEventRequest : ObservableObject
{
    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Category { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsOver18 { get; set; }

    [ObservableProperty]
    public partial DateTime StartDate { get; set; } = DateTime.Today.AddDays(30);

    [ObservableProperty]
    public partial DateTime EndDate { get; set; } = DateTime.Today.AddDays(31);

    [ObservableProperty]
    public partial int MaxParticipants { get; set; } = 100;

    [ObservableProperty]
    public partial string LocationType { get; set; } = "Presencial";

    [ObservableProperty]
    public partial string LocationName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string City { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ZipCode { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Neighborhood { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Street { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Number { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Complement { get; set; } = string.Empty;

    partial void OnLocationTypeChanged(string value)
    {
        OnPropertyChanged(nameof(IsPhysicalLocationVisible));
    }

    public bool IsPhysicalLocationVisible => LocationType != "Online";
}
