using System;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Requests;
using Contract.DTO;
using ReactiveUI;

namespace Client.Avalonia.Models.Documentation;

public class TypeCheckBoxViewModel(IRequestSender requestSender) : ReactiveObject
{
    private readonly Guid _noteTypeId;
    private bool _isChecked;
    private NoteTypeDto? _noteType;

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

    public NoteTypeDto? NoteType
    {
        get
        {
            if (_noteType == null) _ = LoadNoteTypeAsync();

            return _noteType;
        }
        init => this.RaiseAndSetIfChanged(ref _noteType, value);
    }

    private async Task LoadNoteTypeAsync()
    {
        _noteType ??= await requestSender.GetNoteTypeById(NoteTypeId);
    }
}