using Presentacion.WPF.Commands;
using Presentacion.WPF.State.Authenticators;
using Presentacion.WPF.State.Navigators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Presentacion.WPF.ViewModels
{
	public class LoginViewModel : ViewModelBase
	{
		#region Constructor
		public LoginViewModel(IAuthenticator authenticator, IRenavigator renavigator)
		{
			LoginCommand = new LoginCommand(this, authenticator, renavigator);
		}

		#endregion

		#region Variables
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

		public ICommand LoginCommand { get; }

		#endregion

	}
}
