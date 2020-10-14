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
using System.Windows.Media.Imaging;
using Transversales.Modelos.Exceptions;
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

            TxtBackground.ImageSource = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\Ppar.jpg"));
            DepartureTxtBackground.ImageSource = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\Ppar.jpg"));
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty SaveEntryTicketCommandProperty =
            DependencyProperty.Register("SaveEntryTicketCommand", typeof(ICommand), typeof(RegisterEntryView), new PropertyMetadata(null));

        public static readonly DependencyProperty CalculatePriceCommandProperty =
            DependencyProperty.Register("CalculatePriceCommand", typeof(ICommand), typeof(RegisterEntryView), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveDepartureTicketCommandProperty =
            DependencyProperty.Register("SaveDepartureTicketCommand", typeof(ICommand), typeof(RegisterEntryView), new PropertyMetadata(null));

        public static readonly DependencyProperty RunDialogCommandProperty =
            DependencyProperty.Register("RunDialogCommand", typeof(ICommand), typeof(RegisterEntryView), new PropertyMetadata(null));

        public static readonly DependencyProperty CreateBillCommandProperty =
            DependencyProperty.Register("CreateBillCommand", typeof(ICommand), typeof(RegisterEntryView), new PropertyMetadata(null));

        public ICommand SaveEntryTicketCommand
        {
            get { return (ICommand)GetValue(SaveEntryTicketCommandProperty); }
            set { SetValue(SaveEntryTicketCommandProperty, value); }
        }

        public ICommand CalculatePriceCommand
        {
            get { return (ICommand)GetValue(CalculatePriceCommandProperty); }
            set { SetValue(CalculatePriceCommandProperty, value); }
        }

        public ICommand SaveDepartureTicketCommand
        {
            get { return (ICommand)GetValue(SaveDepartureTicketCommandProperty); }
            set { SetValue(SaveDepartureTicketCommandProperty, value); }
        }

        public ICommand RunDialogCommand
        {
            get { return (ICommand)GetValue(RunDialogCommandProperty); }
            set { SetValue(RunDialogCommandProperty, value); }
        }

        public ICommand CreateBillCommand
        {
            get { return (ICommand)GetValue(CreateBillCommandProperty); }
            set { SetValue(CreateBillCommandProperty, value); }
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

        private void DeparturePlatesTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    CreateBill();
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

        private void CreateBill()
        {
            try
            {

                if (CalculatePriceCommand != null)
                {
                    CalculatePriceCommand.Execute(PlatesTextBox.Text);
                    ((RegisterEntryViewModel)DataContext).Plates = PlatesTextBox.Text;

                    //if ((((RegisterEntryViewModel)DataContext).Fractions > 1 && ((RegisterEntryViewModel)DataContext).Hours == 0) || (((RegisterEntryViewModel)DataContext).Fractions != 0 && ((RegisterEntryViewModel)DataContext).Hours != 0))                 
                    //{
                    //    if (RunDialogCommand != null)
                    //    {
                    //        RunDialogCommand.Execute(null);
                    //        PlatesTextBox.Text = string.Empty;
                    //        return;
                    //    }
                    //    MessageBox.Show("No se ha podido inicializar el comando para mostrar el diálogo.");
                    //    ((RegisterEntryViewModel)DataContext).Plates = null;
                    //    PlatesTextBox.Text = string.Empty;
                    //    return;
                    //}

                    if (SaveDepartureTicketCommand != null)
                    {
                        var ticket = new Tickets()
                        {
                            TicketId = 0,
                            EntryTicketId = 0,
                            EntryType = EntryType.Salida,
                            LicensePlate = PlatesTextBox.Text,
                            EntryDate = ((RegisterEntryViewModel)DataContext).DepartureTime
                        };

                        SaveDepartureTicketCommand.Execute(ticket);
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido inicializar el comando para crear guardar el ticket de salida.");
                        ((RegisterEntryViewModel)DataContext).Plates = null;
                        PlatesTextBox.Text = string.Empty;
                        return;
                    }

                    if (CreateBillCommand != null)
                    {
                        CreateBillCommand.Execute(PlatesTextBox.Text);
                        ((RegisterEntryViewModel)DataContext).Plates = null;
                        PlatesTextBox.Text = string.Empty;

                        var priceMessageDialog = new ChargedPriceDialog()
                        {
                            Message = { Text = ((RegisterEntryViewModel)DataContext).Price.ToString("C") }
                        };

                        DialogHost.Show(priceMessageDialog, "RootDialog");
                        return;
                    }

                    MessageBox.Show("No se ha podido inicializar el comando para crear la factura.");
                    ((RegisterEntryViewModel)DataContext).Plates = null;
                    PlatesTextBox.Text = string.Empty;

                    return;

                }
                MessageBox.Show("No se ha podido inicializar el comando para calcular el precio.");
                ((RegisterEntryViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;

            }
            catch (TicketNotFoundException)
            {
                ((RegisterEntryViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
            catch (EntryTicketNotFoundException)
            {
                ((RegisterEntryViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
            catch (DepartureTicketAlreadyRegistered)
            {
                ((RegisterEntryViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
            catch (Exception)
            {
                ((RegisterEntryViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
        }

        #endregion
    }
}
