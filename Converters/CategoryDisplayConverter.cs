using System.Globalization;

namespace CoreventApp.Converters;

public class CategoryDisplayConverter : IValueConverter
{
    private static readonly Dictionary<string, string> Map = new()
    {
        ["music"] = "Música",
        ["tech"] = "Tecnologia",
        ["education"] = "Educação",
        ["sports"] = "Esportes",
        ["business"] = "Negócios",
        ["art_culture"] = "Arte e Cultura",
        ["gastronomy"] = "Gastronomia",
        ["health_wellness"] = "Saúde e Bem-estar",
        ["family_kids"] = "Família e Crianças",
        ["religious_spiritual"] = "Religioso/Espiritual",
        ["games"] = "Jogos",
        ["community_social"] = "Comunidade/Social",
        ["fashion_beauty"] = "Moda e Beleza",
        ["other"] = "Outro"
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string category && Map.TryGetValue(category, out var display)
            ? display
            : value ?? "";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
