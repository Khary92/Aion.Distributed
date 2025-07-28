using System;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class TypeCheckBoxViewModel(IRequestSender requestSender, NoteTypeClientModel noteTypeClientModel)
    : ReactiveObject
{
    private readonly NoteTypeClientModel _noteType = noteTypeClientModel;
    private readonly Guid _noteTypeId = noteTypeClientModel.NoteTypeId;
    private bool _isChecked;

    public Guid NoteTypeId
    {
        get => _noteTypeId;
        init => this.RaiseAndSetIfChanged(ref _noteTypeId, value);
    }

    public bool IsChecked
    {
        get => _isChecked;
        set => this.RaiseAndSetIfChanged(ref _isChecked, value);
    }

    public NoteTypeClientModel NoteType
    {
        get => _noteType;
        init => this.RaiseAndSetIfChanged(ref _noteType, value);
    }
}