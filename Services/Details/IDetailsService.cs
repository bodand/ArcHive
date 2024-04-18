using System.Threading.Tasks;
using ArcHive.Model;

namespace ArcHive.Services.Details;

/// <summary>
///     A service interface for retrieving additional details about a given
///     resource, using the OpenLibrary API endpoints.
/// </summary>
public interface IDetailsService
{
    /// <summary>
    ///     Retrieves statistical data about users who have this book in their
    ///     bookshelves, and at what stage.
    /// </summary>
    /// <param name="workOlid">The OlId of the book, including the <c>/work/</c> prefix.</param>
    /// <returns>The bookshelf object or null, if it could not be retrieved.</returns>
    Task<Bookshelf?> BookshelfOf(string workOlid);

    /// <summary>
    ///     Retrieves extraneous data about books that is not provided in the
    ///     normal query endoint.
    /// </summary>
    /// <param name="workOlid">The OlId of the book, including the <c>/work/</c> prefix.</param>
    /// <returns>The details object or null, if it could not be retrieved.</returns>
    Task<WorkDetails?> GetDetailsOf(string workOlid);
}
