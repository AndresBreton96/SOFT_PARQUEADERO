using Presentacion.WPF.State.Navigators;

namespace Presentacion.WPF.ViewModels.Factories
{
    public interface IViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
