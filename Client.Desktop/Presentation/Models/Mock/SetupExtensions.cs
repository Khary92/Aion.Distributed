using Client.Desktop.Communication.Mock.DataProvider;

namespace Client.Desktop.Presentation.Models.Mock;

public static class SetupExtensions
{
    public static MockSetup ToSetup(this ServerSeedSettingsModel serverSettingsModel)
    {
        return new MockSetup(serverSettingsModel.WorkDayCount, serverSettingsModel.TrackingSlotsCount,
            serverSettingsModel.SprintCount, serverSettingsModel.TicketCount, serverSettingsModel.TagCount,
            serverSettingsModel.NoteTypeCount, serverSettingsModel.NoteCount,
            serverSettingsModel.AmountOfReplayDocumentation,
            new MockRanges(serverSettingsModel.MinimumAmountOfCheckedTagsPerTimeSlot,
                serverSettingsModel.MaximumAmountOfCheckedTagsPerTimeSlot),
            new MockRanges(serverSettingsModel.MinimumAmountOfNotesPerTimeSlot,
                serverSettingsModel.MaximumAmountOfNotesPerTimeSlot));
    }
}