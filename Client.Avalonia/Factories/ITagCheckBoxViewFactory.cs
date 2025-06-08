using Client.Avalonia.ViewModels.TimeTracking.DynamicControls;
using Contract.DTO;

namespace Client.Avalonia.Factories;

public interface ITagCheckBoxViewFactory
{
    TagCheckBoxViewModel Create(TagDto tag);
}