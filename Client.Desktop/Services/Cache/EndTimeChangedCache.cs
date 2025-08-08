using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Client.Desktop.Communication.Commands;
using Client.Desktop.Communication.Commands.TimeSlots.Records;
using Client.Desktop.FileSystem;
using Client.Tracing.Tracing.Tracers;

namespace Client.Desktop.Services.Cache;

public class EndTimeChangedCache(
    ICommandSender commandSender,
    IFileSystemWrapper fileSystemWrapper,
    IFileSystemWriter fileSystemWriter,
    IFileSystemReader fileSystemReader,
    ITraceCollector tracer) : IPersistentCache<ClientSetEndTimeCommand>
{
    private const string Path = "EndTimes.json";

    private readonly Dictionary<Guid, ClientSetEndTimeCommand> _commands = new();

    public async Task Persist()
    {
        var traceId = Guid.NewGuid();
        await tracer.TimeSlot.SetEndTime.StartUseCase(GetType(), traceId);
        
        if (!fileSystemWrapper.IsFileExisting(Path))
        {
            await tracer.TimeSlot.SetEndTime.CacheIsEmpty(GetType(), traceId);
            return;
        }

        var data = await fileSystemReader.GetObject<Dictionary<Guid, ClientSetEndTimeCommand>>(Path);

        foreach (var command in data.Values)
        {
            await tracer.TimeSlot.SetEndTime.SendingCommand(GetType(), traceId, command);
            await commandSender.Send(command);
        }
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