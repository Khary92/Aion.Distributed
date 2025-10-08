using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Client.Desktop.Communication.Mock.Tracer;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using ReactiveUI;
using Service.Monitoring.Shared;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerLogModel(IMockTraceDataPublisher logPublisher) : ReactiveObject, IMessengerRegistration
{
    private ObservableCollection<ServiceTraceDataCommand> _traces = [];

    public ObservableCollection<ServiceTraceDataCommand> Traces
    {
        get => _traces;
        set => this.RaiseAndSetIfChanged(ref _traces, value);
    }

    public void RegisterMessenger()
    {
        logPublisher.LogReceived += AddTrace;
    }

    public void UnregisterMessenger()
    {
        logPublisher.LogReceived -= AddTrace;
    }

    private Task AddTrace(ServiceTraceDataCommand trace)
    {
        Traces.Add(trace);
        return Task.CompletedTask;
    }
}