using System;
using Transversales.Modelos;

namespace Presentacion.WPF.State.Authenticators
{
    public interface IAuthenticator
    {
        SystemUsers CurrentUser { get; }
        bool IsLoggedIn { get; }

        event Action StateChanged;

        //Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
        bool Login(string username, string password);
        void Logout();
    }
}
