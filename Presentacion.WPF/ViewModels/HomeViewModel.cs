using Presentacion.WPF.State.Accounts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using Transversales.Modelos;

namespace Presentacion.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Constructor
        public HomeViewModel(IAccountStore accountStore)
        {
            _accountStore = accountStore;

			_username = _accountStore.CurrentUser.UserName;
        }

        #endregion

        #region Variables
        private readonly IAccountStore _accountStore;

		private string _username = "";
		public string Username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}

		#endregion

	}
}
