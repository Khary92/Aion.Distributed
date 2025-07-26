using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.Notifications.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.DataModels;
using Client.Desktop.DataModels.Decorators;
using Client.Desktop.Services.Initializer;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Command.Tags;
using Proto.Requests.AnalysisData;
using Proto.Requests.Tags;
using ReactiveUI;

namespace Client.Desktop.Presentation.Models.Analysis;

public class AnalysisByTagModel(IRequestSender requestSender, IMessenger messenger)
    : ReactiveObject, IInitializeAsync, IRegisterMessenger
{
    private AnalysisByTagDecorator? _analysisByTag;

    public ObservableCollection<TagClientModel> Tags { get; } = [];

    public AnalysisByTagDecorator? AnalysisByTag
    {
        get => _analysisByTag;
        private set => this.RaiseAndSetIfChanged(ref _analysisByTag, value);
    }

    public InitializationType Type => InitializationType.Model;

    public async Task InitializeAsync()
    {
        Tags.Clear();
        Tags.AddRange(await requestSender.Send(new GetAllTagsRequestProto()));
    }

    public void RegisterMessenger()
    {
        messenger.Register<NewTagMessage>(this, (_, m) => { Tags.Add(m.Tag); });

        messenger.Register<UpdateTagCommandProto>(this, (_, m) =>
        {
            var tag = Tags.FirstOrDefault(t => t.TagId == Guid.Parse(m.TagId));

            if (tag == null) return;

            tag.Name = m.Name;
        });
    }

    public async Task SetAnalysisForTag(TagClientModel selectedTag)
    {
        AnalysisByTag = await requestSender.Send(new GetTagAnalysisById
        {
            TagId = selectedTag.TagId.ToString()
        });
    }
}