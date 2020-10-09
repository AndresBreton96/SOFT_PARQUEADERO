using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para SearchBillsView.xaml
    /// </summary>
    public partial class SearchBillsView : UserControl
    {
        #region Constructor
        public SearchBillsView()
        {
            InitializeComponent();

            InitialDatePicker.DisplayDateEnd = DateTime.Today;
            FinalDatePicker.DisplayDateEnd = DateTime.Today;
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty SearchBillsCommandProperty =
            DependencyProperty.Register("SearchBillsCommand", typeof(ICommand), typeof(SearchBillsView), new PropertyMetadata(null));

        public ICommand SearchBillsCommand
        {
            get { return (ICommand)GetValue(SearchBillsCommandProperty); }
            set { SetValue(SearchBillsCommandProperty, value); }
        }

        #endregion

        #region Events
        private void InitialDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FinalDatePicker.SelectedDate < InitialDatePicker.SelectedDate)
            {
                MessageBox.Show("La fecha final debe ser mayor a la inicial, intente nuevamente");
                return;
            }

            Search();
        }

        private void FinalDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FinalDatePicker.SelectedDate < InitialDatePicker.SelectedDate)
            {
                MessageBox.Show("La fecha final no debe ser menor a la inicial, intente nuevamente");
                return;
            }

            Search();
        }

        private void LicenseTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:

                    if (InitialDatePicker.SelectedDate == DateTime.MinValue || InitialDatePicker.SelectedDate == null)
                    {
                        MessageBox.Show("Por favor seleccione una fecha inicial e intente nuevamente");
                        return;
                    }

                    if (FinalDatePicker.SelectedDate == DateTime.MinValue || FinalDatePicker.SelectedDate == null)
                    {
                        MessageBox.Show("Por favor seleccione una fecha final e intente nuevamente");
                        return;
                    }

                    Search();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods
        private void Search()
        {
            try
            {
                if (SearchBillsCommand != null)
                {
                    ((SearchBillsViewModel)DataContext).LicensePlate = LicenseTextBox.Text;
                    SearchBillsCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un inconveniente: " + ex.Message);
            }
        }

        #endregion
    }
}
