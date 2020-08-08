using System;
using Transversales.Modelos;

namespace Presentacion.WPF.State.Accounts
{
    public class AccountStore : IAccountStore
    {
        private SystemUsers _currentUser;
        public SystemUsers CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;

    }
}
