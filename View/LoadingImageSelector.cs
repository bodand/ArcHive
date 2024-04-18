using System;
using ArcHive.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ArcHive.View;

/// <summary>
///     A DataTemplateSelector used to select which element to render for a
///     given image that is currently loading.
///     When it loads, it either ends up as shown and rendered, or errors, which
///     ends up rendering the provided error template.
/// </summary>
public class LoadingImageSelector : DataTemplateSelector
{
    /// <summary>
    ///     The template to render while the image is loading.
    /// </summary>
    public DataTemplate Loading { get; set; } = default!;

    /// <summary>
    ///     The template to render after the image has loaded. Is passed the
    ///     <see cref="LoadingUrl"/> object.
    /// </summary>
    public DataTemplate Loaded { get; set; } = default!;

    /// <summary>
    ///     The template to render if the image has failed loading.
    /// </summary>
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
