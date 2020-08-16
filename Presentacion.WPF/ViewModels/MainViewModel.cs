using Presentacion.WPF.Commands;
using Presentacion.WPF.Controls;
using Presentacion.WPF.State.Accounts;
using Presentacion.WPF.State.Authenticators;
using Presentacion.WPF.State.Navigators;
using Presentacion.WPF.ViewModels.Factories;
using Presentacion.WPF.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructor
        public MainViewModel(INavigator navigator, IViewModelFactory viewModelFactory, IAuthenticator authenticator, IAccountStore accountStore)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            _authenticator = authenticator;
            _accountStore = accountStore;

            _navigator.StateChanged += Navigator_StateChanged;
            _authenticator.StateChanged += Authenticator_StateChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        #endregion

        #region Variables
        private readonly IViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;
        private readonly IAuthenticator _authenticator;
        private readonly IAccountStore _accountStore;

        public bool IsLoggedIn => _authenticator.IsLoggedIn;

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

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        private ObservableCollection<MenuItem> _menuItems;
        private MenuItem _selectedItem;
        private int _selectedIndex;

        public ObservableCollection<MenuItem> MenuItems
        {
            get => _menuItems;
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }

        public MenuItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value == null || value.Equals(_selectedItem)) return;

                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                OpenNewMenu(SelectedItem);
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        public ICommand UpdateCurrentViewModelCommand { get; }

        #endregion

        #region Events
        private void Authenticator_StateChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));

            if (IsLoggedIn)
            {
                var menus = _accountStore.CurrentUser.Menus;
                MenuItems = GenerateMenuItems(menus);
            }
        }

        private void Navigator_StateChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        #endregion

        #region Methods
        private ObservableCollection<MenuItem> GenerateMenuItems(IEnumerable<UsersMenu> menus)
        {
            var menusReturn = new ObservableCollection<MenuItem>
            {
                new MenuItem("Home", new HomeView { DataContext = new HomeViewModel(_accountStore) }, ViewType.Home)
            };

            foreach (var menu in menus)
            {
                var flag = false;
                ViewType viewType;
                object content;
                switch (menu.MenuView)
                {
                    case "ModifyPricesView":
                        content = new ModifyPricesView();
                        viewType = ViewType.ModifyPrices;
                        flag = true;
                        break;
                    case "RegisterEntryView":
                        content = new RegisterEntryView();
                        viewType = ViewType.RegisterEntry;
                        flag = true;
                        break;
                    default:
                        content = new HomeView();
                        viewType = ViewType.Home;
                        flag = true;
                        break;
                }

                if (flag)
                {
                    var menuItem = new MenuItem(menu.MenuName, content, viewType);
                    menusReturn.Add(menuItem);
                }
            }
            return menusReturn;
        }

        private void OpenNewMenu(MenuItem menu)
        {
            UpdateCurrentViewModelCommand.Execute(menu.ViewType);
        }

        #endregion
    }
}
