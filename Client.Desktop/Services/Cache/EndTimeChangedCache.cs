using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.FileSystem;
using Proto.Command.TimeSlots;

namespace Client.Desktop.Services.Cache;

public class EndTimeChangedCache(
    ICommandSender commandSender,
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemWriter fileSystemWriter,
    IFileSystemReader fileSystemReader) : IPersistentCache<ClientSetEndTimeCommand>
{
    private const string Path = "EndTimes.json";

    private readonly Dictionary<Guid, ClientSetEndTimeCommand> _commands = new();

    public async Task Persist()
    {
        if (!fileSystemWrapper.IsFileExisting(Path)) return;

        var data = await fileSystemReader.GetObject<Dictionary<Guid, ClientSetEndTimeCommand>>(Path);

        foreach (var command in data.Values) await commandSender.Send(command);

        CleanUp();
    }

    public void Store(ClientSetEndTimeCommand command)
    {
        if (!_commands.TryAdd(command.TimeSlotId, command))
            _commands[command.TimeSlotId] = command;

        fileSystemWriter.Write(JsonSerializer.Serialize(_commands), Path);
    }

    private void CleanUp()
    {
        fileSystemWrapper.Delete(Path);
    }
}