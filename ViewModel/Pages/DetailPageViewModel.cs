using System.ComponentModel;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Services.Covers;
using ArcHive.Services.Details;
using Autofac.Features.AttributeFilters;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.ViewModel.Pages;

/// <summary>
///     The viewmodel for the details page view.
/// </summary>
public sealed partial class DetailPageViewModel : ObservableObject
{
    /// <summary>
    ///     Constructor for the details viewmodel.
    ///     Initializes internal event handlers, and fields.
    /// </summary>
    /// <param name="coverService">
    ///     The cover service to use. Required to be "cached".
    /// </param>
    /// <param name="detailsService">
    ///     The details service to use.
    /// </param>
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

    /// <summary>
    ///     The rate by which to set the reading statistics' grid columns' for
    ///     the wants to read statistics to.
    /// </summary>
    [ObservableProperty]
    private string _wantRate = "*";

    /// <summary>
    ///     The rate by which to set the reading statistics' grid columns' for
    ///     the currently reading statistics to.
    /// </summary>
    [ObservableProperty]
    private string _currentRate = "*";

    /// <summary>
    ///     The rate by which to set the reading statistics' grid columns' for
    ///     the already read statistics to.
    /// </summary>
    [ObservableProperty]
    private string _readedRate = "*";

    /// <summary>
    ///     The book to show the details of.
    /// </summary>
    [ObservableProperty]
    private Work _work = default!;

    /// <summary>
    ///     The bookshelf extraneous data to show the as the details for the
    ///     book. Automatically loaded when a book object is assigned.
    /// </summary>
    [ObservableProperty]
    private Bookshelf? _bookshelf;

    /// <summary>
    ///     The extraneous data to show the as the details for the book.
    ///     Automatically loaded when a book object is assigned.
    /// </summary>
    [ObservableProperty]
    private WorkDetails? _details;

    /// <summary>
    ///     The loading-ring ready url for the large version of the cover of the
    ///     rendered book. Automatically loaded when a book object is assigned.
    /// </summary>
    [ObservableProperty]
    private LoadingUrl _largeImage;

    /// <summary>
    ///     The rating value of the book scaled for the rating component.
    /// </summary>
    [ObservableProperty]
    private double _rating = -1;

    /// <summary>
    ///     The label value for the label component of the book.
    /// </summary>
    [ObservableProperty]
    private string _ratingsCount = "There are no ratings";
}
