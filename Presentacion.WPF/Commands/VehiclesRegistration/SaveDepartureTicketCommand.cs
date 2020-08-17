using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.Commands.VehiclesRegistration
{
    public class SaveDepartureTicketCommand : ICommand
    {
        #region Constructor
        public SaveDepartureTicketCommand(RegisterDepartureViewModel registerDepartureViewModel, ITicketsAdministrator ticketsAdministrator)
        {
            _registerDepartureViewModel = registerDepartureViewModel;
            _ticketsAdministrator = ticketsAdministrator;
        }

        #endregion

        #region Variables
        private readonly RegisterDepartureViewModel _registerDepartureViewModel;
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
                if (parameter is Tickets)
                {
                    Tickets ticket = (Tickets)parameter;
                    ticket.EntryType = EntryType.Departure;

                    var entryTicket = _ticketsAdministrator.GetEntryTicket(ticket.LicensePlate);
                    ticket.EntryTicketId = entryTicket.TicketId;

                    _ticketsAdministrator.AddTicket(ticket);
                }
            }
            catch (EntryTicketNotFoundException ex)
            {
                MessageBox.Show($"La entrada con Id {ex.EntryTicketId} no ha sido encontrada en la base de datos.");
                throw ex;
            }
            catch (DepartureTicketAlreadyRegistered ex)
            {
                MessageBox.Show($"Ya se ha registrado una salida para la entrada con id {ex.DepartureTicketId}.");
                throw ex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw ex;
            }
        }

        #endregion
    }
}
