namespace Client.Avalonia.ViewModels.Tracing;

public class TracingViewModel
{
    public TracingViewModel(TracingModel model)
    {
        Model = model;
        Model.RegisterMessenger();
    }

    public TracingModel Model { get; }
}