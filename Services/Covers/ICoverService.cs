using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ArcHive.Services.Covers;

/// <summary>
///     A service interface for interacting with the OpenLibrary Covers API.
///     Retrieves URLs to the images for the books of the covers, which can be
///     found by ISBN or OlId.
/// </summary>
public interface ICoverService
{
    /// <summary>
    ///     Enum representing the sizes of covers available on the OpenLibrary
    ///     cover API.
    /// </summary>
    enum Size
    {
        Small, Medium, Large
    }

    /// <summary>
    ///     Retrieves a cover image from the current API wrapper, uses the OlId
    ///     then the ISBNs.
    /// </summary>
    /// <param name="olid">
    ///     The OlId of the book to get the cover of.
    /// </param>
    /// <param name="isbns">
    ///     A list of ISBN values for the book the get the cover of.
    /// </param>
    /// <param name="size">The size to get the cover in.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>The URL for the image, or null.</returns>
    public async Task<string?> GetCoverAny(string? olid,
        IEnumerable<string> isbns,
        Size size = Size.Medium,
        CancellationToken token = default)
    {
        try
        {
            return await CoverAnyImpl(olid, isbns, size, token);
        }
        catch (HttpRequestException) { }
        catch (TaskCanceledException) { }
        return null;
    }

    private async Task<string?> CoverAnyImpl(string? olid,
        IEnumerable<string> isbns,
        Size size,
        CancellationToken token)
    {
        if (olid is not null)
        {
            var img = await GetCoverUrlFromOlid(olid, size, token);
            if (img is not null) return img;
        }

        foreach (var isbn in isbns)
        {
            var img = await GetCoverUrlFromIsbn(isbn, size, token);
            if (img is not null) return img;
        }

        return null;
    }

    /// <summary>
    ///     Returns a book's cover specified by the book's OlId.
    /// </summary>
    /// <param name="olid">The OlId to search for.</param>
    /// <param name="size">The size to get the cover in.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>The URL for the image, or null.</returns>
    Task<string?> GetCoverUrlFromOlid(string olid, Size size = Size.Medium, CancellationToken token = default);

    /// <summary>
    ///     Returns a book's cover specified by the book's ISBN number.
    /// </summary>
    /// <param name="isbn">The ISBN to search for.</param>
    /// <param name="size">The size to get the cover in.</param>
    /// <param name="token">A cancellation token.</param>
    /// <returns>The URL for the image, or null.</returns>
    Task<string?> GetCoverUrlFromIsbn(string isbn, Size size = Size.Medium, CancellationToken token = default);
}
