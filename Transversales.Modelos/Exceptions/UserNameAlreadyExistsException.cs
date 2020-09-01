using System;
using System.Collections.Generic;
using System.Text;

namespace Transversales.Modelos.Exceptions
{
    public class UserNameAlreadyExistsException : Exception
    {
        public string UserName { get; set; }

        public UserNameAlreadyExistsException(string userName)
        {
            UserName = userName;
        }

        public UserNameAlreadyExistsException(string message, string userName) : base(message)
        {
            UserName = userName;
        }

        public UserNameAlreadyExistsException(string message, Exception innerException, string userName) : base(message, innerException)
        {
            UserName = userName;
        }

    }
}
