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

                        AddBtn.Visibility = Visibility.Hidden;
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

                        AddBtn.Visibility = Visibility.Visible;
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

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
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

        #endregion
    }
}
