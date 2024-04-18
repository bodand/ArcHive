using System;
using System.Threading.Tasks;
using ArcHive.Model.Search;

namespace ArcHive.ViewModel;

public interface ISearchInitiator
{
    event Func<ISearchFields, Task> SearchInitiated;

    void SearchEnded();
}
