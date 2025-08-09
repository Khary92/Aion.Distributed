using System;
using Client.Desktop.Communication.Commands.WorkDays.Records;
using Google.Protobuf.WellKnownTypes;
using Proto.Command.WorkDays;
using Proto.DTO.TraceData;

namespace Client.Desktop.Communication.Commands.WorkDays;

public static class WorkDayExtensions
{
    public static CreateWorkDayCommandProto ToProto(this ClientCreateWorkDayCommand command)
    {
        return new CreateWorkDayCommandProto
        {
            WorkDayId = command.WorkDayId.ToString(),
            Date = Timestamp.FromDateTimeOffset(command.Date),
            TraceData = new TraceDataProto
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };
    }
}