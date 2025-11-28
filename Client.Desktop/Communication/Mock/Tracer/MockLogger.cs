using System;
using System.Threading.Tasks;
using Service.Monitoring.Shared;
using Tracing_ITracingDataSender = Client.Tracing.ITracingDataSender;

namespace Client.Desktop.Communication.Mock.Tracer;

public class MockLogger : Tracing_ITracingDataSender, IMockTraceDataPublisher
{
    public event Func<ServiceTraceDataCommand, Task>? LogReceived;

    public async Task<bool> Send(ServiceTraceDataCommand command)
    {
        if (LogReceived == null) return false;

        await LogReceived(command);
        return true;
    }
}