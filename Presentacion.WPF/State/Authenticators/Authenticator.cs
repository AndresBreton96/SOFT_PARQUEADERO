using System;
using System.Threading.Tasks;

namespace Presentacion.WPF.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        #region Constructor
        public Authenticator()
        {

        }

        #endregion

        #region Variables
        //private readonly IAuthenticationService _authenticationService; SERVICIO CUENTA

        public bool IsLoggedIn => false;// CurrentAccount != null;

        public event Action StateChanged;

        #endregion

        #region Metodos
        public async Task<bool> Login(string username, string password)
        {
            bool success = true;

            try
            {
                //CurrentAccount = await _authenticationService.Login(username, password);  METODO AUTENTICACION
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public void Logout()
        {
            //CurrentAccount = null;  METODO CIERRE SESION
        }

        #endregion
    }
}
