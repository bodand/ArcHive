using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Model.Search;
using ArcHive.ViewModel;
using ArcHive.ViewModel.Navigation;
using ArcHive.ViewModel.Pages;

namespace ArcHive.View;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ListPage : Page
{
    public ListPage()
    {
        InitializeComponent();

        Model = App.Current.Provider.Get<ListPageViewModel>();
    }

    public readonly ListPageViewModel Model;

    private IPageNavigator? _pageNavigator;
    private BookCardViewModel? _navigationItem;

    private void BookCardView_Clicked(BookCardViewModel obj, string name)
    {
        _navigationItem = obj;
        _pageNavigator?.GoToDetails(_navigationItem.Work);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (e.Parameter is not IPageLoad load) return;

        _pageNavigator = load;
        load.SearchInitiated += InitOnSearchInitiated;

        return;
        Task InitOnSearchInitiated(ISearchFields arg)
        {
            return Model.DoSearch(arg, load);
        }
    }

    private void ResultsCollection_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (_navigationItem is null) return;
        ResultsCollection.ScrollIntoView(_navigationItem);
        ResultsCollection.UpdateLayout();
        ResultsCollection.Focus(FocusState.Programmatic);
    }
}
