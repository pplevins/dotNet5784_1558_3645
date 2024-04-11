using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PL.converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Collapsed : Visibility.Visible;
        }
        else
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter?.ToString() == "IsString" && value is string strValue)
        {
            return strValue.Length >= 6 ? Visibility.Collapsed : Visibility.Visible;
        }
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class TimeSpanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            return $"{timeSpan.Days}.{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds}";
        }
        return value?.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string timeString)
        {
            string[] parts = timeString.Split('.');
            if (parts.Length == 2 && int.TryParse(parts[0], out int days))
            {
                string[] timeParts = parts[1].Split(':');
                if (timeParts.Length == 3 &&
                    int.TryParse(timeParts[0], out int hours) &&
                    int.TryParse(timeParts[1], out int minutes) &&
                    int.TryParse(timeParts[2], out int seconds))
                {
                    return new TimeSpan(days, hours, minutes, seconds);
                }
            }
        }
        return DependencyProperty.UnsetValue; // Return DependencyProperty.UnsetValue if conversion fails
    }
}