using System.Text.RegularExpressions;

namespace Transversales.Utilitarios.Tools
{
    public static class ValidationMethods
    {
        public static bool IsUsername(string username)
        {
            string pattern;
            // start with a letter, allow letter or number, length between 6 to 12.
            //pattern = @"^[a-zA-Z][a-zA-Z0-9]{5,11}$";
            // To only allow alphanumeric and some symbols.
            pattern = @"[^ \""\']+$";

            Regex regex = new Regex(pattern);
            return regex.IsMatch(username);
        }

        public static bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{4,12}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasNotAllowedSymbolsPattern = @"[^ \""\']+$";
            var hasNotAllowedSymbols = new Regex(hasNotAllowedSymbolsPattern);

            //if (!hasLowerChar.IsMatch(input))
            //{
            //    ErrorMessage = "Password should contain At least one lower case letter";
            //    return false;
            //}
            //else if (!hasUpperChar.IsMatch(input))
            //{
            //    ErrorMessage = "Password should contain At least one upper case letter";
            //    return false;
            //}
            //else 
            if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "La contraseña debe contener entre 4 y 12 caracteres.";
                return false;
            }
            else if (!hasNotAllowedSymbols.IsMatch(input))
            {
                ErrorMessage = "La contraseña contiene caracteres no permitidos.";
                return false;
            }
            //else if (!hasNumber.IsMatch(input))
            //{
            //    ErrorMessage = "Password should contain At least one numeric value";
            //    return false;
            //}

            //else if (!hasSymbols.IsMatch(input))
            //{
            //    ErrorMessage = "Password should contain At least one special case characters";
            //    return false;
            //}
            else
            {
                return true;
            }
        }
    }
}
