using System;
using System.Collections.Generic;

namespace Client.Desktop.Communication.RequiresChange.Tracers.TimerSettings.UseCase;

public interface IChangeDocuTimerSaveIntervalTraceCollector
{
    void StartUseCase(Type originClassType, Guid traceId, Dictionary<string, string> attributes);
    void CommandSent(Type originClassType, Guid traceId, object command);
    void NotificationReceived(Type originClassType, Guid traceId, object notification);
    void NoAggregateFound(Type originClassType, Guid traceId);
    void ChangesApplied(Type originClassType, Guid traceId);
    void PropertyNotChanged(Type originClassType, Guid traceId, Dictionary<string, string> asTraceAttributes);
}