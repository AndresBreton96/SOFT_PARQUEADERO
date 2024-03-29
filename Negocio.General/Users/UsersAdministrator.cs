﻿using Datos.Contratos;
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
            var user = _repositorio.GetAll($"WHERE UserId = {id}").FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException();

            var menusObject = _repositorio.ExecuteQueryObject($"SELECT * FROM UsersMenu WHERE UserId = {user.UserId} AND Permission = 1");

            if (menusObject == null)
            {
                throw new UserNotFoundException($"El usuario {user.UserName} no tiene menús asignados.", user.UserName);
            }

            var userMenus = JsonConvert.DeserializeObject<IEnumerable<UsersMenu>>(JsonConvert.SerializeObject(menusObject));

            foreach (var menu in userMenus)
                menu.MenuName = ResourcesReader.GetPropertyWithLanguage("MenuNames", menu.MenuView);

            user.Menus = userMenus;

            return user;
        }

        public SystemUsers GetFullUser(int id)
        {
            var user = _repositorio.GetAll($"WHERE UserId = {id}").FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException();

            var menusObject = _repositorio.ExecuteQueryObject($"SELECT * FROM UsersMenu WHERE UserId = {user.UserId}");

            if (menusObject == null)
            {
                throw new UserNotFoundException($"El usuario {user.UserName} no tiene menús asignados.", user.UserName);
            }

            var userMenus = JsonConvert.DeserializeObject<IEnumerable<UsersMenu>>(JsonConvert.SerializeObject(menusObject));

            foreach (var menu in userMenus)
                menu.MenuName = ResourcesReader.GetPropertyWithLanguage("MenuNames", menu.MenuView);

            user.Menus = userMenus;

            return user;
        }

        public IEnumerable<SystemUsers> SearchUsers(string searchParameter)
        {
            searchParameter = searchParameter.Replace(' ', '%');
            var outInt = 0;
            var users = _repositorio.GetAll(@$"
                            WHERE UserName LIKE '%{searchParameter}%' OR 
                                  FirstName LIKE '%{searchParameter}%' OR 
                                  LastName LIKE '%{searchParameter}%' OR 
                                  UserId = {(int.TryParse(searchParameter, out outInt) ? Convert.ToInt32(searchParameter).ToString() : "0")} ");
            if (users == null)
                throw new UserNotFoundException(searchParameter);

            return users;
        }

        public void AddUser(SystemUsers user)
        {
            var lastId = _repositorio.GetLastId("UserId");

            var userName = _repositorio.GetAll($"WHERE UserName = '{user.UserName}'");
            if (userName.Any())
                throw new UserNameAlreadyExistsException(user.UserName);

            using (var scope = new TransactionScope())
            {
                var query1 = $@"INSERT INTO [dbo].[SystemUsers]
                                VALUES
                                ({lastId + 1}
                                ,'{user.FirstName}'
                                ,'{user.LastName}'
                                ,'{user.UserName}'
                                ,'{_passwordHasher.HashPassword(user.Password)}'
                                ,GETDATE())";
                _repositorio.ExecuteQuery(query1);

                var query2 = "";
                foreach (var menu in user.Menus)
                {
                    query2 = $@"INSERT INTO [dbo].[UsersMenu]
                                VALUES
                                ({lastId + 1}
                                ,{menu.MenuId}
                                ,'{menu.MenuView}'
                                ,'{menu.Permission}')";
                    _repositorio.ExecuteQuery(query2);
                }

                scope.Complete();
            }

        }

        public void UpdateUser(SystemUsers user)
        {
            var existingUser = _repositorio.GetAll($"WHERE UserId = {user.UserId}");
            if (!existingUser.Any())
                throw new UserDoesNotExistException(user.UserId);


            using (var scope = new TransactionScope())
            {
                _repositorio.ExecuteQuery($@"UPDATE [dbo].[SystemUsers]
                                             SET FirstName = '{user.FirstName}',
                                                 LastName = '{user.LastName}',
                                                 UserName = '{user.UserName}'
                                                 {(!string.IsNullOrEmpty(user.Password) ? ",Password = '" + _passwordHasher.HashPassword(user.Password) + "'" : "")}
                                             WHERE UserId = {user.UserId}");

                foreach (var menu in user.Menus)
                {
                    _repositorio.ExecuteQuery($@"UPDATE [dbo].[UsersMenu]
                                                 SET Permission = '{menu.Permission}'
                                                 WHERE UserId = {menu.UserId} AND MenuId = {menu.MenuId}");
                }

                scope.Complete();
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
