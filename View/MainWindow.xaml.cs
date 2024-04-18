using System;
using System.ComponentModel;
using Windows.ApplicationModel;
using Windows.Foundation;
using ArcHive.ViewModel;
using Microsoft.UI.Input;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using ArcHive.Model.Search;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Numerics;
using ArcHive.View.Pages;

namespace ArcHive.View;

/// <summary>
///     The main window view of the application.
/// </summary>
[ObservableObject]
public sealed partial class MainWindow : Window
{
    private readonly AppWindow _appWindow;

    /// <summary>
    ///     The viewmodel of the view.
    /// </summary>
    public MainWindowViewModel Model { get; set; }

    [ObservableProperty]
    private bool _paneOpen;

    [ObservableProperty]
    private bool _basicSearchEnabled = true;

    /// <summary>
    ///     A search field for generating simple all-encompassing queries.
    /// </summary>
    public SimpleSearchField SimpleSearchField { get; set; } = new();

    /// <summary>
    ///     A search field for generating specially crafted queries.
    /// </summary>
    public ComplexSearchField ComplexSearchField { get; set; } = new();

    /// <summary>
    ///     A constructor that initializes the view, and sets the title-bar.
    ///     The viewmodel is also set, and is not null after this has run.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        ExtendsContentIntoTitleBar = true;

        Model = App.Current.Provider.Get<MainWindowViewModel>();
        Model.NavigationSource = ContentFrame;

        _appWindow = AppWindow;
        _appWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

        AppTitleBar.Loaded += AppTitleBar_Loaded;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        TitleBarTextBlock.Text = AppInfo.Current.DisplayInfo.DisplayName;

        SuggestBox.Translation = new Vector3(0, 0, 16);
        AdvButton.Translation = new Vector3(0, 0, 16);

        ContentFrame.IsNavigationStackEnabled = true;

        ContentFrame.Navigate(typeof(ListPage), Model);

        PropertyChanged += BasicSearchChecker;

        return;
        void BasicSearchChecker(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(PaneOpen)) return;
            BasicSearchEnabled = !PaneOpen;
        }
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar)
        {
            SetRegionsForCustomTitleBar();
        }
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (ExtendsContentIntoTitleBar)
        {
            SetRegionsForCustomTitleBar();
        }
    }

    private void SetRegionsForCustomTitleBar()
    {
        var scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;

        RightPaddingColumn.Width = new GridLength(_appWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(_appWindow.TitleBar.LeftInset / scaleAdjustment);

        var transform = TitleBarSearchBox.TransformToVisual(null);
        var bounds = transform.TransformBounds(new Rect(0, 0,
            TitleBarSearchBox.ActualWidth,
            TitleBarSearchBox.ActualHeight));
        var searchBoxRect = GetRect(bounds, scaleAdjustment);

        transform = BackButton.TransformToVisual(null);
        var backBounds = transform.TransformBounds(new Rect(0, 0,
            BackButton.ActualWidth,
            BackButton.ActualHeight));
        var backRect = GetRect(backBounds, scaleAdjustment);

        var nonClientInputSrc = InputNonClientPointerSource.GetForWindowId(AppWindow.Id);
        nonClientInputSrc.SetRegionRects(NonClientRegionKind.Passthrough, [searchBoxRect, backRect]);
    }

    private static Windows.Graphics.RectInt32 GetRect(Rect bounds, double scale)
    {
        return new Windows.Graphics.RectInt32(
            _X: (int)Math.Round(bounds.X * scale),
            _Y: (int)Math.Round(bounds.Y * scale),
            _Width: (int)Math.Round(bounds.Width * scale),
            _Height: (int)Math.Round(bounds.Height * scale)
        );
    }

    private void QueryMode_Switch(object sender, RoutedEventArgs e) => PaneOpen = !PaneOpen;

    private void BasicQuery_Start(AutoSuggestBox sender,
        AutoSuggestBoxQuerySubmittedEventArgs args) => Model.InitSearch(SimpleSearchField);

    private void BackButton_Click(object sender, RoutedEventArgs e) => Model.GoBack();

    private void AdvancedQuery_Start(object sender, RoutedEventArgs e) => Model.InitSearch(ComplexSearchField);
}

