using System;

namespace Client.Desktop.Communication.RequiresChange;

public interface IRunTimeSettings
{
    DateTimeOffset SelectedDate { get; set; }
    bool IsSelectedDateCurrentDate();
}