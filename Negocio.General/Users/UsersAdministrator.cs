using Datos.Contratos;
using Microsoft.AspNet.Identity;
using Negocio.Contratos.Users;
using System.Linq;
using System.Threading.Tasks;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;

namespace Negocio.General.Users
{
    public class UsersAdministrator : IUsersAdministrator
    {
        #region Constructor
        public UsersAdministrator(IUsersRepository repositorio, IPasswordHasher passwordHasher)
        {
            _repositorio = repositorio;
            _passwordHasher = passwordHasher;
        }

        #endregion

        #region Variables
        private readonly IUsersRepository _repositorio;
        private readonly IPasswordHasher _passwordHasher;

        #endregion

        #region Metodos
        public SystemUsers ValidateLogIn(string username, string password)
        {
            var user = _repositorio.GetAll($"WHERE UserName = '{username}'").FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException(username);

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(user.Password, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }
            return user;
        }

        #endregion
    }
}
