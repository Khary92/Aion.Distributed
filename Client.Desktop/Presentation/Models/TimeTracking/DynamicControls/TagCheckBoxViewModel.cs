using System.Threading.Tasks;
using Client.Desktop.Communication.Local;
using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.DataModels;
using Client.Desktop.Lifecycle.Startup.Tasks.Register;
using Client.Tracing.Tracing.Tracers;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class TagCheckBoxViewModel(ITraceCollector tracer, INotificationPublisherFacade notificationPublisher)
    : ReactiveObject, IEventRegistration
{
    private bool _isChecked;
    private TagClientModel? _tag;

    public TagClientModel? Tag
    {
        get => _tag;
        set => this.RaiseAndSetIfChanged(ref _tag, value);
    }

    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }

    public void RegisterMessenger()
    {
        notificationPublisher.Tag.ClientTagUpdatedNotificationReceived += HandleClientTagUpdatedNotification;
    }

    public void UnregisterMessenger()
    {
        notificationPublisher.Tag.ClientTagUpdatedNotificationReceived -= HandleClientTagUpdatedNotification;
    }

    private async Task HandleClientTagUpdatedNotification(ClientTagUpdatedNotification notification)
    {
        if (Tag == null || Tag.TagId != notification.TagId) return;

        Tag.Apply(notification);

        await tracer.Tag.Update.ChangesApplied(GetType(), notification.TraceId);
    }
}