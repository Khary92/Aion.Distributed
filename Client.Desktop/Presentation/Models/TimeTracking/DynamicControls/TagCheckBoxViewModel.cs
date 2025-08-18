using Client.Desktop.Communication.Notifications.Tag.Records;
using Client.Desktop.DataModels;
using Client.Tracing.Tracing.Tracers;
using CommunityToolkit.Mvvm.Messaging;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class TagCheckBoxViewModel(IMessenger messenger, ITraceCollector tracer) : ReactiveObject
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
        messenger.Register<ClientTagUpdatedNotification>(this, async void (_, notification) =>
        {
            if (Tag == null || Tag.TagId != notification.TagId) return;

            Tag.Apply(notification);
            await tracer.Tag.Update.ChangesApplied(GetType(), notification.TraceId);
        });
    }
}