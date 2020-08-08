using System.Threading.Tasks;
using Transversales.Modelos;

namespace Datos.Contratos
{
    public interface IUsersRepository : IBaseRepository<SystemUsers>
    {
        Task<SystemUsers> ValidateLogIn(string username, string password);
    }
}
