using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client.Desktop.Converter;

public class DateTimeOffsetConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is DateTime dt ? new DateTimeOffset(dt) : value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is DateTime dt ? new DateTimeOffset(dt) : value;
    }
}