using System;
using ReactiveUI;

namespace Client.Desktop.DataModels.Local;

public class SettingsClientModel(string exportPath) : ReactiveObject
{
    private string _exportPath = exportPath;
    private DateTimeOffset _selectedDate = DateTimeOffset.Now;

    public string ExportPath
    {
        get => _exportPath;
        set => this.RaiseAndSetIfChanged(ref _exportPath, value);
    }

    public DateTimeOffset SelectedDate
    {
        get => _selectedDate;
        set => this.RaiseAndSetIfChanged(ref _selectedDate, value);
    }
}