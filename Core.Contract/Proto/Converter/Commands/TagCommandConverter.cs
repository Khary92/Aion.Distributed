using Contract.CQRS.Commands.Entities.Tags;
using Proto.Command.Tags;

namespace Contract.Proto.Converter.Commands
{
    public static class TagCommandConverter
    {
        public static CreateTagProtoCommand ToProto(this CreateTagCommand command)
            => new()
            {
                TagId = command.TagId.ToString(),
                Name = command.Name
            };

        public static UpdateTagProtoCommand ToProto(this UpdateTagCommand command)
            => new()
            {
                TagId = command.TagId.ToString(),
                Name = command.Name
            };
    }
}