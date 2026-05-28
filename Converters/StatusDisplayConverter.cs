using System.Globalization;

namespace CoreventApp.Converters;

public class StatusDisplayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            "draft" => "RASCUNHO",
            "opened" => "ATIVO",
            "going" => "EM ANDAMENTO",
            "canceled" => "CANCELADO",
            "finished" => "ENCERRADO",
            _ => "ATIVO"
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
