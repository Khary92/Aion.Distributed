using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Avalonia.Communication.Commands;
using Client.Avalonia.Communication.Notifications.NoteType;
using Client.Avalonia.Communication.Requests;
using CommunityToolkit.Mvvm.Messaging;
using Contract.DTO;
using Contract.Tracing;
using Contract.Tracing.Tracers;
using DynamicData;
using Proto.Command.NoteTypes;
using Proto.Notifications.NoteType;
using ReactiveUI;

namespace Client.Avalonia.Models.Data;

public class NotesDataModel(
    ICommandSender commandSender,
    IRequestSender requestSender,
    IMessenger messenger)
    //ITracingCollectorProvider tracer)
    : ReactiveObject
{
    private ObservableCollection<NoteTypeDto> _noteTypes = [];

    public ObservableCollection<NoteTypeDto> NoteTypes
    {
        get => _noteTypes;
        set => this.RaiseAndSetIfChanged(ref _noteTypes, value);
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewNoteTypeMessage>(this, (_, m) =>
        {
            //tracer.NoteType.Create.AggregateReceived(GetType(), m.NoteType.NoteTypeId, m.NoteType.AsTraceAttributes());
            NoteTypes.Add(m.NoteType);
            //tracer.NoteType.Create.AggregateAdded(GetType(), m.NoteType.NoteTypeId);
        });

        messenger.Register<NoteTypeColorChangedNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.NoteTypeId);
            //tracer.NoteType.ChangeColor.NotificationReceived(GetType(), parsedId, m);

            var noteType = NoteTypes.FirstOrDefault(n => n.NoteTypeId == parsedId);

            if (noteType == null)
            {
                //tracer.NoteType.ChangeColor.NoAggregateFound(GetType(), parsedId);
                return;
            }

            noteType.Apply(m);
            //tracer.NoteType.ChangeColor.ChangesApplied(GetType(), parsedId);
        });

        messenger.Register<NoteTypeNameChangedNotification>(this, (_, m) =>
        {
            var parsedId = Guid.Parse(m.NoteTypeId);
            //tracer.NoteType.ChangeName.NotificationReceived(GetType(), parsedId, m);

            var noteType = NoteTypes.FirstOrDefault(n => n.NoteTypeId == parsedId);

            if (noteType == null)
            {
                //tracer.NoteType.ChangeName.NoAggregateFound(GetType(), parsedId);
                return;
            }

            noteType.Apply(m);
            //tracer.NoteType.ChangeName.ChangesApplied(GetType(), parsedId);
        });
    }

    public async Task Initialize()
    {
        NoteTypes.AddRange(await requestSender.GetAllNoteTypes());
    }

    public async Task AddNewNoteType(string noteType, string color)
    {
        if (string.IsNullOrWhiteSpace(noteType) || NoteTypes.Any(nt => nt.Name == noteType)) return;

        var createNoteTypeCommand = new CreateNoteTypeCommand
        {
            NoteTypeId = Guid.NewGuid().ToString(),
            Color = color,
            Name = noteType
        };

        await commandSender.Send(createNoteTypeCommand);

        //tracer.NoteType.Create.CommandSent(GetType(), Guid.Parse(createNoteTypeCommand.NoteTypeId),
         //   createNoteTypeCommand);
    }

    public async Task ChangeNoteTypeName(NoteTypeDto noteType)
    {
        var changeNoteTypeNameCommand = new ChangeNoteTypeNameCommand
        {
            NoteTypeId = noteType.NoteTypeId.ToString(),
            Name = noteType.Name
        };

        await commandSender.Send(changeNoteTypeNameCommand);

        //tracer.NoteType.ChangeName.CommandSent(GetType(), noteType.NoteTypeId, changeNoteTypeNameCommand.NoteTypeId);
    }

    public async Task ChangeNoteTypeColor(NoteTypeDto noteType)
    {
        var changeNoteTypeColorCommand = new ChangeNoteTypeColorCommand
        {
            NoteTypeId = noteType.NoteTypeId.ToString(),
            Color = noteType.Color
        };

        await commandSender.Send(changeNoteTypeColorCommand);

        //tracer.NoteType.ChangeColor.CommandSent(GetType(), noteType.NoteTypeId, changeNoteTypeColorCommand.NoteTypeId);
    }
}