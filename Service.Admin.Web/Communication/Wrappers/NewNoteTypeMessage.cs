using Service.Admin.Web.Models;

namespace Service.Admin.Web.Communication.Wrappers
{
    public record NewNoteTypeMessage(NoteTypeWebModel NoteType, Guid TraceId);
}