using System.Globalization;

namespace CoreventApp.Converters;

public class IntegerToVisibilityConverter : IValueConverter
{
  public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    if (value is int currentStep && parameter is string targetStepStr)
    {
      _ = int.TryParse(targetStepStr, out int targetStep);
      return currentStep == targetStep;
    }
    return false;
  }

  public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
  {
    return Binding.DoNothing;
  }
}