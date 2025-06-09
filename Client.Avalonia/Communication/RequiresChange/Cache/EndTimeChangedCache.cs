using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Contract.FileSystem;
using Proto.Command.TimeSlots;

namespace Client.Avalonia.Communication.RequiresChange.Cache;

public class EndTimeChangedCache(
    // ITimeSlotCommandsService timeSlotCommandsService,
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemWriter fileSystemWriter,
    IFileSystemReader fileSystemReader) : IPersistentCache<SetEndTimeCommand>
{
    private const string Path = ".\\EndTimes.json";

    private readonly Dictionary<Guid, SetEndTimeCommand> _commands = new();

    public async Task Persist()
    {
        if (!fileSystemWrapper.IsFileExisting(Path)) return;

        var data = await fileSystemReader.GetObject<Dictionary<Guid, SetEndTimeCommand>>(Path);

        // foreach (var command in data.Values) await timeSlotCommandsService.SetEndTime(command);

        CleanUp();
    }

    public void Store(SetEndTimeCommand command)
    {
        if (!_commands.TryAdd(Guid.Parse(command.TimeSlotId), command)) _commands[Guid.Parse(command.TimeSlotId)] = command;

        fileSystemWriter.Write(JsonSerializer.Serialize(_commands), Path);
    }

    private void CleanUp()
    {
        fileSystemWrapper.Delete(Path);
    }
}