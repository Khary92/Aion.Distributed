using System;
using Client.Desktop.Models.TimeTracking.DynamicControls;
using Contract.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Desktop.Factories;

public class TagCheckBoxViewFactory(IServiceProvider serviceProvider) : ITagCheckBoxViewFactory
{
    public TagCheckBoxViewModel Create(TagDto tag)
    {
        var tagCheckBoxViewModel = serviceProvider.GetRequiredService<TagCheckBoxViewModel>();

        tagCheckBoxViewModel.Tag = new TagDto(tag.TagId, tag.Name, tag.IsSelected);

        tagCheckBoxViewModel.RegisterMessenger();

        return tagCheckBoxViewModel;
    }
}