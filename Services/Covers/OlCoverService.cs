using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ArcHive.Services.Covers;

internal class OlCoverService(
    HttpClient httpClient
) : ICoverService
{
    private class CoverPayload
    {
        [JsonPropertyName("olid")]
        public string OlId { get; init; } = null!;

        [JsonPropertyName("deleted")]
        public bool Deleted { get; init; }
    }

    private const string RootUrl = "https://covers.openlibrary.org/b/";

    public async Task<string?> GetCoverUrlFromIsbn(string isbn,
        ICoverService.Size size = ICoverService.Size.Medium,
        CancellationToken token = default)
    {
        var response = await httpClient.GetAsync($"{RootUrl}isbn/{isbn}.json", token);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        await using var stream = await response.Content.ReadAsStreamAsync(token);
        var payload = await JsonSerializer.DeserializeAsync<CoverPayload>(stream, new JsonSerializerOptions(), token);

        if (payload is null or { OlId: null } or { Deleted: true }) return null;

        return $"{RootUrl}olid/{payload.OlId}-{EncodeSizeToUrl(size)}.jpg";
    }

    private static char EncodeSizeToUrl(ICoverService.Size size)
    {
        return size switch
        {
            ICoverService.Size.Small => 'S',
            ICoverService.Size.Medium => 'M',
            ICoverService.Size.Large => 'L',
            _ => throw new ArgumentException("invalid size specification for OpenLibraryCoverService", nameof(size))
        };
    }

    public async Task<string?> GetCoverUrlFromOlid(string olid,
        ICoverService.Size size = ICoverService.Size.Medium,
        CancellationToken token = default)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Head,
                $"{RootUrl}olid/{olid}-{EncodeSizeToUrl(size)}.jpg?default=false");
            var ops = await httpClient.SendAsync(request, token);

            if (ops is { IsSuccessStatusCode: false }) return null;
            return $"{RootUrl}olid/{olid}-{EncodeSizeToUrl(size)}.jpg";
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
