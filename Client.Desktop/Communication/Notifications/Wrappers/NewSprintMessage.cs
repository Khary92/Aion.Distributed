using System;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Notifications.Wrappers;

public record NewSprintMessage(SprintClientModel Sprint, Guid TraceId);