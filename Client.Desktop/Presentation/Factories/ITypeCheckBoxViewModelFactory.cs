using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Documentation;

namespace Client.Desktop.Presentation.Factories;

public interface ITypeCheckBoxViewModelFactory
{
    TypeCheckBoxViewModel Create(NoteTypeClientModel noteTypeClientModel);
}