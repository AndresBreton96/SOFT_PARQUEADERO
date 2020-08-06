using Presentacion.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentacion.WPF.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        public ViewModelFactory(CreateViewModel<LoginViewModel> createLoginViewModel, 
                                CreateViewModel<HomeViewModel> createHomeViewModel)
        {
            _createLoginViewModel = createLoginViewModel;
            _createHomeViewModel = createHomeViewModel;
        }

        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;

        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }

        }
    }
}
