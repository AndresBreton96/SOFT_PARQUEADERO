using Negocio.Contratos.Users;
using Presentacion.WPF.Commands.Users;
using Presentacion.WPF.State.Authenticators;
using System.Collections.Generic;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        #region Constructor
        public UsersViewModel(IUsersAdministrator usersAdministrator, IAuthenticator authenticator)
        {
            _usersAdministrator = usersAdministrator;
            _authenticator = authenticator;

            AddUserCommand = new AddUserCommand(this, _usersAdministrator);
        }

        #endregion

        #region Variables
        public readonly IUsersAdministrator _usersAdministrator;
        private readonly IAuthenticator _authenticator;

        public SystemUsers User;
        private IEnumerable<UsersMenu> _searchMenusResultSymbol = new List<UsersMenu>();
        public IEnumerable<UsersMenu> SearchMenusResultSymbol
        {
            get
            {
                return _searchMenusResultSymbol;
            }
            set
            {
                _searchMenusResultSymbol = value;
                OnPropertyChanged(nameof(SearchMenusResultSymbol));
            }
        }

        #region Commands
        public ICommand AddUserCommand { get; set; }

        #endregion

        #endregion
    }
}
