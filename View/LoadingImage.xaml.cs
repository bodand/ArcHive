using ArcHive.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ArcHive.View;

/// <summary>
///     A custom control that renders a loading-ring until an image source is
///     available.
/// </summary>
public sealed partial class LoadingImage : UserControl
{
    /// <summary>
    ///     The source dependency property of the control.
    /// </summary>
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source),
        typeof(LoadingUrl),
        typeof(LoadingImage),
        new PropertyMetadata(null));

    /// <summary>
    ///     The loading url source of the image.
    ///     Its value determines whether to render the loading-ring or the image
    ///     itself.
    /// </summary>
    public LoadingUrl? Source
    {
        get => (LoadingUrl?)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    ///     A constructor that initializes the control view.
    ///     The url is left as null.
    /// </summary>
    public LoadingImage()
    {
        InitializeComponent();
    }
}
