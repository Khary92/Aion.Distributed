using System;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using ReactiveUI;

namespace Client.Desktop.Models.Documentation;

public class TypeCheckBoxViewModel(IRequestSender requestSender, NoteTypeDto noteTypeDto) : ReactiveObject
{
    private readonly Guid _noteTypeId = noteTypeDto.NoteTypeId;
    private bool _isChecked;
    private NoteTypeDto _noteType = noteTypeDto;

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

    public NoteTypeDto NoteType
    {
        get => _noteType;
        init => this.RaiseAndSetIfChanged(ref _noteType, value);
    }
}