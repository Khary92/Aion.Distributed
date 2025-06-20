using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.SQLite.Converter;

public class GeneralDateConverter() : ValueConverter<DateTimeOffset, string>(v => v.ToString("yyyy-MM-dd"),
    v => DateTime.ParseExact(v, "yyyy-MM-dd", null));