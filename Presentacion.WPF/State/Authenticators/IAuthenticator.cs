using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Presentacion.WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        #region Variables
        //Account CurrentAccount { get; }
        bool IsLoggedIn { get; }

        event Action StateChanged;

        #endregion

        #region Metodos
        //Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
        Task<bool> Login(string username, string password);
        void Logout();

        #endregion
    }
}
