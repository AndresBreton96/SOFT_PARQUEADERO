using MaterialDesignThemes.Wpf;
using Presentacion.WPF.Dialogs.ViewModels;
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
using Transversales.Utilitarios.Tools;

namespace Presentacion.WPF.Dialogs.Views
{
    /// <summary>
    /// Lógica de interacción para UserSearchDialog.xaml
    /// </summary>
    public partial class UserSearchDialog : UserControl
    {
        #region Constructor
        public UserSearchDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Events
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchUsers();
        }

        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeSelectedUser();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfirmUserSelected();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelSelection();
        }

        #endregion

        #region Methods
        private void SearchUsers()
        {
            if (!ValidationMethods.IsUsername(SearchTextBox.Text)) return;

            var command = ((UserSearchDialogViewModel)DataContext).SearchUserCommand;
            if (command != null)
            {
                command.Execute(SearchTextBox.Text);
            }
        }

        private void ChangeSelectedUser()
        {
            if (UsersListView.SelectedItem is SystemUsers)
            {
                ((UserSearchDialogViewModel)DataContext).SelectedUser = (SystemUsers)UsersListView.SelectedItem;
            }
        }

        private void ConfirmUserSelected()
        {
            if (((UserSearchDialogViewModel)DataContext).SelectedUser != null)
            {
                DialogHost.CloseDialogCommand.Execute(((UserSearchDialogViewModel)DataContext).SelectedUser, null);
                return;
            }

            MessageBox.Show("Por favor seleccione un usuario o cancele el proceso", "Alerta", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void CancelSelection()
        {
            DialogHost.CloseDialogCommand.Execute(null, null);
        }

        #endregion
    }
}
