using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ArcHive.Model.Search;
using ArcHive.Services.Covers;
using ArcHive.Services.Search;
using Autofac.Features.AttributeFilters;

namespace ArcHive.ViewModel.Pages;

/// <summary>
///     A viewmodel for the view of the page rendering the list of returned
///     search results.
/// </summary>
/// <param name="searchService">A search service.</param>
/// <param name="coverService">A "cache" capable cover service.</param>
public class ListPageViewModel(
    ISearchService searchService,
    [KeyFilter("cached")]
    ICoverService coverService
)
{
    private CancellationTokenSource? _cts = null;

    /// <summary>
    ///     The list of found books, represented by their viewmodels.
    /// </summary>
    public readonly ObservableCollection<BookCardViewModel> ListElements = [];

    /// <summary>
    ///     Starts a search based on the provided search fields object using the
    ///     ctor injected <see cref="ISearchService"/> object.
    /// </summary>
    /// <param name="fields">The fields to query for.</param>
    /// <param name="searchInitiator">
    ///     The search initiator object that is notified when the searching
    ///     finishes.
    /// </param>
    public async Task DoSearch(ISearchFields fields, ISearchInitiator searchInitiator)
    {
        if (_cts is not null) await _cts.CancelAsync();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        try
        {
            ListElements.Clear();

            var worksEnumerable = searchService.FindWorks(fields);
            await using var workEnumerator = worksEnumerable.GetAsyncEnumerator(token);
            do
            {
                if (token.IsCancellationRequested) return;
                if (workEnumerator.Current is null) continue;

                var work = workEnumerator.Current;
                ListElements.Add(new BookCardViewModel(work, coverService, token));
            } while (await workEnumerator.MoveNextAsync());
        }
        catch (TaskCanceledException)
        {
            // nop
        }
        catch (COMException ex)
        {
            // thrown if window is closed while this loop is running
            // (async void callback is not awaited)
            Debug.WriteLine($"COMException: {ex.Message}");
        }
        finally
        {
            searchInitiator.SearchEnded();
        }
    }
}
