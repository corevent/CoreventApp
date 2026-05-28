using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Models.Dtos;
using CoreventApp.Services;
using CoreventApp.Services.Api;
using CoreventApp.Views;

namespace CoreventApp.ViewModels;

[QueryProperty(nameof(EditingEventId), "EventId")]
public partial class CreateEventViewModel : ObservableObject
{
    private const int TotalSteps = 3;
    private readonly EventsService _eventsService;
    private readonly StatesApiClient _statesApi;
    private readonly PaymentInfoService _paymentInfoService;
    private string? _editingEventId;
    private EventDetailDto? _originalEvent;

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

    [ObservableProperty]
    public partial bool IsLoadingStates { get; set; }

    [ObservableProperty]
    public partial bool IsSaving { get; set; }

    public DateTime Today => DateTime.Today;
    public DateTime Tomorrow => DateTime.Today.AddDays(1);

    public List<string> Categories { get; } = new()
    {
        "Música", "Esportes", "Tecnologia", "Negócios", "Educação",
        "Arte e Cultura", "Gastronomia", "Saúde e Bem-estar",
        "Família e Crianças", "Religioso/Espiritual", "Jogos",
        "Comunidade/Social", "Moda e Beleza", "Outro"
    };

    public List<string> LocationTypes { get; } = new()
    {
        "Online", "Presencial", "Híbrido"
    };

    private static readonly Dictionary<string, string> CategoryToApi = new()
    {
        ["Música"] = "music",
        ["Esportes"] = "sports",
        ["Tecnologia"] = "tech",
        ["Negócios"] = "business",
        ["Educação"] = "education",
        ["Arte e Cultura"] = "art_culture",
        ["Gastronomia"] = "gastronomy",
        ["Saúde e Bem-estar"] = "health_wellness",
        ["Família e Crianças"] = "family_kids",
        ["Religioso/Espiritual"] = "religious_spiritual",
        ["Jogos"] = "games",
        ["Comunidade/Social"] = "community_social",
        ["Moda e Beleza"] = "fashion_beauty",
        ["Outro"] = "other"
    };

    private static readonly Dictionary<string, string> LocationTypeToApi = new()
    {
        ["Online"] = "online",
        ["Presencial"] = "in_person",
        ["Híbrido"] = "hybrid"
    };

    public CreateEventViewModel(EventsService eventsService, StatesApiClient statesApi, PaymentInfoService paymentInfoService)
    {
        _eventsService = eventsService;
        _statesApi = statesApi;
        _paymentInfoService = paymentInfoService;
    }

    public string? EditingEventId
    {
        set
        {
            if (value is null) return;
            _editingEventId = value;
            _ = LoadEditingEventAsync(value);
        }
    }

    private async Task LoadEditingEventAsync(string eventId)
    {
        var evt = await _eventsService.GetByIdAsync(eventId);
        if (evt is null) return;

        _originalEvent = evt;
        IsEditing = true;

        Form.Title = evt.Title;
        Form.Description = evt.Description ?? string.Empty;
        Form.MaxParticipants = evt.MaxParticipants;
        Form.StartDate = evt.StartDate;
        Form.EndDate = evt.EndDate;
        Form.IsOver18 = evt.IsAdultOnly;
        Form.ZipCode = evt.ZipCode ?? string.Empty;
        Form.Neighborhood = evt.Neighborhood ?? string.Empty;
        Form.Street = evt.Street ?? string.Empty;
        Form.Number = evt.Number?.ToString() ?? "0";
        Form.Complement = evt.Complement ?? string.Empty;
        Form.LocationName = evt.LocationName;
        Form.BannerUrl = evt.BannerUrl ?? string.Empty;

        var locationTypeDisplay = LocationTypeToApi
            .FirstOrDefault(x => x.Value == evt.LocationType).Key;
        if (locationTypeDisplay is not null)
            Form.LocationType = locationTypeDisplay;

        var categoryDisplay = CategoryToApi
            .FirstOrDefault(x => x.Value == evt.Category).Key;
        if (categoryDisplay is not null)
            Form.Category = categoryDisplay;

        // Determine editability
        var now = DateTime.Now;
        IsEditable = evt.Status switch
        {
            "draft" => true,
            "opened" => now <= evt.StartDate,
            "going" => false,
            "finished" => false,
            "canceled" => false,
            _ => true
        };

        bool isOpened = evt.Status == "opened";
        ShowPublishButton = IsEditable && !isOpened;
        ShowDraftButton = IsEditable && !isOpened;
        DraftButtonText = isOpened ? "Salvar Alterações" : "Salvar Rascunho";

        UpdateUI();

        // Load states and pre-select
        if (evt.CityId > 0)
        {
            await LoadStatesAsync();
            var matchedState = Form.States.FirstOrDefault(s => s.Uf == evt.StateAcronym);
            var stateIdx = matchedState is not null ? Form.States.IndexOf(matchedState) : -1;
            if (stateIdx >= 0)
                Form.SelectedStateIndex = stateIdx;
        }
    }

