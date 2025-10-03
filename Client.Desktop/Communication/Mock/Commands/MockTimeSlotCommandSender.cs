using System.Threading.Tasks;
using Client.Desktop.Communication.Commands.TimeSlots;
using Client.Desktop.Communication.Commands.TimeSlots.Records;

namespace Client.Desktop.Communication.Mock.Commands;

public class MockTimeSlotCommandSender : ITimeSlotCommandSender
{
    public Task<bool> Send(ClientSetStartTimeCommand command)
    {
        // irrelevant as this is only sent on startup and shutdown of the application.
        // This persists the times of the TrackingControls. This has no use in a mocked state.
        return Task.FromResult(true);
    }

    public Task<bool> Send(ClientSetEndTimeCommand command)
    {
        // irrelevant as this is only sent on startup and shutdown of the application.
        // This persists the times of the TrackingControls. This has no use in a mocked state.
        return Task.FromResult(true);
    }
}