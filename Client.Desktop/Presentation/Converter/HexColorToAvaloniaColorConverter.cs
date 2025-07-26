using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Client.Desktop.Presentation.Converter;

public class HexColorToAvaloniaColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string hex || string.IsNullOrEmpty(hex))
            return new SolidColorBrush(Colors.Transparent);

        try
        {
            var color = Color.Parse(hex);
            return new SolidColorBrush(color);
        }
        catch (FormatException)
        {
            return new SolidColorBrush(Colors.Transparent);
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is SolidColorBrush brush) return brush.Color.ToString();

        return "#000000";
    }
}