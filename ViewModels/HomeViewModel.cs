using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CoreventApp.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    [RelayCommand]
    private async Task NavigateToFestival()
    {
        var eventData = new EventSummary
        {
            Name = "Festival de Verão 2026",
            Category = "MUSICA",
            Date = "15 Out, 2026 - 18:00",
            Location = "Parque Ibirapuera, São Paulo - SP",
            ImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=900&auto=format&fit=crop",
            Price = "R$ 150",
            OrganizerName = "Produtora XYZ",
            Description = "O maior festival de verão do Brasil está de volta! Com mais de 20 atrações nacionais e internacionais, o Festival de Verão 2026 promete ser inesquecível. Prepare-se para três dias de muita música, arte e cultura em um dos melhores parques da cidade.",
        };
        await Shell.Current.GoToAsync(nameof(Views.EventDetail), new Dictionary<string, object>
        {
            ["EventData"] = eventData
        });
    }

    [RelayCommand]
    private async Task NavigateToTech()
    {
        var eventData = new EventSummary
        {
            Name = "Tech Summit 2026",
            Category = "TEC",
            Date = "22 Nov, 2026 - 09:00",
            Location = "Centro de Convenções, São Paulo - SP",
            ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=900&auto=format&fit=crop",
            Price = "R$ 90",
            OrganizerName = "Tech Events Ltda",
            Description = "O Tech Summit 2026 reúne os maiores nomes da tecnologia para um dia de palestras, workshops e networking. Temas como inteligência artificial, cibersegurança, desenvolvimento web e muito mais serão abordados por especialistas do mercado.",
        };
        await Shell.Current.GoToAsync(nameof(Views.EventDetail), new Dictionary<string, object>
        {
            ["EventData"] = eventData
        });
    }
}