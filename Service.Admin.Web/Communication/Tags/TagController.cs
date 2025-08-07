using Proto.Command.Tags;
using Proto.DTO.TraceData;
using Service.Admin.Tracing;
using Service.Admin.Web.Communication.Tags.Records;
using Service.Admin.Web.Models;
using Service.Admin.Web.Pages;

namespace Service.Admin.Web.Communication.Tags;

public class TagController(ITraceCollector tracer, ISharedCommandSender commandSender) : ITagController
{
    public TagWebModel? SelectedTag { get; set; }
    public string InputTagName { get; set; } = string.Empty;
    public bool IsEditMode { get; set; }

    private async Task UpdateTag()
    {
        if (SelectedTag == null) return;
        
        SelectedTag.Name = InputTagName;

        var traceId = Guid.NewGuid();
        await tracer.Tag.Update.StartUseCase(GetType(), traceId);

        var updateTagCommand = new WebUpdateTagCommand(SelectedTag.TagId, InputTagName, traceId);
        
        await tracer.Tag.Update.SendingCommand(GetType(), SelectedTag.TagId, updateTagCommand);
        await commandSender.Send(updateTagCommand.ToProto());
    }

    private async Task CreateTag()
    {
        var traceId = Guid.NewGuid();
        
        var createTagDto = new TagWebModel(Guid.NewGuid(), InputTagName, false);

        await tracer.Tag.Create.StartUseCase(GetType(), traceId);

        var createTagCommand = new WebCreateTagCommand(Guid.NewGuid(), InputTagName, traceId);

        await tracer.Tag.Create.SendingCommand(GetType(), createTagDto.TagId, createTagCommand);
        await commandSender.Send(createTagCommand.ToProto());

        InputTagName = string.Empty;
        IsEditMode = false;
    }

    public Task PersistTag()
    {
        return IsUpdateRequired() ? UpdateTag() : CreateTag();
    }


    private bool IsUpdateRequired()
    {
        return IsEditMode && SelectedTag != null;
    }

    public void ToggleTagEditMode()
    {
        IsEditMode = !IsEditMode;
        InputTagName = string.Empty;

        if (IsEditMode && SelectedTag != null)
        {
            InputTagName = SelectedTag.Name;
        }
    }
}