using System.Collections.ObjectModel;
using CoreventApp.ViewModels;

namespace CoreventApp.Services;

public class AttractionStore
{
    private readonly Dictionary<string, ObservableCollection<Attraction>> _store = new();

    public ObservableCollection<Attraction> GetAttractions(string eventName)
    {
        if (!_store.ContainsKey(eventName))
        {
            _store[eventName] = new ObservableCollection<Attraction>();
            SeedInitialData(eventName);
        }
        return _store[eventName];
    }

    private void SeedInitialData(string eventName)
    {
        if (eventName.Contains("Conferência de Tecnologia", StringComparison.OrdinalIgnoreCase))
        {
            _store[eventName].Add(new Attraction
            {
                Title = "Keynote: O Futuro da IA",
                Guest = "Dr. Ricardo Mendes",
                StartTime = new TimeSpan(9, 0, 0),
                EndTime = new TimeSpan(10, 30, 0)
            });
            _store[eventName].Add(new Attraction
            {
                Title = "Painel: Inovação em Cloud",
                Guest = "Ana Oliveira, Carlos Silva",
                StartTime = new TimeSpan(11, 30, 0),
                EndTime = new TimeSpan(12, 30, 0)
            });
            _store[eventName].Add(new Attraction
            {
                Title = "Workshop: Rust para Iniciantes",
                Guest = "Pedro Costa",
                StartTime = new TimeSpan(14, 0, 0),
                EndTime = new TimeSpan(17, 0, 0)
            });
        }
        else if (eventName.Contains("Festival de Verão", StringComparison.OrdinalIgnoreCase))
        {
            _store[eventName].Add(new Attraction
            {
                Title = "Show de Abertura",
                Guest = "Banda Rock Nacional",
                StartTime = new TimeSpan(18, 0, 0),
                EndTime = new TimeSpan(19, 30, 0)
            });
            _store[eventName].Add(new Attraction
            {
                Title = "DJ Set Principal",
                Guest = "DJ Alves",
                StartTime = new TimeSpan(21, 0, 0),
                EndTime = new TimeSpan(23, 0, 0)
            });
        }
    }
}
