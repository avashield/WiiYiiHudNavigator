using System.Globalization;

namespace WiiYiiHudNavigator.Common.Converters;

public class IntToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int intValue)
        {
            // If parameter is "true", then convert to inverted bool (0 = true, >0 = false)
            if (parameter?.ToString()?.ToLower() == "true")
                return intValue == 0;
            
            // Default: convert to bool (0 = false, >0 = true)
            return intValue > 0;
        }

        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}