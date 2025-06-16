using System;

namespace Client.Desktop.Services;

public interface IRunTimeSettings
{
    DateTimeOffset SelectedDate { get; set; }
    bool IsSelectedDateCurrentDate();
}