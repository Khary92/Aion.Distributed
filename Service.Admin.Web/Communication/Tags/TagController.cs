using Proto.Command.Tags;
using Proto.DTO.TraceData;
using Service.Admin.Tracing;
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

        await tracer.Tag.Update.StartUseCase(GetType(), SelectedTag.TagId, SelectedTag.AsTraceAttributes());

        var updateTagCommand = new UpdateTagCommandProto
        {
            TagId = SelectedTag.TagId.ToString(),
            Name = InputTagName,
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        await commandSender.Send(updateTagCommand);
        await tracer.Tag.Update.CommandSent(GetType(), SelectedTag.TagId, updateTagCommand);
    }

    private async Task CreateTag()
    {
        var createTagDto = new TagWebModel(Guid.NewGuid(), InputTagName, false);

        await tracer.Tag.Create.StartUseCase(GetType(), createTagDto.TagId, createTagDto.AsTraceAttributes());

        var createTagCommand = new CreateTagCommandProto
        {
            TagId = createTagDto.TagId.ToString(),
            Name = InputTagName,
            TraceData = new TraceDataProto()
            {
                TraceId = Guid.NewGuid().ToString()
            }
        };

        await commandSender.Send(createTagCommand);
        await tracer.Tag.Create.CommandSent(GetType(), createTagDto.TagId, createTagCommand);

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