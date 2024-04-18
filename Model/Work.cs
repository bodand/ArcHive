using System.Collections.Generic;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.Model;

internal class WorkDto
{
    [JsonPropertyName("author_name")]
    public List<string> AuthorNames { get; set; } = [];

    [JsonPropertyName("key")]
    public string Olid { get; set; } = default!;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("cover_edition_key")]
    public string? CoverOlId { get; set; }

    [JsonPropertyName("first_publish_year")]
    public int? FirstPublishYear { get; set; }

    [JsonPropertyName("ebook_access")]
    public string? EbookAccess { get; set; }

    [JsonPropertyName("ratings_average")]
    public double? RatingsAverage { get; set; }

    [JsonPropertyName("ratings_count")]
    public int? RatingsCount { get; set; }

    [JsonPropertyName("subject")]
    public List<string> Subjects { get; set; } = [];

    [JsonPropertyName("isbn")]
    public List<string> Isbns { get; set; } = [];
}

public partial class Work : ObservableObject
{
    [ObservableProperty]
    private string _olId = null!;
    [ObservableProperty]
    private string? _author;
    [ObservableProperty]
    private string? _title;
    [ObservableProperty]
    private int? _firstPublishedYear;
    [ObservableProperty]
    private string? _ebookAccess;
    [ObservableProperty]
    private double? _ratingsAverage;
    [ObservableProperty]
    private int? _ratingsCount;
    [ObservableProperty]
    private List<string> _subjects = [];
    [ObservableProperty]
    private List<string> _isbns = [];

    [ObservableProperty]
    private string? _coverOlId;
    [ObservableProperty]
    private string? _coverUrl;

    public bool Borrowable => "borrowable".Equals(EbookAccess);

    internal Work(WorkDto dto)
    {
        OlId = dto.Olid;
        Author = string.Join(", ", dto.AuthorNames); // todo properly handle multi-author
        Title = dto.Title;
        FirstPublishedYear = dto.FirstPublishYear;
        EbookAccess = dto.EbookAccess;
        RatingsAverage = dto.RatingsAverage;
        RatingsCount = dto.RatingsCount;
        Subjects = dto.Subjects;
        Isbns = dto.Isbns;
        CoverOlId = dto.CoverOlId;
    }
}
