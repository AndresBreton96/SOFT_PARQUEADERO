using Negocio.Contratos.Users;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;

namespace Presentacion.WPF.Commands.Users
{
    public class AddUserCommand : ICommand
    {
        #region Constructor
        public AddUserCommand(UsersViewModel usersViewModel, IUsersAdministrator usersAdministrator)
        {
            _usersViewModel = usersViewModel;
            _usersAdministrator = usersAdministrator;
        }

        #endregion

        #region Variables
        private readonly UsersViewModel _usersViewModel;
        private readonly IUsersAdministrator _usersAdministrator;

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Events
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                if (parameter is SystemUsers)
                {
                    SystemUsers user = (SystemUsers)parameter;
                    if(user.UserId != 0)
                    {
                        _usersAdministrator.UpdateUser(user);
                        MessageBox.Show("Usuario modificado con éxito.");
                    }
                    else
                    {
                        _usersAdministrator.AddUser(user);
                        MessageBox.Show("Usuario agregado con éxito.");
                    }
                }
            }
            catch(UserNameAlreadyExistsException ex)
            {
                throw new Exception($"El nombre de usuario {ex.UserName} ya se encuentra registrado en la base de datos.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
