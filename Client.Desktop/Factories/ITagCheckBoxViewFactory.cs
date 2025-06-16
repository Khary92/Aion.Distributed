using Client.Desktop.DTO;
using Client.Desktop.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Factories;

public interface ITagCheckBoxViewFactory
{
    TagCheckBoxViewModel Create(TagDto tag);
}