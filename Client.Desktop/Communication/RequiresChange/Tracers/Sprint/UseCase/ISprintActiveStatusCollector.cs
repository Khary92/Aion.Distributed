using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.RequiresChange.Tracers.Sprint.UseCase;

public interface ISprintActiveStatusCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void NotificationReceived(Type originClassType, Guid traceId, object notification);
    void NoAggregateFound(Type originClassType, Guid traceId);
    void ChangesApplied(Type originClassType, Guid traceId);
}