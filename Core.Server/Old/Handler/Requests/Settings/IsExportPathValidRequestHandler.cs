using Application.Contract.CQRS.Requests.Settings;
using Application.Contract.FileSystem;
using Application.Services.Entities.Settings;
using MediatR;

namespace Application.Handler.Requests.Settings;

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