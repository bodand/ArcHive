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

public class ListPageViewModel(
    ISearchService searchService,
    [KeyFilter("cached")]
    ICoverService coverService
)
{
    private CancellationTokenSource? _cts = null;
    public readonly ObservableCollection<BookCardViewModel> ListElements = [];

    public async Task DoSearch(ISearchFields fields, ISearchInitiator searchInitiator)
    {
        _cts?.Cancel();
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
