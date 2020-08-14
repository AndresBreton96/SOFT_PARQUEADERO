using Presentacion.WPF.State.Authenticators;
using Presentacion.WPF.State.Navigators;
using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos.Exceptions;

namespace Presentacion.WPF.Commands
{
    public class LoginCommand : ICommand
    {
        #region Constructor
        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator authenticator, IRenavigator renavigator)
        {
            _loginViewModel = loginViewModel;
            _authenticator = authenticator;
            _renavigator = renavigator;
        }

        #endregion

        #region Variables
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _renavigator;

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Eventos
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                bool success = _authenticator.Login(_loginViewModel.Username, parameter.ToString());

                if (success)
                {
                    _renavigator.Renavigate();
                }
            }
            catch (UserNotFoundException userEx)
            {
                throw userEx;
            }
            catch (InvalidPasswordException passEx)
            {
                throw passEx;
            }
        }

        #endregion
    }
}
