using System.Collections.Generic;
using System.Threading.Tasks;
using ArcHive.Model;
using ArcHive.Model.Search;

namespace ArcHive.Services.Search;

public interface ISearchService
{
    IAsyncEnumerable<Work> FindWorks(ISearchFields fields);
}
