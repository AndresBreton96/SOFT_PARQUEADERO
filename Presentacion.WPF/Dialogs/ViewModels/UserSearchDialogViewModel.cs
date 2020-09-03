using Negocio.Contratos.Users;
using Presentacion.WPF.Commands.Users;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.Dialogs.ViewModels
{
    public class UserSearchDialogViewModel : ViewModelBase
    {
        #region Constructor
        public UserSearchDialogViewModel(IUsersAdministrator usersAdministrator)
        {
            _usersAdministrator = usersAdministrator;

            SearchUserCommand = new SearchUsersCommand(this, _usersAdministrator);
        }

        #endregion

        #region Variables
        public readonly IUsersAdministrator _usersAdministrator;

        public SystemUsers SelectedUser { get; set; }

        private IEnumerable<SystemUsers> _searchUsersResultSymbol = new List<SystemUsers>();
        public IEnumerable<SystemUsers> SearchUsersResultSymbol
        {
            get
            {
                return _searchUsersResultSymbol;
            }
            set
            {
                _searchUsersResultSymbol = value;
                OnPropertyChanged(nameof(SearchUsersResultSymbol));
            }
        }

        #endregion

        #region Commands
        public ICommand SearchUserCommand { get; set; }

        #endregion
    }
}
