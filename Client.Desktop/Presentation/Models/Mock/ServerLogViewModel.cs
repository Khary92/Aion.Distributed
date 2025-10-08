namespace Client.Desktop.Presentation.Models.Mock;

public class ServerLogViewModel(ServerLogModel model)
{
    public ServerLogModel Model { get; } = model;
}