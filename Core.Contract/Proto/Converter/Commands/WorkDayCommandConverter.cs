using Contract.CQRS.Commands.Entities.WorkDays;
using Proto.Command.WorkDays;

namespace Contract.Proto.Converter.Commands
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