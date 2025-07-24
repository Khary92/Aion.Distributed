using System;

namespace Client.Desktop.Services.LocalSettings.Commands;

public record SetWorkDaySelectionCommand(DateTimeOffset Date);