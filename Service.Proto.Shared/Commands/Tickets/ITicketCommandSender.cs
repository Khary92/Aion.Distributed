﻿using Proto.Command.Tickets;

namespace Service.Proto.Shared.Commands.Tickets;

public interface ITicketCommandSender
{
    Task<bool> Send(CreateTicketCommandProto command);
    Task<bool> Send(UpdateTicketDataCommandProto command);
    Task<bool> Send(UpdateTicketDocumentationCommandProto command);
}