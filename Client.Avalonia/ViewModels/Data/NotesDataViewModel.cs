using System.Threading.Tasks;
using Avalonia.Media;
using Contract.DTO;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Avalonia.ViewModels.Data;

public class NotesDataViewModel : ReactiveObject
{
    private string _editButtonText = "Edit";

    private bool _isEditMode;

    private string _nameInputFieldText = string.Empty;

    private Color _selectedColor;

    private NoteTypeDto? _selectedNoteType;

    public NotesDataViewModel(NotesDataModel model)
    {
        Model = model;

        AddNoteTypeCommand = ReactiveCommand.CreateFromTask(AddNoteType);
        EditNoteTypeCommand = ReactiveCommand.Create(ToggleTagEditMode);

        _ = InitializeAsync();
    }

    public NotesDataModel Model { get; }

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

    public NoteTypeDto? SelectedNoteType
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

    private async Task InitializeAsync()
    {
        await Model.Initialize();
        Model.RegisterMessenger();
    }

    private void ResetData()
    {
        SelectedColor = Colors.Transparent;
        NameInputFieldText = string.Empty;
    }

    private async Task AddNoteType()
    {
        if (!IsEditMode)
        {
            await Model.AddNewNoteType(NameInputFieldText, SelectedColor.ToString());
            ResetData();
            return;
        }

        if (SelectedNoteType == null) return;

        SelectedNoteType.Name = NameInputFieldText;
        SelectedNoteType.Color = SelectedColor.ToString();

        if (SelectedNoteType.IsColorChanged()) await Model.ChangeNoteTypeColor(SelectedNoteType);

        if (SelectedNoteType.IsNameChanged()) await Model.ChangeNoteTypeName(SelectedNoteType);

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