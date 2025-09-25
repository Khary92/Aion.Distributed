using System;
using System.Collections.Generic;
using System.Linq;
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

        foreach (var commandWithCorrectTraceId in data.Values.Select(command => command with { TraceId = traceId }))
        {
            await tracer.TimeSlot.SetStartTime.SendingCommand(GetType(), traceId, commandWithCorrectTraceId);
            await commandSender.Send(commandWithCorrectTraceId);
        }

        await CleanUp(traceId);
    }

    public void Store(ClientSetStartTimeCommand command)
    {
        if (!_commands.TryAdd(command.TimeSlotId, command))
            _commands[command.TimeSlotId] = command;

        fileSystemWriter.Write(JsonSerializer.Serialize(_commands), Path);
    }

    private async Task CleanUp(Guid traceId)
    {
        fileSystemWrapper.Delete(Path);

        if (fileSystemWrapper.IsFileExisting(Path))
        {
            await tracer.TimeSlot.SetEndTime.FlushingCacheFailed(GetType(), traceId, Path);
        }
    }
}