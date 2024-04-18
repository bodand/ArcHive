using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ArcHive.Model;
using ArcHive.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ArcHive.View;

public sealed partial class BookCardView : UserControl
{
    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
        nameof(Model),
        typeof(BookCardViewModel),
        typeof(BookCardView),
        new PropertyMetadata(null));

    public BookCardViewModel? Model
    {
        get => (BookCardViewModel) GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }

    public event Action<BookCardViewModel, string>? Clicked;

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