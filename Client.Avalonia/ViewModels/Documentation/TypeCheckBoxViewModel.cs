using System;
using System.Threading.Tasks;
using Contract.DTO;
using MediatR;
using ReactiveUI;

namespace Client.Avalonia.ViewModels.Documentation;

public class TypeCheckBoxViewModel(IMediator mediator) : ReactiveObject
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
        _noteType ??= await mediator.Send(new GetNoteTypeByIdRequest(NoteTypeId));
    }
}