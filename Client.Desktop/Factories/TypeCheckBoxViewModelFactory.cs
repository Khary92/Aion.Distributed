using System;
using System.Threading.Tasks;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DTO;
using Client.Desktop.Models.Documentation;

namespace Client.Desktop.Factories;

public class TypeCheckBoxViewModelFactory(IRequestSender requestSender) : ITypeCheckBoxViewModelFactory
{
    public TypeCheckBoxViewModel Create(NoteTypeDto noteTypeDto)
    {
        var typeCheckBoxViewModel = new TypeCheckBoxViewModel(requestSender, noteTypeDto);
        return typeCheckBoxViewModel;
    }
}