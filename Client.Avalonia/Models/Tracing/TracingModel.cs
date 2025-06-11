using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Notifications.UseCase;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Tracing;

public class TracingModel(IMessenger messenger) : ReactiveObject
{
    private ObservableCollection<string> traces = [];

    public ObservableCollection<string> Traces
    {
        get => traces;
        set => this.RaiseAndSetIfChanged(ref traces, value);
    }

    public void RegisterMessenger()
    {
        messenger.Register<TraceReportSentNotification>(this, (_, m) => { traces.Add(m.TraceReportDto.Logs); });
    }
}