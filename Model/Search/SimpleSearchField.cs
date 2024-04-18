using CommunityToolkit.Mvvm.ComponentModel;

namespace ArcHive.Model.Search;

/// <summary>
///     A basic search field that generates a query to match against anything it
///     can find.
/// </summary>
public partial class SimpleSearchField : ObservableObject, ISearchFields
{
    /// <summary>
    ///     The value to find a book by. Could be title, author, subject, or
    ///     anything else related to the book.
    /// </summary>
    [ObservableProperty]
    private string? _query;

    /// <inheritdoc/>
    public string ToQueryString()
    {
        return Query?.Replace(' ', '+') ?? string.Empty;
    }
}
