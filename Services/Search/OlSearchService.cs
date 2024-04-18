using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Model.Search;

namespace ArcHive.Services.Search;

public class OlSearchService(
    HttpClient httpClient
) : ISearchService
{
    private class ServicePayload
    {
        [JsonPropertyName("numFound")] public int NumFound { get; set; }
        [JsonPropertyName("num_found")] public int NumFound2 { get; set; }
        public List<WorkDto> Docs { get; set; } = [];
        [JsonPropertyName("q")] public string Query { get; set; } = default!;
        public int Offset { get; set; }
        [JsonPropertyName("numFoundExact")] public bool IsCountExact { get; set; }
    }

    private const int LimitSteps = 20;
    private const string RootUrl = "https://openlibrary.org/search.json";

    public async IAsyncEnumerable<Work> FindWorks(ISearchFields fields)
    {
        var offset = 0;
        var queryString = fields.ToQueryString();
        var toRead = LimitSteps;
        var read = 0;
        var chunkScaling = 1;
        int? total = null;

        do
        {
            toRead = LimitSteps * chunkScaling;
            var url = new Uri($"{RootUrl}?q={queryString}&fields=&limit={toRead}&offset={offset}&mode=everything");

            var payload = await httpClient.GetFromJsonAsync<ServicePayload>(url);
            total ??= payload?.NumFound;
            read = payload?.Docs.Count ?? 0;
            offset += read;

            foreach (var entry in payload?.Docs?.Select(x => new Work(x)) ?? [])
            {
                yield return entry;
            }

        } while (total is null ? read == toRead : offset + read < total);
    }
}