using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.FileSystem;
using Client.Tracing.Tracing.Tracers;

namespace Client.Desktop.Services.Cache;

public class StartTimeChangedCache(
    ICommandSender commandSender,
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemWriter fileSystemWriter,
    IFileSystemReader fileSystemReader,
    ITraceCollector tracer) : IPersistentCache<ClientSetStartTimeCommand>
{
    private const string Path = "StartTimes.json";

    private readonly Dictionary<Guid, ClientSetStartTimeCommand> _commands = new();

    public async Task Persist()
    {
        var traceId = Guid.NewGuid();
        await tracer.TimeSlot.SetStartTime.StartUseCase(GetType(), traceId);

        if (!fileSystemWrapper.IsFileExisting(Path))
        {
            await tracer.TimeSlot.SetStartTime.CacheIsEmpty(GetType(), traceId);
            return;
        }

        var data = await fileSystemReader.GetObject<Dictionary<Guid, ClientSetStartTimeCommand>>(Path);

        foreach (var command in data.Values)
        {
            await tracer.TimeSlot.SetStartTime.SendingCommand(GetType(), traceId, command);
            await commandSender.Send(command);
        }

        CleanUp();
    }

    public void Store(ClientSetStartTimeCommand command)
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