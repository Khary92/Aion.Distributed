using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Desktop.FileSystem;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Services.Cache;

public class StartTimeChangedCache(
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemWriter fileSystemWriter,
    IFileSystemReader fileSystemReader) : IPersistentCache<SetStartTimeCommandProto>
{
    private const string Path = ".\\StartTimes.json";

    private readonly Dictionary<Guid, SetStartTimeCommandProto> _commands = new();

    public async Task Persist()
    {
        if (!fileSystemWrapper.IsFileExisting(Path)) return;

        var data = await fileSystemReader.GetObject<Dictionary<Guid, SetStartTimeCommandProto>>(Path);

        //  foreach (var command in data.Values) await timeSlotCommandsService.SetStartTime(command);

        CleanUp();
    }

    public void Store(SetStartTimeCommandProto command)
    {
        if (!_commands.TryAdd(Guid.Parse(command.TimeSlotId), command))
            _commands[Guid.Parse(command.TimeSlotId)] = command;

        fileSystemWriter.Write(JsonSerializer.Serialize(_commands), Path);
    }

    private void CleanUp()
    {
        fileSystemWrapper.Delete(Path);
    }
}