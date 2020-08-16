using Presentacion.WPF.State.Navigators;
using Presentacion.WPF.ViewModels.Factories;
using System;
using System.Windows.Input;

namespace Presentacion.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        #region Constructor
        public UpdateCurrentViewModelCommand(INavigator navigator, IViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        #endregion

        #region Variables
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly IViewModelFactory _viewModelFactory;

        #endregion

        #region Events
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }

        #endregion
    }
}
