using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.Commands.VehiclesRegistration;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Transversales.Modelos.VehiclesRegistration;

namespace Presentacion.WPF.ViewModels
{
    public class SearchBillsViewModel : ViewModelBase
    {
        #region Constructor
        public SearchBillsViewModel(IBillsAdministrator billsAdministrator)
        {
            _billsAdministrator = billsAdministrator;

            SearchBillsCommand = new SearchBillsCommand(this, _billsAdministrator);

            InitialDate = DateTime.Today;
            FinalDate = DateTime.Today;
            LicensePlate = string.Empty;

            SearchBillsCommand.Execute(null);
        }

        #endregion

        #region Variables
        private readonly IBillsAdministrator _billsAdministrator;

        public ICommand SearchBillsCommand { get; }

        public bool ShowItem = true;

        private IEnumerable<Bills> _searchBillsResultSymbol = new List<Bills>();
        public IEnumerable<Bills> SearchBillsResultSymbol
        {
            get
            {
                return _searchBillsResultSymbol;
            }
            set
            {
                _searchBillsResultSymbol = value;
                OnPropertyChanged(nameof(SearchBillsResultSymbol));
            }
        }

        public DateTime InitialDate { get; set; }
        public DateTime FinalDate { get; set; }
        public string LicensePlate { get; set; }


        #endregion
    }
}
