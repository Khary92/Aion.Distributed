using System;

namespace Client.Avalonia.Communication.RequiresChange;

public interface IRunTimeSettings
{
    DateTimeOffset SelectedDate { get; set; }
    bool IsSelectedDateCurrentDate();
}