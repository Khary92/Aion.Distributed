using System.Threading.Tasks;
using Client.Desktop.Communication.Mock.DataProvider;
using Client.Desktop.FileSystem;
using Client.Desktop.FileSystem.Serializer;

namespace Client.Desktop.Services.Mock;

public class MockSeedSetupService(
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemReader fileSystemReader,
    IFileSystemWriter fileSystemWriter,
    ISerializationService serializationService)
    : IMockSeedSetupService
{
    private const string SettingsFileName = "MockSetup.json";

    private const int WorkDayCount = 1;
    private const int TrackingSlotsCount = 12;
    private const int SprintCount = 2;
    private const int TicketCount = 10;
    private const int TagCount = 7;
    private const int NoteCount = 30;
    private const int NoteTypeCount = 3;
    private const int AmountOfReplayDocumentation = 5;
    private const int MinimumAmountOfNotesPerTimeSlot = 3;
    private const int MaximumAmountOfNotesPerTimeSlot = 7;
    private const int MinimumAmountOfCheckedTagsPerTimeSlot = 2;
    private const int MaximumAmountOfCheckedTagsPerTimeSlot = 7;

    public async Task<MockSetup> ReadSetupFromFile()
    {
        if (IsClearSetup)
            return new MockSetup(1, 0, 0, 0, 0, 0, 0, 0, new MockRanges(0, 0), new MockRanges(0, 0));

        if (fileSystemWrapper.IsFileExisting(SettingsFileName))
            return await fileSystemReader.GetObject<MockSetup>(SettingsFileName);

        var mockSetup = new MockSetup(WorkDayCount, TrackingSlotsCount, SprintCount, TicketCount, TagCount, NoteCount,
            NoteTypeCount, AmountOfReplayDocumentation,
            new MockRanges(MinimumAmountOfNotesPerTimeSlot, MaximumAmountOfNotesPerTimeSlot),
            new MockRanges(MinimumAmountOfCheckedTagsPerTimeSlot, MaximumAmountOfCheckedTagsPerTimeSlot));

        await SaveSettings(mockSetup);
        return mockSetup;
    }

    public async Task SaveSettings(MockSetup changedSettings)
    {
        await fileSystemWriter.Write(serializationService.Serialize(changedSettings), SettingsFileName);
    }

    public bool IsClearSetup { get; set; }
}