using Presentacion.WPF.State.Navigators;
using Presentacion.WPF.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        #region Constructor
        public HomeView()
        {
            InitializeComponent();
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty UpdateCurrentViewModelCommandProperty =
            DependencyProperty.Register("UpdateCurrentViewModelCommand", typeof(ICommand), typeof(HomeView), new PropertyMetadata(null));

        public ICommand UpdateCurrentViewModelCommand
        {
            get { return (ICommand)GetValue(UpdateCurrentViewModelCommandProperty); }
            set { SetValue(UpdateCurrentViewModelCommandProperty, value); }
        }

        #endregion

        #region Events
        private void btnModifyPrices_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentViewModel(ViewType.ModifyPrices);
        }

        private void btnRegisterEntrance_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentViewModel(ViewType.RegisterEntry);
        }

        private void btnSearchTickets_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentViewModel(ViewType.SearchTickets);
        }

        private void btnSearchBills_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentViewModel(ViewType.SearchBills);
        }

        private void btnModifyUsers_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentViewModel(ViewType.Users);
        }

        #endregion

        #region Methods
        private void UpdateCurrentViewModel(ViewType viewType)
        {
            UpdateCurrentViewModelCommand.Execute(viewType);
        }

        #endregion
    }
}
