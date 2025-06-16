using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Sprint.UseCase;

public interface ICreateSprintTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void AggregateReceived(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void AggregateAdded(Type originClassType, Guid traceId);
}