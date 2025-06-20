using Service.Server.CQRS.Requests.Settings;
using Service.Server.Old.Services.Entities.Settings;

namespace Service.Server.Old.Handler.Requests.Settings;

public class IsExportPathValidRequestHandler(
    ISettingsRequestsService settingsRequestsService,
    IFileSystemWrapper fileSystemWrapper) :
    IRequestHandler<IsExportPathValidRequest, bool>
{
    public async Task<bool> Handle(IsExportPathValidRequest request, CancellationToken cancellationToken)
    {
        var settingsDto = await settingsRequestsService.Get();

        return fileSystemWrapper.IsDirectoryExisting(settingsDto!.ExportPath);
    }
}