using System;
using Client.Desktop.DataModels;
using CommunityToolkit.Mvvm.Messaging;
using Proto.Notifications.Tag;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

public class TagCheckBoxViewModel(IMessenger messenger) : ReactiveObject
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
        messenger.Register<TagUpdatedNotification>(this, (_, m) =>
        {
            if (Tag == null || Tag.TagId != Guid.Parse(m.TagId)) return;

            Tag.Apply(m);
        });
    }
}