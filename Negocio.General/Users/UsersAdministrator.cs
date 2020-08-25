using Datos.Contratos;
using Microsoft.AspNet.Identity;
using Negocio.Contratos.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;
using Transversales.Utilitarios.Tools;

namespace Negocio.General.Users
{
    public class UsersAdministrator : IUsersAdministrator
    {
        #region Constructor
        public UsersAdministrator(IUsersRepository repositorio, IPasswordHasher passwordHasher)
        {
            _repositorio = repositorio;
            _passwordHasher = passwordHasher;
        }

        #endregion

        #region Variables
        private readonly IUsersRepository _repositorio;
        private readonly IPasswordHasher _passwordHasher;

        #endregion

        #region Methods
        public SystemUsers ValidateLogIn(string username, string password)
        {
            var user = _repositorio.GetAll($"WHERE UserName = '{username}'").FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException(username);

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(user.Password, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }

            var menusObject = _repositorio.ExecuteQueryObject($"SELECT * FROM UsersMenu WHERE UserId = {user.UserId} AND Permission = 1");

            if (menusObject == null)
            {
                throw new UserNotFoundException($"El usuario {username} no tiene menús asignados.", username);
            }

            var userMenus = JsonConvert.DeserializeObject<IEnumerable<UsersMenu>>(JsonConvert.SerializeObject(menusObject));

            user.Menus = userMenus;

            return user;
        }

        public SystemUsers GetUser(int id)
        {
            var user = _repositorio.GetAll($"WHERE Id = {id}").FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException();

            var menusObject = _repositorio.ExecuteQueryObject($"SELECT * FROM UsersMenu WHERE UserId = {user.UserId} AND Permiso = 1");

            if (menusObject == null)
            {
                throw new UserNotFoundException($"El usuario {user.UserName} no tiene menús asignados.", user.UserName);
            }

            var userMenus = JsonConvert.DeserializeObject<IEnumerable<UsersMenu>>(JsonConvert.SerializeObject(menusObject));

            user.Menus = userMenus;

            return user;
        }

        public IEnumerable<SystemUsers> SearchUsers(string searchParameter)
        {
            var users = _repositorio.GetAll($"WHERE UserName = '{searchParameter}' OR FirstName = '{searchParameter}' OR LastName = '{searchParameter}' OR Id = {searchParameter} ");
            if (users == null)
                throw new UserNotFoundException(searchParameter);

            return users;
        }

        public void AddUser(SystemUsers user)
        {
            try
            {
                var lastId = _repositorio.GetLastId("UserId");

                using (var scope = new TransactionScope())
                {
                    _repositorio.ExecuteQuery($@"INSERT INTO [dbo].[Bills]
                                                 VALUES
                                                    ({lastId + 1}
                                                    ,'{user.FirstName}'
                                                    ,'{user.LastName}'
                                                    ,'{user.UserName}'
                                                    ,'{_passwordHasher.HashPassword(user.Password)}'
                                                    ,GETDATE())");

                    foreach (var menu in user.Menus)
                    {
                        _repositorio.ExecuteQuery($@"INSERT INTO [dbo].[UsersMenu]
                                                     VALUES
                                                        ({lastId + 1}
                                                        ,{menu.MenuId}
                                                        ,'{menu.MenuView}'
                                                        ,'{menu.Permission}')");
                    }

                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UsersMenu> LoadMenus()
        {
            try
            {
                var menusObject = _repositorio.ExecuteQueryObject($"SELECT * FROM MenuOptions");

                var menus = JsonConvert.DeserializeObject<IEnumerable<MenuOptions>>(JsonConvert.SerializeObject(menusObject));

                var userMenus = new List<UsersMenu>();
                foreach (var menu in menus)
                {
                    userMenus.Add(new UsersMenu()
                    {
                        UserId = 0,
                        MenuId = menu.MenuId,
                        MenuName = ResourcesReader.GetPropertyWithLanguage("MenuNames", menu.MenuView),
                        MenuView = menu.MenuView,
                        Permission = false
                    });
                }

                return userMenus;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
