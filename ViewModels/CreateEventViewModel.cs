using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EditingEvent), "EventData")]
public partial class CreateEventViewModel : ObservableObject
{
    private const int TotalSteps = 3;
    private EventSummary? _originalEvent;

    [ObservableProperty]
    public partial int CurrentStep { get; set; } = 1;

    [ObservableProperty]
    public partial double Progress { get; set; } = 0.33;

    [ObservableProperty]
    public partial string PageTitle { get; set; } = "Criar Evento";

    [ObservableProperty]
    public partial string StepTitle { get; set; } = "Informações Básicas";

    [ObservableProperty]
    public partial string StepDescription { get; set; } = "Conte os detalhes principais do seu evento.";

    [ObservableProperty]
    public partial string ButtonNextText { get; set; } = "Próximo";

    [ObservableProperty]
    public partial CreateEventRequest Form { get; set; } = new();

    [ObservableProperty]
    public partial bool IsEditing { get; set; }

    [ObservableProperty]
    public partial bool IsEditable { get; set; } = true;

    [ObservableProperty]
    public partial bool ShowDraftButton { get; set; } = true;

    [ObservableProperty]
    public partial bool ShowPublishButton { get; set; } = true;

    [ObservableProperty]
    public partial string DraftButtonText { get; set; } = "Salvar Rascunho";

    public DateTime Today => DateTime.Today;

    public List<string> Categories { get; } = new()
    {
        "Festival", "Show", "Teatro", "Esporte", "Corporativo", "Workshop", "Outro"
    };

    public List<string> LocationTypes { get; } = new()
    {
        "Online", "Presencial", "Híbrido"
    };

    public EventSummary? EditingEvent
    {
        set
        {
            if (value is null) return;

            _originalEvent = value;
            IsEditing = true;
            Form.Id = value.Name; // Simulated unique identifier
            Form.Title = value.Name;
            Form.StartDate = value.StartDate;
            Form.EndDate = value.EndDate;

            // Determine editability
            var now = DateTime.Now;
            IsEditable = value.Status switch
            {
                EventStatus.Draft => true,
                EventStatus.Opened => now <= value.StartDate,
                EventStatus.Going => false,
                EventStatus.Finished => false,
                EventStatus.Canceled => false,
                _ => true
            };

            // Button visibility for step 3
            bool isOpened = value.Status == EventStatus.Opened;
            ShowPublishButton = IsEditable && !isOpened;
            ShowDraftButton = IsEditable && !isOpened;
            DraftButtonText = isOpened ? "Salvar Alterações" : "Salvar Rascunho";

            UpdateUI();
        }
    }

    [RelayCommand]
    private async Task NextAsync()
    {
        if (CurrentStep >= TotalSteps) return;

        if (!ValidateStep(CurrentStep)) return;

        CurrentStep++;
        UpdateUI();
    }

    [RelayCommand]
    private async Task SaveDraftAsync()
    {
        if (!ValidateAll()) return;

        // Block Opened → Draft transition for published events
        if (_originalEvent?.Status == EventStatus.Opened)
        {
            await Shell.Current.DisplayAlertAsync("Operação Inválida",
                "Um evento publicado não pode voltar a ser rascunho.", "OK");
            return;
        }

        if (IsEditing)
        {
            await Shell.Current.DisplayAlertAsync("Rascunho Atualizado",
                "As alterações foram salvas como rascunho.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Rascunho Salvo",
                "Seu evento foi salvo como rascunho.", "OK");
        }

        await Shell.Current.GoToAsync("../..");
    }

    [RelayCommand]
    private async Task PublishEventAsync()
    {
        if (!ValidateAll()) return;

        if (_originalEvent?.Status == EventStatus.Opened)
        {
            await Shell.Current.DisplayAlertAsync("Já Publicado",
                "Este evento já está publicado. Salve as alterações como rascunho.", "OK");
            return;
        }

        if (IsEditing)
        {
            await Shell.Current.DisplayAlertAsync("Evento Atualizado",
                "Suas alterações foram publicadas.", "OK");
        }
        else
        {
            await Shell.Current.DisplayAlertAsync("Evento Publicado",
                "Seu evento foi publicado com sucesso!", "OK");
        }

        await Shell.Current.GoToAsync("../..");
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

    private bool ValidateStep(int step)
    {
        return step switch
        {
            1 => !string.IsNullOrWhiteSpace(Form.Title),
            2 => Form.EndDate >= Form.StartDate && Form.StartDate != default,
            _ => true
        };
    }

    private bool ValidateAll()
    {
        return
            !string.IsNullOrWhiteSpace(Form.Title) &&
            Form.StartDate != default &&
            Form.EndDate != default &&
            Form.EndDate >= Form.StartDate &&
            (Form.LocationType == "Online" ||
             (!string.IsNullOrWhiteSpace(Form.Street) &&
              !string.IsNullOrWhiteSpace(Form.Number) &&
              !string.IsNullOrWhiteSpace(Form.City)));
    }

    private void UpdateUI()
    {
        Progress = (double)CurrentStep / TotalSteps;
        PageTitle = IsEditing ? "Editar Evento" : "Criar Evento";

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
    public partial string Id { get; set; } = string.Empty;

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
        OnPropertyChanged(nameof(TitleError));
        OnPropertyChanged(nameof(StartDateError));
        OnPropertyChanged(nameof(EndDateError));
        OnPropertyChanged(nameof(AddressError));
    }

    public bool IsPhysicalLocationVisible => LocationType != "Online";

    public string? TitleError => string.IsNullOrWhiteSpace(Title) ? "O título é obrigatório." : null;

    public string? StartDateError => StartDate == default ? "A data de início é obrigatória." : null;

    public string? EndDateError
    {
        get
        {
            if (EndDate == default) return "A data de término é obrigatória.";
            if (EndDate < StartDate) return "A data de término não pode ser anterior à data de início.";
            return null;
        }
    }

    public string? AddressError
    {
        get
        {
            if (LocationType == "Online") return null;
            if (string.IsNullOrWhiteSpace(Street)) return "O endereço (rua) é obrigatório para eventos presenciais ou híbridos.";
            if (string.IsNullOrWhiteSpace(Number)) return "O número é obrigatório para eventos presenciais ou híbridos.";
            if (string.IsNullOrWhiteSpace(City)) return "A cidade é obrigatória para eventos presenciais ou híbridos.";
            return null;
        }
    }

    partial void OnTitleChanged(string value) => OnPropertyChanged(nameof(TitleError));
    partial void OnStartDateChanged(DateTime value) { OnPropertyChanged(nameof(StartDateError)); OnPropertyChanged(nameof(EndDateError)); }
    partial void OnEndDateChanged(DateTime value) => OnPropertyChanged(nameof(EndDateError));
    partial void OnStreetChanged(string value) => OnPropertyChanged(nameof(AddressError));
    partial void OnNumberChanged(string value) => OnPropertyChanged(nameof(AddressError));
    partial void OnCityChanged(string value) => OnPropertyChanged(nameof(AddressError));
}
