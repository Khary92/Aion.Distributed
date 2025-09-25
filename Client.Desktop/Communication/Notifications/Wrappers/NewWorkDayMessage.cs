using System;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Notifications.Wrappers;

public record NewWorkDayMessage(WorkDayClientModel WorkDay, Guid TraceId);