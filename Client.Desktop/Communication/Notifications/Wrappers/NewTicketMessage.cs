using System;
using Client.Desktop.DataModels;

namespace Client.Desktop.Communication.Notifications.Wrappers;

public record NewTicketMessage(TicketClientModel Ticket, Guid TraceId);