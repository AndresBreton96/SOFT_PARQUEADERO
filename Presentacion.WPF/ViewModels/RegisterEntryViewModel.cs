using MaterialDesignThemes.Wpf;
using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.Commands;
using Presentacion.WPF.Commands.VehiclesRegistration;
using System;
using System.Windows;
using System.Windows.Input;

namespace Presentacion.WPF.ViewModels
{
    public class RegisterEntryViewModel : ViewModelBase
    {
        #region Constructor
        public RegisterEntryViewModel(ITicketsAdministrator ticketsAdministrator)
        {
            _ticketsAdministrator = ticketsAdministrator;
            SaveEntryTicketCommand = new SaveEntryTicketCommand(this, _ticketsAdministrator);
        }

        #endregion

        #region Variables
        public ITicketsAdministrator _ticketsAdministrator;
        public ICommand SaveEntryTicketCommand { get; }

        #endregion

        #region Events

        public void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {

        }

        #endregion
    }
}
