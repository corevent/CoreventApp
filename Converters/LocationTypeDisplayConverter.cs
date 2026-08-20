using System.Globalization;

namespace CoreventApp.Converters;

public class LocationTypeDisplayConverter : IValueConverter
{
    private static readonly Dictionary<string, string> Map = new()
    {
        ["in_person"] = "Presencial",
        ["online"] = "Online",
        ["hybrid"] = "Híbrido"
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is string locationType && Map.TryGetValue(locationType, out var display)
            ? display
            : "Presencial";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
