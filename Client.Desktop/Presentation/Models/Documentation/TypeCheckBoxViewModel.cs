using System;
using Client.Desktop.DataModels;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Documentation;

public class TypeCheckBoxViewModel(NoteTypeClientModel noteTypeClientModel)
    : ReactiveObject
{
    private readonly Guid _noteTypeId = noteTypeClientModel.NoteTypeId;
    private bool _isChecked;
    private readonly NoteTypeClientModel _noteTypeClientModel = noteTypeClientModel;

    public event EventHandler<bool>? CheckedChanged;
    
    public Guid NoteTypeId
    {
        get => _noteTypeId;
        init => this.RaiseAndSetIfChanged(ref _noteTypeId, value);
    }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref _isChecked, value);
            CheckedChanged?.Invoke(this, _isChecked);
        }
    }

    public NoteTypeClientModel NoteType
    {
        get => _noteTypeClientModel;
        init => this.RaiseAndSetIfChanged(ref _noteTypeClientModel, value);
    }
}