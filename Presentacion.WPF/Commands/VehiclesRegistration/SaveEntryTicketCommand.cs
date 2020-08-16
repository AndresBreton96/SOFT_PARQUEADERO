using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.Commands.VehiclesRegistration
{
    public class SaveEntryTicketCommand : ICommand
    {
        #region Constructor
        public SaveEntryTicketCommand(RegisterEntryViewModel registerEntryViewModel, ITicketsAdministrator ticketsAdministrator)
        {
            _registerEntryViewModel = registerEntryViewModel;
            _ticketsAdministrator = ticketsAdministrator;
        }

        #endregion

        #region Variables
        private readonly RegisterEntryViewModel _registerEntryViewModel;
        private readonly ITicketsAdministrator _ticketsAdministrator;

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Events
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            try
            {
                if(parameter is Tickets)
                {
                    Tickets ticket = (Tickets)parameter;
                    ticket.EntryType = EntryType.Entrance;
                    _ticketsAdministrator.AddTicket(ticket);
                }
            }
            catch (EntryTicketAlreadyRegistered ex)
            {
                MessageBox.Show($"El vehículo de placas {ex.LicensePlate} se encuentra registrado dentro del parqueadero y no se ha registrado su salida.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
