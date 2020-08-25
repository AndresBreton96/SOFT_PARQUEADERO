using Negocio.Contratos.Users;
using Presentacion.WPF.State.Authenticators;
using System.Collections.Generic;
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

        #endregion
    }
}
