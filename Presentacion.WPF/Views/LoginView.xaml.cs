using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        #region Metodos
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

        #region Eventos
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (LoginCommand != null)
            {
                string password = pbPassword.Password;
                LoginCommand.Execute(password);
            }
        }

        #endregion
    }
}
