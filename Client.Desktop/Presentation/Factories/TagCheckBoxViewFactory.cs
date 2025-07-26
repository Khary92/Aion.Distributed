using System;
using Client.Desktop.DataModels;
using Client.Desktop.Presentation.Models.TimeTracking.DynamicControls;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Presentation.Factories;

public class TagCheckBoxViewFactory(IServiceProvider serviceProvider) : ITagCheckBoxViewFactory
{
    public TagCheckBoxViewModel Create(TagClientModel tag)
    {
        var tagCheckBoxViewModel = serviceProvider.GetRequiredService<TagCheckBoxViewModel>();

        tagCheckBoxViewModel.Tag = new TagClientModel(tag.TagId, tag.Name, tag.IsSelected);

        tagCheckBoxViewModel.RegisterMessenger();

        return tagCheckBoxViewModel;
    }
}