using System.Collections.Generic;
using Transversales.Modelos;

namespace Negocio.Contratos.Users
{
    public interface IUsersAdministrator
    {
        SystemUsers ValidateLogIn(string username, string password);

        IEnumerable<SystemUsers> SearchUsers(string searchParameter);

        SystemUsers GetUser(int id);

        void AddUser(SystemUsers user);

        IEnumerable<UsersMenu> LoadMenus();
    }
}