    [RelayCommand]
    private async Task LoadStatesAsync()
    {
        try
        {
            IsLoadingStates = true;
            var response = await _statesApi.GetStatesAsync();
            Form.States.Clear();
            foreach (var s in response.Data)
                Form.States.Add(s);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Load states failed: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível carregar a lista de estados.", "OK");
        }
        finally
        {
            IsLoadingStates = false;
        }
    }

    [RelayCommand]
    private async Task LoadCitiesAsync()
    {
        var state = Form.SelectedState;
        if (state is null) return;

        try
        {
            IsLoadingStates = true;
            var response = await _statesApi.GetCitiesAsync(state.Id);
            Form.Cities.Clear();
            foreach (var c in response.Data)
                Form.Cities.Add(c);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Load cities failed: {ex.Message}");
            await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível carregar a lista de cidades.", "OK");
        }
        finally
        {
            IsLoadingStates = false;
        }
    }

    [RelayCommand]
    private async Task NextAsync()
    {
        if (CurrentStep >= TotalSteps) return;

        if (!ValidateStep(CurrentStep)) return;

        CurrentStep++;
        UpdateUI();

        if (CurrentStep == 3 && Form.States.Count == 0)
            await LoadStatesAsync();
    }

    [RelayCommand]
    private async Task SaveDraftAsync()
    {
        if (!ValidateAll()) return;

        if (_originalEvent?.Status == "opened")
        {
            await Shell.Current.DisplayAlertAsync("Operação Inválida",
                "Um evento publicado não pode voltar a ser rascunho.", "OK");
            return;
        }

        IsSaving = true;
        try
        {
            var dto = BuildCreateDto("draft");

            if (IsEditing && _editingEventId is not null)
            {
                var updateDto = BuildUpdateDto(dto, "draft");
                var result = await _eventsService.UpdateAsync(_editingEventId, updateDto);
                if (result is null)
                {
                    await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível salvar o rascunho.", "OK");
                    return;
                }
                await Shell.Current.DisplayAlertAsync("Rascunho Atualizado",
                    "As alterações foram salvas como rascunho.", "OK");
            }
            else
            {
                var result = await _eventsService.CreateAsync(dto);
                if (result is null)
                {
                    await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível salvar o rascunho.", "OK");
                    return;
                }
                await Shell.Current.DisplayAlertAsync("Rascunho Salvo",
                    "Seu evento foi salvo como rascunho.", "OK");
            }

            await Shell.Current.GoToAsync("../..");
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private async Task PublishEventAsync()
    {
        if (!ValidateAll()) return;

        if (_originalEvent?.Status == "opened")
        {
            await Shell.Current.DisplayAlertAsync("Já Publicado",
                "Este evento já está publicado. Salve as alterações como rascunho.", "OK");
            return;
        }

        // Check if organizer has payment info registered
        var paymentInfos = await _paymentInfoService.GetAllAsync();
        if (paymentInfos.Count == 0)
        {
            var goToConfig = await Shell.Current.DisplayAlertAsync(
                "Dados de Repasse Necessários",
                "Você precisa cadastrar seus dados de repasse antes de publicar um evento. Deseja configurar agora?",
                "Configurar Agora", "Cancelar");
            if (goToConfig)
                await Shell.Current.GoToAsync(nameof(Views.TransferSettings));
            return;
        }

        IsSaving = true;
        try
        {
            var dto = BuildCreateDto("opened");

            if (IsEditing && _editingEventId is not null)
            {
                var updateDto = BuildUpdateDto(dto, "opened");
                var result = await _eventsService.UpdateAsync(_editingEventId, updateDto);
                if (result is null)
                {
                    await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível publicar o evento.", "OK");
                    return;
                }
                await Shell.Current.DisplayAlertAsync("Evento Atualizado",
                    "Suas alterações foram publicadas.", "OK");
            }
            else
            {
                var result = await _eventsService.CreateAsync(dto);
                if (result is null)
                {
                    await Shell.Current.DisplayAlertAsync("Erro", "Não foi possível publicar o evento.", "OK");
                    return;
                }
                await Shell.Current.DisplayAlertAsync("Evento Publicado",
                    "Seu evento foi publicado com sucesso!", "OK");
            }

            await Shell.Current.GoToAsync("../..");
        }
        finally
        {
            IsSaving = false;
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

    private bool ValidateStep(int step)
    {
        return step switch
        {
            1 => !string.IsNullOrWhiteSpace(Form.Title),
            2 => Form.StartDate != default &&
                 (IsEditing || Form.StartDate > DateTime.Now) &&
                 Form.EndDate >= Form.StartDate,
            _ => true
        };
    }

    private bool ValidateAll()
    {
        var valid =
            !string.IsNullOrWhiteSpace(Form.Title) &&
            Form.StartDate != default &&
            Form.EndDate != default &&
            Form.EndDate >= Form.StartDate;

        if (!IsEditing && Form.StartDate <= DateTime.Now)
            valid = false;

        if (Form.LocationType != "Online")
        {
            valid = valid &&
                !string.IsNullOrWhiteSpace(Form.Street) &&
                Form.Number.Length > 0 && int.TryParse(Form.Number, out int num) && num > 0 &&
                !string.IsNullOrWhiteSpace(Form.Neighborhood) &&
                !string.IsNullOrWhiteSpace(Form.ZipCode) &&
                Form.SelectedCityId > 0;
        }

        return valid;
    }

    private CreateEventDto BuildCreateDto(string status)
    {
        var title = Form.Title.Trim();
        var category = CategoryToApi.GetValueOrDefault(Form.Category, "other");
        var locationType = LocationTypeToApi.GetValueOrDefault(Form.LocationType, "in_person");
        var bannerUrl = string.IsNullOrWhiteSpace(Form.BannerUrl)
            ? $"https://placehold.co/600x400/FF5722/FFFFFF?text={Uri.EscapeDataString(title)}"
            : Form.BannerUrl;

        var isOnline = locationType == "online";

        int number = 0;
        int.TryParse(Form.Number, out number);

        return new CreateEventDto(
            Title: title,
            Description: Form.Description ?? string.Empty,
            MaxParticipants: Form.MaxParticipants,
            LocationType: locationType,
            LocationName: isOnline ? null : (Form.LocationName ?? string.Empty),
            CityId: isOnline ? null : Form.SelectedCityId,
            ZipCode: isOnline ? null : (Form.ZipCode ?? string.Empty),
            Neighborhood: isOnline ? null : (Form.Neighborhood ?? string.Empty),
            Street: isOnline ? null : (Form.Street ?? string.Empty),
            Number: isOnline ? null : number,
            Complement: string.IsNullOrWhiteSpace(Form.Complement) ? null : Form.Complement,
            StartDate: Form.StartDate,
            EndDate: Form.EndDate,
            Category: category,
            BannerUrl: bannerUrl,
            IsAdultOnly: Form.IsOver18,
            Status: status);
    }

    private static UpdateEventDto BuildUpdateDto(CreateEventDto source, string? status)
    {
        return new UpdateEventDto(
            Title: source.Title,
            Description: source.Description,
            MaxParticipants: source.MaxParticipants,
            LocationType: source.LocationType,
            LocationName: source.LocationName,
            CityId: source.CityId,
            ZipCode: source.ZipCode,
            Neighborhood: source.Neighborhood,
            Street: source.Street,
            Number: source.Number,
            Complement: source.Complement,
            StartDate: source.StartDate,
            EndDate: source.EndDate,
            Category: source.Category,
            BannerUrl: source.BannerUrl,
            IsAdultOnly: source.IsAdultOnly,
            Status: status);
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
    public partial string BannerUrl { get; set; } = string.Empty;

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

    // State / City cascading pickers
    public ObservableCollection<StateDataDto> States { get; } = new();
    public ObservableCollection<CityDataDto> Cities { get; } = new();

    [ObservableProperty]
    public partial int SelectedStateIndex { get; set; } = -1;

    [ObservableProperty]
    public partial int SelectedCityIndex { get; set; } = -1;

    public StateDataDto? SelectedState =>
        SelectedStateIndex >= 0 && SelectedStateIndex < States.Count
            ? States[SelectedStateIndex]
            : null;

    public int SelectedCityId
    {
        get
        {
            if (SelectedCityIndex >= 0 && SelectedCityIndex < Cities.Count)
                return Cities[SelectedCityIndex].Id;
            return 0;
        }
    }

    partial void OnSelectedStateIndexChanged(int value)
    {
        OnPropertyChanged(nameof(SelectedState));
        OnPropertyChanged(nameof(AddressError));
    }

    partial void OnSelectedCityIndexChanged(int value)
    {
        OnPropertyChanged(nameof(SelectedCityId));
        OnPropertyChanged(nameof(AddressError));
    }

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

    public string? StartDateError
    {
        get
        {
            if (StartDate == default) return "A data de início é obrigatória.";
            if (StartDate <= DateTime.Now) return "A data de início deve ser no futuro.";
            return null;
        }
    }

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
            if (string.IsNullOrWhiteSpace(Street)) return "A rua é obrigatória.";
            if (string.IsNullOrWhiteSpace(Number) || !int.TryParse(Number, out int n) || n <= 0)
                return "Informe um número válido.";
            if (string.IsNullOrWhiteSpace(Neighborhood)) return "O bairro é obrigatório.";
            if (string.IsNullOrWhiteSpace(ZipCode)) return "O CEP é obrigatório.";
            if (SelectedCityId <= 0) return "Selecione uma cidade.";
            return null;
        }
    }

    partial void OnTitleChanged(string value) => OnPropertyChanged(nameof(TitleError));
    partial void OnStartDateChanged(DateTime value) { OnPropertyChanged(nameof(StartDateError)); OnPropertyChanged(nameof(EndDateError)); }
    partial void OnEndDateChanged(DateTime value) => OnPropertyChanged(nameof(EndDateError));
    partial void OnStreetChanged(string value) => OnPropertyChanged(nameof(AddressError));
    partial void OnNumberChanged(string value) => OnPropertyChanged(nameof(AddressError));
    partial void OnNeighborhoodChanged(string value) => OnPropertyChanged(nameof(AddressError));
    partial void OnZipCodeChanged(string value) => OnPropertyChanged(nameof(AddressError));
}
