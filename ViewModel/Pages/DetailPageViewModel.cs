using System.ComponentModel;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Services.Covers;
using ArcHive.Services.Details;
using ArcHive.Services.Search;
using Autofac.Features.AttributeFilters;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.ViewModel.Pages;

public sealed partial class DetailPageViewModel : ObservableObject
{
    public DetailPageViewModel(
        [KeyFilter("cached")] ICoverService coverService,
        IDetailsService detailsService)
    {
        PropertyChanged += PropertyChangedImageHandler;

        return;

        async void PropertyChangedImageHandler(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Bookshelf):
                    UpdateBookshelfStats();
                    break;
                case nameof(Work):
                    UpdateRating();
                    await UpdateDetails(detailsService);
                    await UpdateImage(coverService);
                    break;
            }
        }
    }

    private void UpdateBookshelfStats()
    {
        if (Bookshelf is null) return;
        WantRate = BookshelfValueToRate(Bookshelf.WantToRead);
        CurrentRate = BookshelfValueToRate(Bookshelf.Current);
        ReadedRate = BookshelfValueToRate(Bookshelf.AlreadyRead);
    }

    private static string BookshelfValueToRate(int val)
    {
        if (val == 0) return "1*";
        return $"{val}*";
    }

    private async Task UpdateDetails(IDetailsService detailsService)
    {
        Bookshelf = await detailsService.BookshelfOf(Work.OlId);
        Details = await detailsService.GetDetailsOf(Work.OlId);
    }

    private async Task UpdateImage(ICoverService coverService)
    {

        var url = await coverService.GetCoverAny(Work.CoverOlId, Work.Isbns, ICoverService.Size.Large);
        if (url is not null)
        {
            LargeImage = new LoadingUrl(url);
            return;
        }

        url = await coverService.GetCoverAny(Work.CoverOlId, Work.Isbns, ICoverService.Size.Medium);
        if (url is not null)
        {
            LargeImage = new LoadingUrl(url);
            return;
        }

        url = await coverService.GetCoverAny(Work.CoverOlId, Work.Isbns, ICoverService.Size.Small);
        LargeImage = new LoadingUrl(url, url is null);
    }

    private void UpdateRating()
    {
        if (Work.RatingsAverage is null) return;
        Rating = Work.RatingsAverage.Value;

        var count = Work.RatingsCount?.ToString("N0") ?? "Unknown amount of";
        RatingsCount = $"{Work.RatingsAverage:F} \u22c5 {count} ratings";
    }

    [ObservableProperty]
    private string _wantRate = "Auto";
    [ObservableProperty]
    private string _currentRate = "Auto";
    [ObservableProperty]
    private string _readedRate = "Auto";

    [ObservableProperty]
    private Work _work = default!;

    [ObservableProperty]
    private Bookshelf? _bookshelf;

    [ObservableProperty]
    private WorkDetails? _details;

    [ObservableProperty]
    private LoadingUrl _largeImage;

    [ObservableProperty]
    private double _rating = -1;

    [ObservableProperty]
    private string _ratingsCount = "There are no ratings";
}
