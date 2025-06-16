using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Ticket.UseCase;

public interface IAddTicketToCurrentSprintTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
}