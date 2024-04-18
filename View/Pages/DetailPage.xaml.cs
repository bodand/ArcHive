using System;
using System.Numerics;
using ArcHive.Model;
using ArcHive.ViewModel.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace ArcHive.View.Pages;

/// <summary>
///     The page object for rendering the detailed information of a book.
/// </summary>
[ObservableObject]
public sealed partial class DetailPage : Page
{
    /// <summary>
    ///     The viewmodel of the detail view.
    /// </summary>
    public DetailPageViewModel Model { get; set; }

    /// <summary>
    ///     Constructs a view and constructs the model.
    /// </summary>
    public DetailPage()
    {
        InitializeComponent();

        Model = App.Current.Provider.Get<DetailPageViewModel>();

        ImageCard.Translation = new Vector3(0, 0, 48);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is not Work payload) throw new NullReferenceException();
        Model.Work = payload;
    }
}
