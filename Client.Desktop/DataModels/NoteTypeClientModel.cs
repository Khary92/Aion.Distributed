using System;
using Client.Desktop.Communication.Notifications.NoteType.Records;
using ReactiveUI;

namespace Client.Desktop.DataModels;

public class NoteTypeClientModel : ReactiveObject
{
    private readonly Guid _noteTypeId;

    private string _color = string.Empty;
    private string _name = string.Empty;

    private string _previousColor;
    private string _previousName;

    public NoteTypeClientModel(Guid noteTypeId, string name, string color)
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

    public void Apply(ClientNoteTypeColorChangedNotification notification)
    {
        Color = notification.Color;
    }

    public void Apply(ClientNoteTypeNameChangedNotification notification)
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