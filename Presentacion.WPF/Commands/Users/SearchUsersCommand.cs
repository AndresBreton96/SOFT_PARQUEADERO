using Negocio.Contratos.Users;
using Presentacion.WPF.Dialogs.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

namespace Presentacion.WPF.Commands.Users
{
    public class SearchUsersCommand : ICommand
    {
        #region Constructor
        public SearchUsersCommand(UserSearchDialogViewModel usersViewModel, IUsersAdministrator usersAdministrator)
        {
            _usersViewModel = usersViewModel;
            _usersAdministrator = usersAdministrator;
        }

        #endregion

        #region Variables
        private readonly UserSearchDialogViewModel _usersViewModel;
        private readonly IUsersAdministrator _usersAdministrator;

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Events
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (parameter is string)
                {
                    var users = _usersAdministrator.SearchUsers((string)parameter);
                    _usersViewModel.SearchUsersResultSymbol = users;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ha ocurrido un inconveniente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }
}
