using System;
using System.Collections.Generic;
using Transversales.Modelos;

namespace Presentacion.WPF.State.Authenticators
{
    public enum RegistrationResult
    {
        Success,
        PasswordsDoNotMatch,
        EmailAlreadyExists,
        UsernameAlreadyExists
    }

    public interface IAuthenticator
    {
        SystemUsers CurrentUser { get; }
        bool IsLoggedIn { get; }

        event Action StateChanged;

        //Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword);
        bool Login(string username, string password);
        void Logout();

        RegistrationResult Register(string firstName, string lastName, string username, string password, string confirmPassword, IEnumerable<UsersMenu> menus);
    }
}
