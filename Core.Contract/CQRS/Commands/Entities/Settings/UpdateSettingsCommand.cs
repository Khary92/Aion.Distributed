using MediatR;

namespace Contract.CQRS.Commands.Entities.Settings;

public record UpdateSettingsCommand(
    Guid SettingsId,
    string ExportPath,
    bool IsAddNewTicketsToCurrentSprintActive) : INotification, IRequest<Unit>;