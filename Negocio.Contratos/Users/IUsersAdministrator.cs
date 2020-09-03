using System.Collections.Generic;
using Transversales.Modelos;

namespace Negocio.Contratos.Users
{
    public interface IUsersAdministrator
    {
        SystemUsers ValidateLogIn(string username, string password);

        IEnumerable<SystemUsers> SearchUsers(string searchParameter);

        SystemUsers GetUser(int id);

        SystemUsers GetFullUser(int id);

        void AddUser(SystemUsers user);

        void UpdateUser(SystemUsers user);

        IEnumerable<UsersMenu> LoadMenus();
    }
}
