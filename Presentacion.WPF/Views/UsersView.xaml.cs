using MaterialDesignThemes.Wpf;
using Presentacion.WPF.Dialogs.ViewModels;
using Presentacion.WPF.Dialogs.Views;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Transversales.Modelos;
using Transversales.Utilitarios.Enums;
using Transversales.Utilitarios.Tools;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        #region Constructor
        public UsersView()
        {
            InitializeComponent();
        }

        #endregion

        #region Variables
        #region Commands
        public static readonly DependencyProperty AddUserCommandProperty =
            DependencyProperty.Register("AddUserCommand", typeof(ICommand), typeof(UsersView), new PropertyMetadata(null));

        public ICommand AddUserCommand
        {
            get { return (ICommand)GetValue(AddUserCommandProperty); }
            set { SetValue(AddUserCommandProperty, value); }
        }

        #endregion

        private ProcessEnum _currentProcess;
        private ProcessEnum CurrentProcess
        {
            get
            {
                return _currentProcess;
            }
            set
            {
                _currentProcess = value;
                switch (_currentProcess)
                {
                    case ProcessEnum.Adding:
                    case ProcessEnum.Modifying:
                        FirstNameTxtBox.IsEnabled = true;
                        LastNameTxtBox.IsEnabled = true;
                        UserNameTxtBox.IsEnabled = true;
                        PasswordTxtBox.IsEnabled = true;
                        PasswordConfirmationTxtBox.IsEnabled = true;
                        FirstNameTxtBox.Focus();

                        SelectionPnl.Visibility = Visibility.Hidden;
                        ChangesPnl.Visibility = Visibility.Visible;

                        ((UsersViewModel)DataContext).SearchMenusResultSymbol = ((UsersViewModel)DataContext)._usersAdministrator.LoadMenus();
                        break;
                    case ProcessEnum.Nothing:
                    default:
                        FirstNameTxtBox.IsEnabled = false;
                        LastNameTxtBox.IsEnabled = false;
                        UserNameTxtBox.IsEnabled = false;
                        PasswordTxtBox.IsEnabled = false;
                        PasswordConfirmationTxtBox.IsEnabled = false;

                        SelectionPnl.Visibility = Visibility.Visible;
                        ChangesPnl.Visibility = Visibility.Hidden;

                        ((UsersViewModel)DataContext).SearchMenusResultSymbol = new List<UsersMenu>();
                        break;
                }
            }
        }

        #endregion

        #region Events
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            StartUser();
        }

        private void ModifyBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenSearchUser();
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordTxtBox.IsEnabled)
            {
                PasswordTxtBox.Password = "";
                PasswordConfirmationTxtBox.Password = "";
                PasswordTxtBox.IsEnabled = false;
                PasswordConfirmationTxtBox.IsEnabled = false;
            }
            else
            {
                PasswordTxtBox.IsEnabled = true;
                PasswordConfirmationTxtBox.IsEnabled = true;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveUser();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelChanges();
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        #endregion

        #region Methods
        private void StartUser()
        {
            try
            {
                CurrentProcess = ProcessEnum.Adding;
            }
            catch (Exception ex)
            {
                CurrentProcess = ProcessEnum.Nothing;
                MessageBox.Show(ex.Message);
            }
        }

        private async void OpenSearchUser()
        {
            try
            {
                var view = new UserSearchDialog
                {
                    DataContext = new UserSearchDialogViewModel(((UsersViewModel)DataContext)._usersAdministrator)
                };

                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                if (result is null) return;

                var user = ((UserSearchDialogViewModel)view.DataContext).SelectedUser;

                user.Menus = ((UsersViewModel)DataContext)._usersAdministrator.GetFullUser(user.UserId).Menus;

                ((UsersViewModel)DataContext).User = user;

                UserId.Text = user.UserId.ToString();
                FirstNameTxtBox.Text = user.FirstName;
                LastNameTxtBox.Text = user.LastName;
                UserNameTxtBox.Text = user.UserName;

                CurrentProcess = ProcessEnum.Modifying;

                ((UsersViewModel)DataContext).SearchMenusResultSymbol = user.Menus;

                PasswordTxtBox.IsEnabled = false;
                PasswordConfirmationTxtBox.IsEnabled = false;
                ChangePasswordBtn.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ha ocurrido un inconveniente.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveUser()
        {
            try
            {
                if (CurrentProcess == ProcessEnum.Nothing || !ValidateData()) return;

                switch (CurrentProcess)
                {
                    case ProcessEnum.Adding:
                    case ProcessEnum.Modifying:
                        var user = new SystemUsers()
                        {
                            UserId = Convert.ToInt32(UserId.Text),
                            FirstName = FirstNameTxtBox.Text,
                            LastName = LastNameTxtBox.Text,
                            UserName = UserNameTxtBox.Text,
                            Password = PasswordTxtBox.Password,
                            DateJoined = DateTime.Now,
                            Menus = ((UsersViewModel)DataContext).SearchMenusResultSymbol
                        };

                        if (AddUserCommand != null)
                            AddUserCommand.Execute(user);
                        break;
                }

                CancelChanges();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidateData()
        {
            try
            {
                if (string.IsNullOrEmpty(FirstNameTxtBox.Text))
                {
                    MessageBox.Show("Por favor ingrese el nombre del usuario.");
                    FirstNameTxtBox.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(LastNameTxtBox.Text))
                {
                    MessageBox.Show("Por favor ingrese el apellido del usuario.");
                    LastNameTxtBox.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(UserNameTxtBox.Text))
                {
                    MessageBox.Show("Por favor ingrese un usuario.");
                    UserNameTxtBox.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(PasswordTxtBox.Password) && CurrentProcess == ProcessEnum.Adding)
                {
                    MessageBox.Show("Por favor ingrese una contraseña.");
                    PasswordTxtBox.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(PasswordConfirmationTxtBox.Password) && CurrentProcess == ProcessEnum.Adding)
                {
                    MessageBox.Show("Por favor ingrese una confirmación de contraseña.");
                    PasswordConfirmationTxtBox.Focus();
                    return false;
                }

                if (!PasswordTxtBox.Password.Equals(PasswordConfirmationTxtBox.Password))
                {
                    MessageBox.Show("Las contraseñas deben coincidir.");
                    PasswordConfirmationTxtBox.Focus();
                    return false;
                }

                if (ValidationMethods.IsUsername(UserNameTxtBox.Text))
                {
                    MessageBox.Show("El nombre de usuario contiene caracteres no permitidos.");
                    UserNameTxtBox.Focus();
                    return false;
                }

                var errorMessage = "";
                if (!string.IsNullOrEmpty(PasswordConfirmationTxtBox.Password) && !ValidationMethods.ValidatePassword(PasswordTxtBox.Password, out errorMessage))
                {
                    MessageBox.Show(errorMessage);
                    PasswordTxtBox.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private void CancelChanges()
        {
            try
            {
                UserId.Text = "0";
                FirstNameTxtBox.Text = "";
                LastNameTxtBox.Text = "";
                UserNameTxtBox.Text = "";
                PasswordTxtBox.Password = "";
                PasswordConfirmationTxtBox.Password = "";
                ChangePasswordBtn.Visibility = Visibility.Hidden;
                CurrentProcess = ProcessEnum.Nothing;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        #endregion
    }
}
