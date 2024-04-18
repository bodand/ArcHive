namespace ArcHive.Model.Search;

/// <summary>
///     Interface representing an abstract type that can, from itself, generate
///     a valid query string for OpenLibrary.
/// </summary>
public interface ISearchFields
{
    /// <summary>
    ///     Generates an OpenLibrary query string from the current object.
    /// </summary>
    /// <returns>An OpenLibrary query string for this object.</returns>
    string ToQueryString();
}
