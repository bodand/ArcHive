using System;
using ArcHive.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ArcHive.View;

public class LoadingImageSelector : DataTemplateSelector
{
    public DataTemplate Loading { get; set; } = default!;
    public DataTemplate Loaded { get; set; } = default!;
    public DataTemplate Errored { get; set; } = default!;

    protected override DataTemplate SelectTemplateCore(object? item, DependencyObject container)
        => SelectTemplateCore(item);

    protected override DataTemplate SelectTemplateCore(object? item) =>
        item switch
        {
            LoadingUrl(null, false) => Loading,
            LoadingUrl(not null, false) => Loaded,
            _ => Errored,
        };
}
