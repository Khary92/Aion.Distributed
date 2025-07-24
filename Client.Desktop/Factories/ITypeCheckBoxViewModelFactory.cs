using Client.Desktop.DTO;
using Client.Desktop.Models.Documentation;

namespace Client.Desktop.Factories;

public interface ITypeCheckBoxViewModelFactory
{
    TypeCheckBoxViewModel Create(NoteTypeDto noteTypeDto);
}