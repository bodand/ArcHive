using System;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Model.Search;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DetailPage = ArcHive.View.Pages.DetailPage;

namespace ArcHive.ViewModel;

public sealed partial class MainWindowViewModel : ObservableObject, IPageLoad
{
    public event Func<ISearchFields, Task>? SearchInitiated;

    public Frame? NavigationSource { get; set; }

    [ObservableProperty]
    private Visibility _isSearchingConverted = Visibility.Collapsed;

    public void InitSearch(ISearchFields fields)
    {
        IsSearchingConverted = Visibility.Visible;
        SearchInitiated?.Invoke(fields);
        if (NavigationSource?.CanGoBack ?? false) NavigationSource?.GoBack();
    }

    public void GoToDetails(Work work)
    {
        NavigationSource?.Navigate(typeof(DetailPage), work);
    }

    public void GoBack()
    {
        NavigationSource?.GoBack();
    }

    public void SearchEnded()
    {
        IsSearchingConverted = Visibility.Collapsed;
    }
}
