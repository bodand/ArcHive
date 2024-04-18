using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.Model;

/// <summary>
///     A DTO used to communication with the details API of the OpenLibrary
///     servers.
/// </summary>
internal class WorkDetailsDto
{
    /// <summary>
    ///     A description for the work (book).
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}

/// <summary>
///     A model class containing extra fields for books that can be separately
///     requested.
/// </summary>
public partial class WorkDetails : ObservableObject
{
    /// <summary>
    ///     An optional description of the book in Markdown format.
    /// </summary>
    [ObservableProperty]
    private string? _description;

    /// <summary>
    ///     Creates a book detail object from the related DTO object.
    /// </summary>
    /// <param name="dto">The DTO to take the data from.</param>
    internal WorkDetails(WorkDetailsDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.Description)) Description = dto.Description;
    }
}
