using ArcHive.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace ArcHive.View;

public sealed partial class LoadingImage : UserControl
{
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source),
        typeof(LoadingUrl),
        typeof(LoadingImage),
        new PropertyMetadata(null));

    public LoadingUrl? Source
    {
        get => (LoadingUrl?)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public LoadingImage()
    {
        InitializeComponent();
    }
}
