using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.Documentation;

namespace Client.Desktop.Presentation.Factories;

public class TypeCheckBoxViewModelFactory : ITypeCheckBoxViewModelFactory
{
    public TypeCheckBoxViewModel Create(NoteTypeClientModel noteTypeClientModel)
    {
        var typeCheckBoxViewModel = new TypeCheckBoxViewModel(noteTypeClientModel);
        return typeCheckBoxViewModel;
    }
}