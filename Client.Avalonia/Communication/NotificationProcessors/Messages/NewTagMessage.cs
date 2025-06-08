using Contract.DTO;

namespace Client.Avalonia.Communication.NotificationProcessors.Messages;

public record NewTagMessage(TagDto Tag);