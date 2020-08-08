using System.Threading.Tasks;
using Transversales.Modelos;

namespace Negocio.Contratos.Users
{
    public interface IUsersAdministrator
    {
        SystemUsers ValidateLogIn(string username, string password);
    }
}
