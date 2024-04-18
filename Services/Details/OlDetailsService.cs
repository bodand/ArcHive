using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Data.Json;
using ArcHive.Model;

namespace ArcHive.Services.Details;

public class OlDetailsService(HttpClient httpClient) : IDetailsService
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    private const string RootUrl = "https://openlibrary.org";

    private class BookshelfPayload
    {
        [JsonPropertyName("counts")]
        public BookshelfDto Dto { get; set; } = null!;
    }

    public Task<Bookshelf?> BookshelfOf(string workOlid)
    {
        try
        {
            return GetBookshelfFromHttp(workOlid);
        }
        catch (HttpRequestException)
        {
        }
        catch (JsonException)
        {
        }

        return Task.FromResult<Bookshelf?>(null);
    }

    private async Task<Bookshelf?> GetBookshelfFromHttp(string workOlid)
    {
        var payload = await httpClient.GetFromJsonAsync<BookshelfPayload>($"{RootUrl}{workOlid}/bookshelves.json", _jsonOptions);
        if (payload is null) return null;

        return new Bookshelf(payload.Dto);
    }

    public async Task<WorkDetails?> GetDetailsOf(string workOlid)
    {
        try
        {
            return await GetDetailsFromHttp(workOlid);
        }
        catch (HttpRequestException)
        {
        }
        catch (JsonException)
        {
        }

        return null;
    }

    private async Task<WorkDetails?> GetDetailsFromHttp(string workOlid)
    {
        var payload = await httpClient.GetAsync($"{RootUrl}{workOlid}.json");
        if (!payload.IsSuccessStatusCode) return null;

        var jsonData = await new StreamReader(await payload.Content.ReadAsStreamAsync()).ReadToEndAsync();
        if (!JsonObject.TryParse(jsonData, out var json)) return null;
 
        json.TryGetValue("description", out var desc);
        if (desc is null) return null;

        switch (desc.ValueType)
        {
            case JsonValueType.Null:
            case JsonValueType.Boolean:
            case JsonValueType.Number:
            case JsonValueType.Array:
                return null;
            case JsonValueType.String:
                return new WorkDetails(new WorkDetailsDto() { Description = desc.GetString() });
            case JsonValueType.Object:
                var value = desc.GetObject()["value"];
                if (value is null or { ValueType: not JsonValueType.String }) return null;

                return new WorkDetails(new WorkDetailsDto() { Description = value.GetString() });
            default:
                throw new ArgumentOutOfRangeException(nameof(workOlid), "response generated invalid json type");
        }
    }
}
