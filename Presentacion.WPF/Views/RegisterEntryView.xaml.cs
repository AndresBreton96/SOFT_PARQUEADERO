using MaterialDesignThemes.Wpf;
using Presentacion.WPF.Dialogs;
using Presentacion.WPF.Dialogs.ViewModels;
using Presentacion.WPF.Dialogs.Views;
using Presentacion.WPF.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterEntryView.xaml
    /// </summary>
    public partial class RegisterEntryView : UserControl
    {
        #region Constructor
        public RegisterEntryView()
        {
            InitializeComponent();
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty SaveEntryTicketCommandProperty =
            DependencyProperty.Register("SaveEntryTicketCommand", typeof(ICommand), typeof(RegisterEntryView), new PropertyMetadata(null));

        public ICommand SaveEntryTicketCommand
        {
            get { return (ICommand)GetValue(SaveEntryTicketCommandProperty); }
            set { SetValue(SaveEntryTicketCommandProperty, value); }
        }

        #endregion

        #region Events
        private void PlatesTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    RegisterVehicleEntryAsync();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods
        private async Task RegisterVehicleEntryAsync()
        {
            var plates = PlatesTextBox.Text;
            if(plates.Length < 6)
            {
                MessageBox.Show("La placa no cumple con el mínimo requerido");
                PlatesTextBox.Focus();
                return;
            }

            var view = new VehicleTypeDialog
            {
                DataContext = new VehicleTypeDialogViewModel()
            };

            var result = await DialogHost.Show(view, "RootDialog", ((RegisterEntryViewModel)DataContext).ClosingEventHandler);
            var vehicleType = VehicleType.Car;

            if (!(bool)result)
            {
                vehicleType = VehicleType.Bike;
            }

            var ticket = new Tickets()
            {
                EntryDate = DateTime.Now,
                EntryTicketId = 0,
                LicensePlate = plates,
                EntryType = EntryType.Entrada,
                VehicleType = vehicleType,
                TicketId = 0
            };

            if(SaveEntryTicketCommand != null)
            {
                SaveEntryTicketCommand.Execute(ticket);
                PlatesTextBox.Text = "";
                PlatesTextBox.Focus();
                return;
            }

            MessageBox.Show("No ha sido posible inicializar el comando de guardado");
            PlatesTextBox.Focus();

        }

        #endregion
    }
}
