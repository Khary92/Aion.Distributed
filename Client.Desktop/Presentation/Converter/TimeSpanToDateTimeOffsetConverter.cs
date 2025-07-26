using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Client.Desktop.Presentation.Converter;

public class TimeSpanToDateTimeOffsetConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTimeOffset dateTimeOffset) return dateTimeOffset.TimeOfDay;

        return null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan) return new DateTimeOffset(DateTime.Today.Add(timeSpan));

        return DateTimeOffset.Now;
    }
}