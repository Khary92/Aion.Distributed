using System;

namespace Client.Desktop.Communication.RequiresChange;

public class RunTimeSettings : IRunTimeSettings
{
    public DateTimeOffset SelectedDate { get; set; } = DateTimeOffset.Now;

    public bool IsSelectedDateCurrentDate()
    {
        return SelectedDate.Date == DateTimeOffset.Now.Date;
    }
}