using Presentacion.WPF.Commands;
using Presentacion.WPF.State.Accounts;
using Presentacion.WPF.State.Navigators;
using Presentacion.WPF.ViewModels.Factories;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Presentacion.WPF.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        #region Constructor
        public HomeViewModel(INavigator navigator, IViewModelFactory viewModelFactory, IAccountStore accountStore)
        {
            _accountStore = accountStore;
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;

            _username = _accountStore.CurrentUser.UserName;
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);

            var user = _accountStore.CurrentUser;

            ModifyPrices = user.Menus.Any(x => x.MenuView.Equals("ModifyPricesView") && x.Permission) ? Visibility.Visible : Visibility.Hidden;
            RegisterEntrance = user.Menus.Any(x => x.MenuView.Equals("RegisterEntryView") && x.Permission) ? Visibility.Visible : Visibility.Hidden;
            RegisterDeparture = user.Menus.Any(x => x.MenuView.Equals("RegisterDepartureView") && x.Permission) ? Visibility.Visible : Visibility.Hidden;
            SearchTickets = user.Menus.Any(x => x.MenuView.Equals("SearchTicketsView") && x.Permission) ? Visibility.Visible : Visibility.Hidden;
            SearchBills = user.Menus.Any(x => x.MenuView.Equals("SearchBillsView") && x.Permission) ? Visibility.Visible : Visibility.Hidden;
            ModifyUsers = user.Menus.Any(x => x.MenuView.Equals("UsersView") && x.Permission) ? Visibility.Visible : Visibility.Hidden;
        }

        #endregion

        #region Variables
        private readonly IViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;
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

        public ICommand UpdateCurrentViewModelCommand { get; }

        public Visibility ModifyPrices { get; set; }
        public Visibility RegisterEntrance { get; set; }
        public Visibility RegisterDeparture { get; set; }
        public Visibility SearchTickets { get; set; }
        public Visibility SearchBills { get; set; }
        public Visibility ModifyUsers { get; set; }

        #endregion

    }
}
