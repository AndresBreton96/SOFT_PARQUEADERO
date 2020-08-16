using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Transversales.Modelos.Exceptions;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        #region Constructor
        public LoginView()
        {
            InitializeComponent();
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty LoginCommandProperty =
            DependencyProperty.Register("LoginCommand", typeof(ICommand), typeof(LoginView), new PropertyMetadata(null));

        public ICommand LoginCommand
        {
            get { return (ICommand)GetValue(LoginCommandProperty); }
            set { SetValue(LoginCommandProperty, value); }
        }

        #endregion

        #region Events
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (LoginCommand != null)
            {
                string password = pbPassword.Password;
                try
                {
                    LoginCommand.Execute(password);
                }
                catch (UserNotFoundException userEx)
                {
                    if (!string.IsNullOrEmpty(userEx.Message))
                        LogInErrorText.Text = userEx.Message;
                    else
                        LogInErrorText.Text = $"El usuario {userEx.Username} no se ha encontrado en la base de datos.";
                    LogInErrorText.Visibility = Visibility.Visible;
                }
                catch (InvalidPasswordException passEx)
                {
                    LogInErrorText.Text = $"La contraseña ingresada para el usuario {passEx.Username} es incorrecta, por favor intente nuevamente.";
                    LogInErrorText.Visibility = Visibility.Visible;
                }
            }
        }

        #endregion
    }
}
