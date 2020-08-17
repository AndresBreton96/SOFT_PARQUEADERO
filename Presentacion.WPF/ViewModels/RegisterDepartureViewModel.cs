using MaterialDesignThemes.Wpf;
using Negocio.Contratos.Rates;
using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.Commands;
using Presentacion.WPF.Commands.VehiclesRegistration;
using Presentacion.WPF.Dialogs;
using Presentacion.WPF.Dialogs.ViewModels;
using Presentacion.WPF.Dialogs.Views;
using System;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.ViewModels
{
    public class RegisterDepartureViewModel : ViewModelBase
    {
        #region Constructor
        public RegisterDepartureViewModel(ITicketsAdministrator ticketsAdministrator, IRatesAdministrator ratesAdministrator, IBillsAdministrator billsAdministrator)
        {
            _ticketsAdministrator = ticketsAdministrator;
            _ratesAdministrator = ratesAdministrator;
            _billsAdministrator = billsAdministrator;

            CalculatePriceCommand = new CalculatePriceCommand(this, _ticketsAdministrator, _ratesAdministrator);
            SaveDepartureTicketCommand = new SaveDepartureTicketCommand(this, _ticketsAdministrator);
            CreateBillCommand = new CreateBillCommand(this, _ticketsAdministrator, _ratesAdministrator, _billsAdministrator);
        }

        #endregion

        #region Variables
        public ITicketsAdministrator _ticketsAdministrator;
        public IRatesAdministrator _ratesAdministrator;
        public IBillsAdministrator _billsAdministrator;
        public ICommand CalculatePriceCommand { get; }
        public ICommand SaveDepartureTicketCommand { get; }

        public ICommand CreateBillCommand { get; }
        public ICommand RunDialogCommand => new AnotherCommandImplementation(ExecuteRunDialog);

        public double Hours { get; set; }
        public double Fractions { get; set; }
        public double Price { get; set; }

        public DateTime DepartureTime { get; set; }

        public string Plates { get; set; }

        #endregion

        #region Events
        private async void ExecuteRunDialog(object o)
        {
            try
            {
                var view = new FractionsPricesDialog
                {
                    DataContext = new FractionsPricesDialogViewModel()
                };

                var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

                if ((bool)result)
                {
                    Hours++;
                    Fractions = 0;
                }

                if (SaveDepartureTicketCommand != null)
                {
                    var ticket = new Tickets()
                    {
                        TicketId = 0,
                        EntryTicketId = 0,
                        EntryType = EntryType.Salida,
                        LicensePlate = Plates,
                        EntryDate = DepartureTime
                    };

                    SaveDepartureTicketCommand.Execute(ticket);
                }
                else
                {
                    MessageBox.Show("No se ha podido inicializar el comando para crear guardar el ticket de salida.");
                    Plates = null;
                    return;
                }


                if (CreateBillCommand != null)
                {
                    CreateBillCommand.Execute(Plates);
                    Plates = null;

                    var priceMessageDialog = new ChargedPriceDialog()
                    {
                        Message = { Text = Price.ToString("C") }
                    };

                    DialogHost.Show(priceMessageDialog, "RootDialog");
                    return;
                }

                MessageBox.Show("No se ha podido inicializar el comando para crear la factura.");
                Plates = null;
                return;
            }
            catch (TicketNotFoundException)
            {
                Plates = null;
                return;
            }
            catch (EntryTicketNotFoundException)
            {
                Plates = null;
                return;
            }
            catch (DepartureTicketAlreadyRegistered)
            {
                Plates = null;
                return;
            }
            catch (Exception)
            {
                Plates = null;
                return;
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        #endregion

    }
}
