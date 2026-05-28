using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CoreventApp.Views;
using System.Collections.ObjectModel;

namespace CoreventApp.ViewModels;

public partial class FavoritesViewModel : ObservableObject
{
    public ObservableCollection<EventSummary> FavoriteEvents { get; } = new();

    public FavoritesViewModel()
    {
        FavoriteEvents.Add(new EventSummary
        {
            Name = "Festival de Verão 2026",
            Category = "Música",
            Date = "15 Out, 2026",
            Location = "Praia de Copacabana, RJ",
            ImageUrl = "https://images.unsplash.com/photo-1493225457124-a3eb161ffa5f?w=400&h=400&fit=crop",
            Price = "R$ 150",
            Rating = 4.8,
            Type = EventType.Presencial,
            OrganizerName = "Corevent Produções",
            OrganizerAvatar = "profile_default_icon.png",
            Description = "O maior festival de verão do Brasil! Venha curtir os melhores artistas em um evento inesquecível à beira-mar. São mais de 20 atrações nacionais e internacionais confirmadas.",
            IsFavorite = true
        });

        FavoriteEvents.Add(new EventSummary
        {
            Name = "Tech Summit 2026",
            Category = "Tecnologia",
            Date = "22 Nov, 2026",
            Location = "Centro de Convenções, SP",
            ImageUrl = "https://images.unsplash.com/photo-1511795409834-ef04bbd61622?w=400&h=400&fit=crop",
            Price = "R$ 90",
            Rating = 4.9,
            Type = EventType.Hibrido,
            OnlineUrl = "https://zoom.us/techsummit2026",
            OrganizerName = "Tech Events Brasil",
            OrganizerAvatar = "profile_default_icon.png",
            Description = "O maior encontro de tecnologia da América Latina. Palestras, workshops e networking com os maiores nomes do setor. Disponível presencial e online.",
            IsFavorite = true
        });
    }

    [RelayCommand]
    private void ToggleFavorite(EventSummary eventItem)
    {
        eventItem.IsFavorite = !eventItem.IsFavorite;
        if (!eventItem.IsFavorite)
        {
            FavoriteEvents.Remove(eventItem);
        }
    }

    [RelayCommand]
    private async Task SelectEventAsync(EventSummary eventItem)
    {
        await Shell.Current.GoToAsync(nameof(Views.EventDetail), new Dictionary<string, object>
        {
            ["EventData"] = eventItem
        });
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
