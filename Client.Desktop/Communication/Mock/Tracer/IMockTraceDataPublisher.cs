using System;
using System.Threading.Tasks;
using Service.Monitoring.Shared;

namespace Client.Desktop.Communication.Mock.Tracer;

public interface IMockTraceDataPublisher
{
    event Func<ServiceTraceDataCommand, Task>? LogReceived;
}