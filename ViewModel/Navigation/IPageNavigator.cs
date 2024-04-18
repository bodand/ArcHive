using ArcHive.Model;

namespace ArcHive.ViewModel.Navigation;

public interface IPageNavigator
{
    void GoToDetails(Work work);
    void GoBack();
}
