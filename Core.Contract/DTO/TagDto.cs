using Contract.Notifications.Entities.Tags;
using ReactiveUI;

namespace Contract.DTO;

public class TagDto : ReactiveObject
{
    private Guid _tagId;
    private string _name = string.Empty;
    private bool _isSelected;

    public TagDto(Guid tagId, string name, bool isSelected)
    {
        TagId = tagId;
        Name = name;
        IsSelected = isSelected;
    }

    public bool IsSelected
    {
        get => _isSelected;
        init => this.RaiseAndSetIfChanged(ref _isSelected, value);
    }

    public Guid TagId
    {
        get => _tagId;
        init => this.RaiseAndSetIfChanged(ref _tagId, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public void Apply(TagUpdatedNotification notification)
    {
        Name = notification.Name;
    }
}