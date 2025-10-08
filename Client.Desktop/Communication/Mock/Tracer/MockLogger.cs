using System;
using System.Threading.Tasks;
using Service.Monitoring.Shared;
using Service.Monitoring.Shared.Tracing;

namespace Client.Desktop.Communication.Mock.Tracer;

public class MockLogger : ITracingDataSender, IMockTraceDataPublisher
{
    public event Func<ServiceTraceDataCommand, Task>? LogReceived;

    public async Task<bool> Send(ServiceTraceDataCommand command)
    {
        if (LogReceived == null) return false;

        await LogReceived(command);
        return true;
    }
}