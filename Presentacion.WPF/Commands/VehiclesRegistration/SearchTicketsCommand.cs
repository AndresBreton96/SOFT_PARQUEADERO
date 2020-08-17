using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.Commands.VehiclesRegistration
{
    public class SearchTicketsCommand : ICommand
    {
        #region Constructor
        public SearchTicketsCommand(SearchTicketsViewModel searchTicketsViewModel, ITicketsAdministrator ticketsAdministrator)
        {
            _searchTicketsViewModel = searchTicketsViewModel;
            _ticketsAdministrator = ticketsAdministrator;
        }

        #endregion

        #region Variables
        private readonly SearchTicketsViewModel _searchTicketsViewModel;
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
                if(parameter == null)
                {
                    var ticketsFound = _ticketsAdministrator.GetAll(_searchTicketsViewModel.InitialDate, _searchTicketsViewModel.FinalDate, _searchTicketsViewModel.EntryType, _searchTicketsViewModel.LicensePlate);
                    if (ticketsFound == null || !ticketsFound.Any())
                        ticketsFound = new List<Tickets>();
                    _searchTicketsViewModel.SearchTicketsResultSymbol = ticketsFound;
                }
            }
            catch (TicketNotFoundException ex)
            {
                _searchTicketsViewModel.SearchTicketsResultSymbol = new List<Tickets>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}
