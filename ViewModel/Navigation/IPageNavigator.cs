using ArcHive.Model;

namespace ArcHive.ViewModel.Navigation;

/// <summary>
///     An interface representing functionality for classes that can be
///     instructed to start a navigation action.
/// </summary>
public interface IPageNavigator
{
    /// <summary>
    ///     Specifies that the UI should go a page that renders the details of
    ///     the <see cref="Work"/> object specified.
    /// </summary>
    /// <param name="work">The book to show the detils of.</param>
    void GoToDetails(Work work);

    /// <summary>
    ///     Go back in the navigation stack.
    /// </summary>
    void GoBack();
}
