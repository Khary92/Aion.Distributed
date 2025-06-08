using System;
using Contract.CQRS.Commands.Entities.WorkDays;
using Proto.Command.WorkDays;

namespace Contract.Converters
{
    public static class WorkDayCommandConverter
    {
        public static CreateWorkDayProtoCommand ToProto(this CreateWorkDayCommand command) => new()
        {
            WorkDayId = command.WorkDayId.ToString(),
            Date = command.Date.ToString("o")
        };
    }
}