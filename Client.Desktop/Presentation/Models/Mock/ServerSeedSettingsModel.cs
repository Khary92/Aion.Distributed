using System.Threading.Tasks;
using Client.Desktop.Lifecycle.Startup.Tasks.Initialize;
using Client.Desktop.Services.Mock;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Mock;

public class ServerSeedSettingsModel(IMockSeedSetupService mockSeedSetupService) : ReactiveObject, IInitializeAsync
{
    private int _amountOfReplayDocumentation = 5;
    private int _maximumAmountOfCheckedTagsPerTimeSlot = 7;
    private int _maximumAmountOfNotesPerTimeSlot = 7;
    private int _minimumAmountOfCheckedTagsPerTimeSlot = 2;
    private int _minimumAmountOfNotesPerTimeSlot = 3;
    private int _noteCount = 30;
    private int _noteTypeCount = 3;
    private int _sprintCount = 2;
    private int _tagCount = 7;
    private int _ticketCount = 100;
    private int _trackingSlotsCount = 12;
    private int _workDayCount = 1;

    public int WorkDayCount
    {
        get => _workDayCount;
        set => this.RaiseAndSetIfChanged(ref _workDayCount, value);
    }

    public int TrackingSlotsCount
    {
        get => _trackingSlotsCount;
        set => this.RaiseAndSetIfChanged(ref _trackingSlotsCount, value);
    }

    public int SprintCount
    {
        get => _sprintCount;
        set => this.RaiseAndSetIfChanged(ref _sprintCount, value);
    }

    public int TicketCount
    {
        get => _ticketCount;
        set => this.RaiseAndSetIfChanged(ref _ticketCount, value);
    }

    public int TagCount
    {
        get => _tagCount;
        set => this.RaiseAndSetIfChanged(ref _tagCount, value);
    }

    public int NoteCount
    {
        get => _noteCount;
        set => this.RaiseAndSetIfChanged(ref _noteCount, value);
    }

    public int NoteTypeCount
    {
        get => _noteTypeCount;
        set => this.RaiseAndSetIfChanged(ref _noteTypeCount, value);
    }

    public int AmountOfReplayDocumentation
    {
        get => _amountOfReplayDocumentation;
        set => this.RaiseAndSetIfChanged(ref _amountOfReplayDocumentation, value);
    }

    public int MinimumAmountOfNotesPerTimeSlot
    {
        get => _minimumAmountOfNotesPerTimeSlot;
        set => this.RaiseAndSetIfChanged(ref _minimumAmountOfNotesPerTimeSlot, value);
    }

    public int MaximumAmountOfNotesPerTimeSlot
    {
        get => _maximumAmountOfNotesPerTimeSlot;
        set => this.RaiseAndSetIfChanged(ref _maximumAmountOfNotesPerTimeSlot, value);
    }

    public int MinimumAmountOfCheckedTagsPerTimeSlot
    {
        get => _minimumAmountOfCheckedTagsPerTimeSlot;
        set => this.RaiseAndSetIfChanged(ref _minimumAmountOfCheckedTagsPerTimeSlot, value);
    }

    public int MaximumAmountOfCheckedTagsPerTimeSlot
    {
        get => _maximumAmountOfCheckedTagsPerTimeSlot;
        set => this.RaiseAndSetIfChanged(ref _maximumAmountOfCheckedTagsPerTimeSlot, value);
    }

    public InitializationType Type => InitializationType.MockModels;

    public async Task InitializeAsync()
    {
        var setup = await mockSeedSetupService.ReadSetupFromFile();

        WorkDayCount = setup.WorkDayCount;
        TrackingSlotsCount = setup.TrackingSlotsCount;
        SprintCount = setup.SprintCount;
        TicketCount = setup.TicketCount;
        TagCount = setup.AmountOfTags;
        NoteCount = setup.NoteCount;
        NoteTypeCount = setup.NoteTypeCount;
        AmountOfReplayDocumentation = setup.AmountOfReplayDocumentation;
        MinimumAmountOfCheckedTagsPerTimeSlot = setup.AmountOfCheckedTagsPerTimeSlot.Min;
        MaximumAmountOfCheckedTagsPerTimeSlot = setup.AmountOfCheckedTagsPerTimeSlot.Max;
        MinimumAmountOfNotesPerTimeSlot = setup.AmountOfNotesPerTimeSlot.Min;
        MaximumAmountOfNotesPerTimeSlot = setup.AmountOfNotesPerTimeSlot.Max;
    }

    public async Task Save()
    {
        await mockSeedSetupService.SaveSettings(this.ToSetup());
    }
}