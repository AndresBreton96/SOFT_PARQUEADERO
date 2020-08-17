using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para SearchTicketsView.xaml
    /// </summary>
    public partial class SearchTicketsView : UserControl
    {
        #region Constructor
        public SearchTicketsView()
        {
            InitializeComponent();
            InitialDatePicker.DisplayDateEnd = DateTime.Today;
            FinalDatePicker.DisplayDateEnd = DateTime.Today;

            var entryTypes = Enum.GetValues(typeof(EntryType)).Cast<EntryType>();

            EntryTypesCbo.ItemsSource = entryTypes;
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty SearchTicketsCommandProperty =
            DependencyProperty.Register("SearchTicketsCommand", typeof(ICommand), typeof(SearchTicketsView), new PropertyMetadata(null));

        public ICommand SearchTicketsCommand
        {
            get { return (ICommand)GetValue(SearchTicketsCommandProperty); }
            set { SetValue(SearchTicketsCommandProperty, value); }
        }

        #endregion

        #region Events
        private void InitialDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if(FinalDatePicker.SelectedDate < InitialDatePicker.SelectedDate)
            {
                MessageBox.Show("La fecha final debe ser mayor a la inicial, intente nuevamente");
                return;
            }

            Search();

        }

        private void FinalDatePicker_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FinalDatePicker.SelectedDate > InitialDatePicker.SelectedDate)
            {
                MessageBox.Show("La fecha final no debe ser menor a la inicial, intente nuevamente");
                return;
            }

            Search();
        }

        private void EntryTypesCbo_LostFocus(object sender, RoutedEventArgs e)
        {
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

            if(EntryTypesCbo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un tipo de movimiento e intente nuevamente");
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

                    if (EntryTypesCbo.SelectedIndex == -1)
                    {
                        MessageBox.Show("Por favor seleccione un tipo de movimiento e intente nuevamente");
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
                if(SearchTicketsCommand != null)
                {
                    ((SearchTicketsViewModel)DataContext).LicensePlate = LicenseTextBox.Text;
                    SearchTicketsCommand.Execute(null);
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
