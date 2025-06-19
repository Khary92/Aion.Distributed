using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.Requests;
using Client.Desktop.Decorators;
using Client.Desktop.DTO;
using CommunityToolkit.Mvvm.Messaging;
using DynamicData;
using Proto.Command.Tags;
using Proto.Requests.AnalysisData;
using Proto.Requests.Tags;
using ReactiveUI;

namespace Client.Desktop.Models.Analysis;

public class AnalysisByTagModel : ReactiveObject
{
    private readonly IRequestSender _requestSender;
    private readonly IMessenger _messenger;

    private AnalysisByTagDecorator? _analysisByTag;

    public AnalysisByTagModel(IRequestSender requestSender, IMessenger messenger)
    {
        _requestSender = requestSender;
        _messenger = messenger;

        InitializeAsync().ConfigureAwait(false);
        RegisterMessenger();
    }

    public ObservableCollection<TagDto> Tags { get; } = [];


    public AnalysisByTagDecorator? AnalysisByTag
    {
        get => _analysisByTag;
        private set => this.RaiseAndSetIfChanged(ref _analysisByTag, value);
    }

    private void RegisterMessenger()
    {
        _messenger.Register<NewTagMessage>(this, (_, m) => { Tags.Add(m.Tag); });

        _messenger.Register<UpdateTagCommandProto>(this, (_, m) =>
        {
            var tag = Tags.FirstOrDefault(t => t.TagId == Guid.Parse(m.TagId));

            if (tag == null) return;

            tag.Name = m.Name;
        });
    }

    private async Task InitializeAsync()
    {
        Tags.Clear();
        Tags.AddRange(await _requestSender.Send(new GetAllTagsRequestProto()));
    }

    public async Task SetAnalysisForTag(TagDto selectedTag)
    {
        AnalysisByTag = await _requestSender.Send(new GetTagAnalysisById()
        {
            TagId = selectedTag.TagId.ToString()
        });
    }
}