using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;

namespace Client.Desktop.Presentation.Factories;

public interface ITagCheckBoxViewFactory
{
    TagCheckBoxViewModel Create(TagClientModel tag);
}