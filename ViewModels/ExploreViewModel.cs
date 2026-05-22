using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoreventApp.ViewModels;

public partial class CategoryItem : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsSelected { get; set; }
}

public partial class ExploreViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string SearchText { get; set; } = string.Empty;

    public ObservableCollection<CategoryItem> Categories { get; } = new();
    public ObservableCollection<EventSummary> Events { get; } = new();

    private List<EventSummary> _allEvents = new();

    public ExploreViewModel()
    {
        Categories.Add(new CategoryItem { Name = "Todos", IsSelected = true });
        Categories.Add(new CategoryItem { Name = "Música" });
        Categories.Add(new CategoryItem { Name = "Tecnologia" });
        Categories.Add(new CategoryItem { Name = "Gastronomia" });
        Categories.Add(new CategoryItem { Name = "Arte" });
        Categories.Add(new CategoryItem { Name = "Esporte" });

        LoadMockData();
    }

    partial void OnSearchTextChanged(string value)
    {
        FilterEvents();
    }

    private void FilterEvents()
    {
        var selected = Categories.FirstOrDefault(c => c.IsSelected);
        var categoryName = selected?.Name ?? "Todos";

        var filtered = _allEvents.AsEnumerable();

        if (categoryName != "Todos")
            filtered = filtered.Where(e => e.Category == categoryName);

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLower();
            filtered = filtered.Where(e =>
                e.Name.ToLower().Contains(search) ||
                e.Location.ToLower().Contains(search));
        }

        Events.Clear();
        foreach (var item in filtered)
            Events.Add(item);
    }

    [RelayCommand]
    private async Task OpenFilterAsync()
    {
        await Shell.Current.DisplayAlertAsync("Filtrar", "Opções de filtro em breve.", "OK");
    }

    [RelayCommand]
    private void SelectCategory(CategoryItem category)
    {
        foreach (var c in Categories)
            c.IsSelected = c == category;
        FilterEvents();
    }

    [RelayCommand]
    private void ToggleFavorite(EventSummary eventItem)
    {
        eventItem.IsFavorite = !eventItem.IsFavorite;
    }

    [RelayCommand]
    private async Task SelectEventAsync(EventSummary eventItem)
    {
        await Shell.Current.GoToAsync(nameof(Views.EventDetail), new Dictionary<string, object>
        {
            ["EventData"] = eventItem
        });
    }

    private void LoadMockData()
    {
        _allEvents = new List<EventSummary>
        {
            new() { Name = "Festival de Verão 2026", Category = "Música", Date = "15 Out, 2026", Location = "Praia de Copacabana, RJ", ImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=400&h=400&fit=crop", Price = "R$ 150", Rating = 4.8, Type = EventType.Presencial, OrganizerName = "Corevent Produções", OrganizerAvatar = "profile_default_icon.png", Description = "O maior festival de verão do Brasil! Venha curtir os melhores artistas em um evento inesquecível à beira-mar. São mais de 20 atrações nacionais e internacionais confirmadas." },
            new() { Name = "Tech Summit 2026", Category = "Tecnologia", Date = "22 Nov, 2026", Location = "Centro de Convenções, SP", ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=400&h=400&fit=crop", Price = "R$ 90", Rating = 4.9, Type = EventType.Hibrido, OnlineUrl = "https://zoom.us/techsummit2026", OrganizerName = "Tech Events Brasil", OrganizerAvatar = "profile_default_icon.png", Description = "O maior encontro de tecnologia da América Latina. Palestras, workshops e networking com os maiores nomes do setor. Disponível presencial e online." },
            new() { Name = "Workshop de Fotografia", Category = "Arte", Date = "5 Dez, 2026", Location = "Online", ImageUrl = "https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=400&h=400&fit=crop", Price = "Grátis", Rating = 4.5, Type = EventType.Remoto, OnlineUrl = "https://meet.google.com/fotografia", OrganizerName = "Instituto de Artes", OrganizerAvatar = "profile_default_icon.png", Description = "Aprenda técnicas avançadas de fotografia digital com profissionais renomados. Curso gratuito e ao vivo com certificado de participação." },
            new() { Name = "Feira Gastronômica", Category = "Gastronomia", Date = "12 Jan, 2027", Location = "Parque Ibirapuera, SP", ImageUrl = "https://images.unsplash.com/photo-1529193591184-b1d58069ecdd?w=400&h=400&fit=crop", Price = "R$ 45", Rating = 4.7, Type = EventType.Presencial, OrganizerName = "Gastro Experience", OrganizerAvatar = "profile_default_icon.png", Description = "Uma jornada gastronômica com os melhores chefs da cidade. Degustações, aulas show e muito sabor em um só lugar." },
            new() { Name = "Hackathon de Inovação", Category = "Tecnologia", Date = "5 Jan, 2027", Location = "Online", ImageUrl = "https://images.unsplash.com/photo-1504384308090-c894fdcc538d?w=400&h=400&fit=crop", Price = "Grátis", Rating = 4.6, Type = EventType.Remoto, OnlineUrl = "https://discord.gg/hackathon", OrganizerName = "InovaLab", OrganizerAvatar = "profile_default_icon.png", Description = "48 horas para criar soluções inovadoras para desafios reais. Equipes premiadas ganham mentoria e investimento." },
            new() { Name = "Concerto de Primavera", Category = "Música", Date = "20 Set, 2026", Location = "Teatro Municipal, RJ", ImageUrl = "https://images.unsplash.com/photo-1465847899084-d164df4f8e6d?w=400&h=400&fit=crop", Price = "R$ 200", Rating = 4.9, Type = EventType.Presencial, OrganizerName = "Orquestra Sinfônica", OrganizerAvatar = "profile_default_icon.png", Description = "A Orquestra Sinfônica apresenta seu concerto de primavera com um repertório especial que celebra as obras clássicas mais apreciadas." },
            new() { Name = "Exposição de Arte Moderna", Category = "Arte", Date = "8 Fev, 2027", Location = "MASP, SP", ImageUrl = "https://images.unsplash.com/photo-1531913764164-f85c3e03d0fe?w=400&h=400&fit=crop", Price = "R$ 30", Rating = 4.4, Type = EventType.Presencial, OrganizerName = "MASP Cultural", OrganizerAvatar = "profile_default_icon.png", Description = "Uma exposição imperdível com obras dos maiores artistas modernistas brasileiros e internacionais. Curadoria exclusiva." },
            new() { Name = "Maratona Esportiva", Category = "Esporte", Date = "15 Mar, 2027", Location = "Avenida Paulista, SP", ImageUrl = "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?w=400&h=400&fit=crop", Price = "Grátis", Rating = 4.3, Type = EventType.Hibrido, OnlineUrl = "https://youtube.com/maratonaevent", OrganizerName = "Esporte Brasil", OrganizerAvatar = "profile_default_icon.png", Description = "Participe da maior maratona esportiva do país. Percurso de 10km pelas principais avenidas com opção de participação virtual." },
        };

        Events.Clear();
        foreach (var item in _allEvents)
            Events.Add(item);
    }
}
