using System.Text.Json;
using Application.Contract.CQRS.Commands.Entities.TimeSlots;
using Application.Contract.FileSystem;
using Application.Services.Entities.TimeSlots;

namespace Application.Cache;

public class StartTimeChangedCache(
    ITimeSlotCommandsService timeSlotCommandsService,
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemWriter fileSystemWriter,
    IFileSystemReader fileSystemReader) : IPersistentCache<SetStartTimeCommand>
{
    private const string Path = ".\\StartTimes.json";

    private readonly Dictionary<Guid, SetStartTimeCommand> _commands = new();

    public async Task Persist()
    {
        if (!fileSystemWrapper.IsFileExisting(Path)) return;

        var data = await fileSystemReader.GetObject<Dictionary<Guid, SetStartTimeCommand>>(Path);

        foreach (var command in data.Values) await timeSlotCommandsService.SetStartTime(command);

        CleanUp();
    }

    public void Store(SetStartTimeCommand command)
    {
        if (!_commands.TryAdd(command.TimeSlotId, command)) _commands[command.TimeSlotId] = command;

        fileSystemWriter.Write(JsonSerializer.Serialize(_commands), Path);
    }

    private void CleanUp()
    {
        fileSystemWrapper.Delete(Path);
    }
}