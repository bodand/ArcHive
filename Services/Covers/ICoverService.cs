using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ArcHive.Services.Covers;

public interface ICoverService
{
    enum Size
    {
        Small, Medium, Large
    }

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

    Task<string?> GetCoverUrlFromOlid(string olid, Size size = Size.Medium, CancellationToken token = default);

    Task<string?> GetCoverUrlFromIsbn(string isbn, Size size = Size.Medium, CancellationToken token = default);
}
