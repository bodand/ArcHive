using System;
using System.Threading.Tasks;
using ArcHive.Model.Search;

namespace ArcHive.ViewModel;

/// <summary>
///     An interface that is used to call into functions that need to listen
///     to events for starting searches, and call back into the initiators to
///     tell them about their completions.
/// </summary>
public interface ISearchInitiator
{
    /// <summary>
    ///     An event that is called by initiators whenever a search is to be
    ///     started. The parameter is the search fields object by which the
    ///     query is to be performed.
    /// </summary>
    event Func<ISearchFields, Task> SearchInitiated;

    /// <summary>
    ///     A callback into the initiator to notify it of the search's
    ///     conclusion.
    /// </summary>
    void SearchEnded();
}
