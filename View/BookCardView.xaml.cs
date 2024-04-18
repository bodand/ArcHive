using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Numerics;
using ArcHive.ViewModel;

namespace ArcHive.View;

/// <summary>
///     A control that renders a single element as a card that shows the cover,
///     title and authors of a book.
/// </summary>
public sealed partial class BookCardView : UserControl
{
    /// <summary>
    ///     The model property of the view. Used to allow recycling of views in
    ///     the grid rendering view.
    /// </summary>
    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
        nameof(Model),
        typeof(BookCardViewModel),
        typeof(BookCardView),
        new PropertyMetadata(null));

    /// <summary>
    ///     The viewmodel of the view.
    /// </summary>
    public BookCardViewModel? Model
    {
        get => (BookCardViewModel) GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    /// <summary>
    ///     Dotnet event callback that is triggered when the card is clicked.
    /// </summary>
    public event Action<BookCardViewModel, string>? Clicked;

    /// <summary>
    ///     A constructor that initializes the view. The viewmodel is left
    ///     as a null.
    /// </summary>
    public BookCardView()
    {
        InitializeComponent();
        MainGrid.Translation = new Vector3(0, 0, 32);
    }

    private void MainGrid_Click(object sender, RoutedEventArgs e)
    {
        Clicked?.Invoke(Model!, "ConnectedIcon");
    }
}