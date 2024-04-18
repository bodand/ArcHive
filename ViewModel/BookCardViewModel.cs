using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Services.Covers;
using ArcHive.Services.Search;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Dispatching;

namespace ArcHive.ViewModel;

/// <summary>
///     A viewmodel for the book card view, as used when rendering the listing
///     of found entity elements.
/// </summary>
public partial class BookCardViewModel : ObservableObject
{
    /// <summary>
    ///     The book entity that is rendered in the card.
    /// </summary>
    [ObservableProperty]
    private Work _work;

    // todo
    [ObservableProperty]
    private bool _loaded;

    /// <summary>
    ///     The loading-ring capable url for the medium cover of the image.
    /// </summary>
    [ObservableProperty]
    private LoadingUrl _iconUrl = new(null);

    /// <summary>
    ///     A constructor that initializes the viewmodel, its fields and
    ///     internal event handlers.
    /// </summary>
    /// <param name="work">The book to show.</param>
    /// <param name="covers">The cover service.</param>
    /// <param name="token">
    ///     The cancellation token, which is passed to the cover service when
    ///     using it.
    /// </param>
    public BookCardViewModel(Work work, ICoverService covers, CancellationToken token)
    {
        _work = work;

        PropertyChanged += (e, load) =>
        {
            if (load.PropertyName != nameof(IconUrl)) return;
            if (IconUrl is not (Url: { } url, false)) return;
            Work.CoverUrl = url;
        };

        StartLoadingImage(covers, token);
    }

    private async void StartLoadingImage(ICoverService covers, CancellationToken token)
    {
        try
        {
            var url = await covers.GetCoverAny(Work.CoverOlId, Work.Isbns, ICoverService.Size.Medium, token);
            IconUrl = new LoadingUrl(url, url is null);
        }
        catch (TaskCanceledException) { }
    }
}
