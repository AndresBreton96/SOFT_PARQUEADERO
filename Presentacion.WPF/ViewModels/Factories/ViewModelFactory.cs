using Presentacion.WPF.State.Navigators;
using System;

namespace Presentacion.WPF.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        #region Constructor
        public ViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel, CreateViewModel<LoginViewModel> createLoginViewModel, CreateViewModel<ModifyPricesViewModel> createModifyPricesViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createModifyPricesViewModel = createModifyPricesViewModel;
        }

        #endregion

        #region Variables
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<ModifyPricesViewModel> _createModifyPricesViewModel;

        #endregion

        #region Metodos
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                case ViewType.ModifyPrices:
                    return _createModifyPricesViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }

        #endregion

    }
}
