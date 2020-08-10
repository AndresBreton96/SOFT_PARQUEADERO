using System;
using System.Collections.Generic;

namespace Transversales.Modelos
{
    public class SystemUsers
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime DateJoined { get; set; }
        public IEnumerable<UsersMenu> Menus { get; set; }
    }

    public class UsersMenu
    {
        public int UserId { get; set; }
        public short MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuView { get; set; }
        public bool Permiso { get; set; }
    }

}
