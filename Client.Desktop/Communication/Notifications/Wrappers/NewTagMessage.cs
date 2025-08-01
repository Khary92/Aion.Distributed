using System;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Notifications.Wrappers;

public record NewTagMessage(TagClientModel Tag, Guid TraceId);