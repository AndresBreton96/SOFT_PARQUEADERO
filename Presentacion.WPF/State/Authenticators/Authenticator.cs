using Negocio.Contratos.Users;
using Presentacion.WPF.State.Accounts;
using System;
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

        //public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword)
        //{
        //    return await _authenticationService.Register(email, username, password, confirmPassword);
        //}
    }
}
