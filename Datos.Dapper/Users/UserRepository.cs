using Datos.Contratos;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Transversales.Modelos;

namespace Datos.Dapper.Users
{
    public class UserRepository : BaseRepository<SystemUsers>, IUsersRepository
    {
        #region Constructor
        public UserRepository()
        {
        }

        #endregion

        #region Metodos
        public async Task<SystemUsers> ValidateLogIn(string username, string password)
        {
            var response = await ExecuteSP("ValidateLogIn", new { username, password });
            return response.FirstOrDefault();
        }

        #endregion

    }
}
