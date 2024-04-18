using System.Collections.Generic;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Model.Search;

namespace ArcHive.Services.Search;

/// <summary>
///     A service interface that provides searching through the OpenLibrary
///     query API.
/// </summary>
public interface ISearchService
{
    /// <summary>
    ///     Executes a query on the OpenLibrary query API.
    /// </summary>
    /// <param name="fields">A fields object representing a query.</param>
    /// <returns>The list of results returned by OpenLibrary.</returns>
    IAsyncEnumerable<Work> FindWorks(ISearchFields fields);
}
