using Client.Desktop.Models.TimeTracking.DynamicControls;
using Contract.DTO;

namespace Client.Desktop.Factories;

public interface ITagCheckBoxViewFactory
{
    TagCheckBoxViewModel Create(TagDto tag);
}