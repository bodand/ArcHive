using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using ArcHive.Services.Search;
using Autofac.Features.AttributeFilters;

namespace ArcHive.Services.Covers;

/// <summary>
///     A
/// </summary>
/// <param name="httpClient"></param>
/// <param name="coverService"></param>
public class CachingCoverService(
    HttpClient httpClient,
    [KeyFilter("raw")] ICoverService coverService
) : ICoverService
{
    private readonly StorageFolder _appTempDir = ApplicationData.Current.TemporaryFolder;

    public async Task<string?> GetCoverUrlFromOlid(string olid,
        ICoverService.Size size = ICoverService.Size.Medium,
        CancellationToken token = default)
    {
        var cached = await TryGetFromCache(olid, size);
        if (cached is not null) return cached;

        return await DelegateOlidAndCacheResult(olid, size, token);
    }

    private async Task<string?> TryGetFromCache(string name, ICoverService.Size size)
        => (await _appTempDir.TryGetItemAsync(GetFileName(name, size, ".jpg")))?.Path;

    private async Task<string?> DelegateOlidAndCacheResult(string olid,
        ICoverService.Size size,
        CancellationToken token)
    {
        var url = await coverService.GetCoverUrlFromOlid(olid, size, token);
        if (url is null) return null;

        return await LoadAndWriteToCache(url, GetFileName(olid, size, ".jpg"), token);
    }

    private async Task<string?> LoadAndWriteToCache(string url,
        string cache,
        CancellationToken token)
    {
        try
        {
            return await LoadAndWriteToCacheRaw(url, cache, token);
        }
        catch (IOException)
        {
            return url;
        }
        catch (COMException)
        {
            return url;
        }
    }

    private async Task<string?> LoadAndWriteToCacheRaw(string url, string cache, CancellationToken token)
    {
        var request = httpClient.GetAsync(url, token);

        var file = await _appTempDir.CreateFileAsync(cache);
        await using var outStream = await file.OpenStreamForWriteAsync();

        await using var payload = await AwaitResponse(request, token);
        if (payload is null) return null;

        await payload.CopyToAsync(outStream, token);

        return file.Path;
    }


    private static async Task<Stream?> AwaitResponse(Task<HttpResponseMessage> request, CancellationToken token)
    {
        var response = await request;
        if (response is { IsSuccessStatusCode: false }) return null;

        return await response.Content.ReadAsStreamAsync(token);
    }

    public async Task<string?> GetCoverUrlFromIsbn(string isbn,
        ICoverService.Size size = ICoverService.Size.Medium,
        CancellationToken token = default)
    {
        var cached = await TryGetFromCache(isbn, size);
        if (cached is not null) return cached;

        return await DelegateIsbnAndCacheResult(isbn, size, token);
    }

    private async Task<string?> DelegateIsbnAndCacheResult(string isbn,
        ICoverService.Size size,
        CancellationToken token)
    {
        var url = await coverService.GetCoverUrlFromIsbn(isbn, size, token);
        if (url is null) return null;

        return await LoadAndWriteToCache(url, GetFileName(isbn, size, ".jpg"), token);
    }

    private static string GetFileName(string id, ICoverService.Size size, string ext) => $"{id}-{size:G}{ext}";
}
