using System.Threading.Tasks;
using ArcHive.Model;

namespace ArcHive.Services.Details;

public interface IDetailsService
{
    Task<Bookshelf?> BookshelfOf(string workOlid);

    Task<WorkDetails?> GetDetailsOf(string workOlid);
}
