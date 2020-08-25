using Negocio.Contratos.Users;
using Presentacion.WPF.State.Accounts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;

namespace Presentacion.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        #region Constructor
        public Authenticator(IUsersAdministrator authenticationService, IAccountStore accountStore)
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
        }

        #endregion

        #region Variables
        private readonly IUsersAdministrator _authenticationService;
        private readonly IAccountStore _accountStore;

        public SystemUsers CurrentUser
        {
            get
            {
                return _accountStore.CurrentUser;
            }
            private set
            {
                _accountStore.CurrentUser = value;
                StateChanged?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentUser != null;

        public event Action StateChanged;

        #endregion

        #region Methods
        public bool Login(string username, string password)
        {
            bool success = true;

            try
            {
                CurrentUser = _authenticationService.ValidateLogIn(username, password);
            }
            catch (UserNotFoundException userEx)
            {
                success = false;
                throw userEx;
            }
            catch (InvalidPasswordException passEx)
            {
                success = false;
                throw passEx;
            }

            return success;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        #endregion

        public RegistrationResult Register(string firstName, string lastName, string username, string password, string confirmPassword, IEnumerable<UsersMenu> menus)
        {
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            var usernameAccount = _authenticationService.SearchUsers(username);
            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                SystemUsers user = new SystemUsers()
                {
                    UserId = 0,
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = username,
                    Password = password,
                    DateJoined = DateTime.Now,
                    Menus = menus
                };

                _authenticationService.AddUser(user);
            }

            return result;
        }
    }
}
