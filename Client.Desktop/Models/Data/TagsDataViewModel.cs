using System.Threading.Tasks;
using Client.Desktop.DTO;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Client.Desktop.Models.Data;

public class TagsDataViewModel : ReactiveObject
{
    private string _editButtonText = string.Empty;

    private string _inputTagName = string.Empty;

    private bool _isEditMode;

    private TagDto? _selectedTag;

    public TagsDataViewModel(TagsDataModel tagsDataModel)
    {
        DataModel = tagsDataModel;
        EditTagCommand = ReactiveCommand.Create(ToggleTagEditMode);

        PersistTagCommand = ReactiveCommand.CreateFromTask(PersistTagAsync,
            this.WhenAnyValue(x => x.InputTagName, name => !string.IsNullOrWhiteSpace(name)));

        IsEditMode = false;

        DataModel.InitializeAsync().ConfigureAwait(false);
        DataModel.RegisterMessenger();
    }

    public TagsDataModel DataModel { get; }

    public string InputTagName
    {
        get => _inputTagName;
        set => this.RaiseAndSetIfChanged(ref _inputTagName, value);
    }

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
        set => this.RaiseAndSetIfChanged(ref _editButtonText, value);
    }

    public TagDto? SelectedTag
    {
        get => _selectedTag;
        set => this.RaiseAndSetIfChanged(ref _selectedTag, value);
    }

    public ReactiveCommand<Unit, Unit> PersistTagCommand { get; }
    public ReactiveCommand<Unit, Unit> EditTagCommand { get; }

    private async Task PersistTagAsync()
    {
        await DataModel.PersistTagAsync(InputTagName, SelectedTag, IsEditMode);
        InputTagName = string.Empty;
    }

    private void ToggleTagEditMode()
    {
        IsEditMode = !IsEditMode;
        InputTagName = string.Empty;

        if (!IsEditMode) return;

        InputTagName = SelectedTag != null ? SelectedTag.Name : string.Empty;
    }
}