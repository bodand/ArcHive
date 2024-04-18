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

public partial class BookCardViewModel : ObservableObject
{
    [ObservableProperty]
    private Work _work;
    [ObservableProperty]
    private bool _loaded;
    [ObservableProperty]
    private LoadingUrl _iconUrl = new(null);

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
