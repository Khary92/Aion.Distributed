using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Desktop.Communication.NotificationWrappers;
using Client.Desktop.Communication.RequiresChange;
using CommunityToolkit.Mvvm.Messaging;
using Contract.CQRS.Requests.Tags;
using Contract.Decorators;
using Contract.DTO;
using DynamicData;
using MediatR;
using Proto.Command.Tags;
using ReactiveUI;

namespace Client.Desktop.Models.Analysis;

public class AnalysisByTagModel : ReactiveObject
{
    private readonly IAnalysisDataService _analysisDataService;
    private readonly IMediator _mediator;
    private readonly IMessenger _messenger;

    private AnalysisByTagDecorator? _analysisByTag;

    public AnalysisByTagModel(IMediator mediator, IMessenger messenger, IAnalysisDataService analysisDataService)
    {
        _mediator = mediator;
        _messenger = messenger;
        _analysisDataService = analysisDataService;
        _analysisDataService = analysisDataService;

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

        _messenger.Register<UpdateTagCommand>(this, (_, m) =>
        {
            var tag = Tags.FirstOrDefault(t => t.TagId == Guid.Parse(m.TagId));

            if (tag == null) return;

            tag.Name = m.Name;
        });
    }

    private async Task InitializeAsync()
    {
        Tags.Clear();
        Tags.AddRange(await _mediator.Send(new GetAllTagsRequest()));
    }

    public async Task SetAnalysisForTag(TagDto selectedTag)
    {
        AnalysisByTag = await _analysisDataService.GetAnalysisByTag(selectedTag);
    }
}