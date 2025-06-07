using Contract.Notifications.Entities.NoteType;
using ReactiveUI;

namespace Contract.DTO;

public class NoteTypeDto : ReactiveObject
{
    private readonly Guid _noteTypeId;
    
    private string _color = string.Empty;
    private string _name = string.Empty;

    private string _previousColor;
    private string _previousName;

    public NoteTypeDto(Guid noteTypeId, string name, string color)
    {
        NoteTypeId = noteTypeId;
        Name = name;
        Color = color;

        _previousName = name;
        _previousColor = color;
    }
    
    public Guid NoteTypeId
    {
        get => _noteTypeId;
        private init => this.RaiseAndSetIfChanged(ref _noteTypeId, value);
    }

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    public string Color
    {
        get => _color;
        set => this.RaiseAndSetIfChanged(ref _color, value);
    }

    public void Apply(NoteTypeColorChangedNotification notification)
    {
        Color = notification.Color;
    }

    public void Apply(NoteTypeNameChangedNotification notification)
    {
        Name = notification.Name;
    }

    public bool IsColorChanged()
    {
        if (_color == _previousColor) return false;
        
        _previousColor = _color;
        return true;
    }

    public bool IsNameChanged()
    {
        if (_name == _previousName) return false;
        
        _previousName = _name;
        return true;
    }

    public override string ToString()
    {
        return $"NoteTypeDto:{{Name:'{Name}', Color:'{Color}'}}";
    }
}