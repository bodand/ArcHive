using System;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Model.Search;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DetailPage = ArcHive.View.Pages.DetailPage;

namespace ArcHive.ViewModel;

/// <summary>
///     The viewmodel of the main window's view class, serves as the main
///     handler for page navigations.
/// </summary>
public sealed partial class MainWindowViewModel : ObservableObject, IPageLoad
{
    /// <inheritdoc />
    public event Func<ISearchFields, Task>? SearchInitiated;

    /// <summary>
    ///     The UI element that is the host for the navigation pages.
    /// </summary>
    public Frame? NavigationSource { get; set; }

    /// <summary>
    ///     A property that returns visible if there is an ongoing search, and
    ///     collapsed if there is not.
    /// </summary>
    [ObservableProperty]
    private Visibility _isSearchingConverted = Visibility.Collapsed;

    /// <summary>
    ///     Initiates a search to be invoked on the server.
    ///     The result is not awaitable, it is handled by the listing page.
    /// </summary>
    /// <param name="fields">The fields to query by.</param>
    /// <seealso cref="Pages.ListPageViewModel"/>
    public void InitSearch(ISearchFields fields)
    {
        IsSearchingConverted = Visibility.Visible;
        SearchInitiated?.Invoke(fields);
        if (NavigationSource?.CanGoBack ?? false) NavigationSource?.GoBack();
    }

    /// <inheritdoc />
    public void GoToDetails(Work work)
    {
        NavigationSource?.Navigate(typeof(DetailPage), work);
    }

    /// <inheritdoc />
    public void GoBack()
    {
        NavigationSource?.GoBack();
    }

    /// <inheritdoc />
    public void SearchEnded()
    {
        IsSearchingConverted = Visibility.Collapsed;
    }
}
