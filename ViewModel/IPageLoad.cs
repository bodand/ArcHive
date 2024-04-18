using ArcHive.ViewModel.Navigation;

namespace ArcHive.ViewModel;

/// <summary>
///     An interface that provides the functionality to be a call-back for
///     loaded pages.
/// </summary>
public interface IPageLoad : IPageNavigator, ISearchInitiator;
