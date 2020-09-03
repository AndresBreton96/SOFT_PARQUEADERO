using System;

namespace Transversales.Modelos.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public int UserId { get; set; }

        public UserDoesNotExistException(int userId)
        {
            UserId = userId;
        }

        public UserDoesNotExistException(string message, int userId) : base(message)
        {
            UserId = userId;
        }

        public UserDoesNotExistException(string message, Exception innerException, int userId) : base(message, innerException)
        {
            UserId = userId;
        }
    }
}
