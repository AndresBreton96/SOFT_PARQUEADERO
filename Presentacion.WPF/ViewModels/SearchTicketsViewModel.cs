using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.Commands.VehiclesRegistration;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Transversales.Modelos.RegistrationEntries;

namespace Presentacion.WPF.ViewModels
{
    public class SearchTicketsViewModel : ViewModelBase
    {
        #region Constructor
        public SearchTicketsViewModel(ITicketsAdministrator ticketsAdministrator)
        {
            _ticketsAdministrator = ticketsAdministrator;

            SearchTicketsCommand = new SearchTicketsCommand(this, _ticketsAdministrator);

            InitialDate = DateTime.Today;
            FinalDate = DateTime.Today;
            EntryType = EntryType.Vacio;
            LicensePlate = string.Empty;

            SearchTicketsCommand.Execute(null);
        }

        #endregion

        #region Variables
        private readonly ITicketsAdministrator _ticketsAdministrator;

        public ICommand SearchTicketsCommand { get; }

        public bool ShowItem = true;

        private IEnumerable<Tickets> _searchTicketsResultSymbol = new List<Tickets>();
        public IEnumerable<Tickets> SearchTicketsResultSymbol
        {
            get
            {
                return _searchTicketsResultSymbol;
            }
            set
            {
                _searchTicketsResultSymbol = value;
                OnPropertyChanged(nameof(SearchTicketsResultSymbol));
            }
        }

        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public EntryType EntryType { get; set; }
        public string LicensePlate { get; set; }

        #endregion
    }
}
