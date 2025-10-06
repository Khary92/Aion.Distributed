using System;
using System.Threading.Tasks;
using Avalonia.Media;
using Client.Desktop.DataModels;
using Proto.Command.NoteTypes;
using Proto.DTO.TraceData;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerNoteTypeDataViewModel : ReactiveObject
{
    private string _editButtonText = "Edit";

    private bool _isEditMode;

    private string _nameInputFieldText = string.Empty;

    private Color _selectedColor;

    private NoteTypeClientModel? _selectedNoteType;

    public ServerNoteTypeDataViewModel(ServerNoteTypeDataModel model)
    {
        Model = model;

        AddNoteTypeCommand = ReactiveCommand.CreateFromTask(AddNoteType);
        EditNoteTypeCommand = ReactiveCommand.Create(ToggleTagEditMode);
    }

    public ServerNoteTypeDataModel Model { get; }

    private bool IsEditMode
    {
        get => _isEditMode;
        set
        {
            this.RaiseAndSetIfChanged(ref _isEditMode, value);
            EditButtonText = _isEditMode ? "Cancel Edit" : "Edit";
        }
    }

    public string EditButtonText
    {
        get => _editButtonText;
        private set => this.RaiseAndSetIfChanged(ref _editButtonText, value);
    }

    public Color SelectedColor
    {
        get => _selectedColor;
        set => this.RaiseAndSetIfChanged(ref _selectedColor, value);
    }

    public NoteTypeClientModel? SelectedNoteType
    {
        get => _selectedNoteType;
        set => this.RaiseAndSetIfChanged(ref _selectedNoteType, value);
    }

    public string NameInputFieldText
    {
        get => _nameInputFieldText;
        set => this.RaiseAndSetIfChanged(ref _nameInputFieldText, value);
    }

    public ReactiveCommand<Unit, Unit> AddNoteTypeCommand { get; }
    public ReactiveCommand<Unit, Unit> EditNoteTypeCommand { get; }


    private void ResetData()
    {
        SelectedColor = Colors.Transparent;
        NameInputFieldText = string.Empty;
    }

    private async Task AddNoteType()
    {
        if (!IsEditMode)
        {
            var createCommand = new CreateNoteTypeCommandProto()
            {
                NoteTypeId = Guid.NewGuid().ToString(),
                Color = SelectedColor.ToString(),
                Name = _nameInputFieldText,
                TraceData = new TraceDataProto()
                {
                    TraceId = Guid.NewGuid().ToString()
                }
            };

            await Model.Send(createCommand);
            ResetData();
            return;
        }

        if (SelectedNoteType == null) return;

        SelectedNoteType.Name = NameInputFieldText;
        SelectedNoteType.Color = SelectedColor.ToString();

        if (SelectedNoteType.IsColorChanged())
        {
            await Model.Send(new ChangeNoteTypeColorCommandProto()
            {
                NoteTypeId = SelectedNoteType.NoteTypeId.ToString(),
                Color = _selectedColor.ToString(),
                TraceData = new TraceDataProto()
                {
                    TraceId = Guid.NewGuid().ToString()
                }
            });
        }

        if (SelectedNoteType.IsNameChanged())
        {
            await Model.Send(new ChangeNoteTypeNameCommandProto()
            {
                NoteTypeId = SelectedNoteType.NoteTypeId.ToString(),
                Name = SelectedNoteType.Name,
                TraceData = new TraceDataProto()
                {
                    TraceId = Guid.NewGuid().ToString()
                }
            });
        }

        ToggleTagEditMode();
        ResetData();
    }

    private void ToggleTagEditMode()
    {
        IsEditMode = !IsEditMode;
        ResetData();

        if (!IsEditMode || SelectedNoteType == null) return;

        SelectedColor = Color.Parse(SelectedNoteType.Color);
        NameInputFieldText = SelectedNoteType.Name;
    }
}