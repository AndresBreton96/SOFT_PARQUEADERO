using Negocio.Contratos.Rates;
using Presentacion.WPF.Commands;
using Presentacion.WPF.Commands.Rates;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.ViewModels
{
    public class ModifyPricesViewModel : ViewModelBase
    {
        #region Constructor
        public ModifyPricesViewModel(IRatesAdministrator ratesAdministrator)
        {
            _ratesAdministrator = ratesAdministrator;
            SaveRateCommand = new SaveRateCommand(this, ratesAdministrator);
            SearchRatesCommand = new SearchRatesCommand(this, ratesAdministrator);
            DropRateCommand = new DropRateCommand(this, ratesAdministrator);
            UpdateRateCommand = new UpdateRateCommand(this, ratesAdministrator);
            SearchRatesCommand.Execute(null);
        }

        #endregion

        #region Variables
        public IRatesAdministrator _ratesAdministrator;

        private bool ShowItem = true;

        public ICommand SaveRateCommand { get; }
        public ICommand SearchRatesCommand { get; }
        public ICommand DropRateCommand { get; }
        public ICommand UpdateRateCommand { get; }

        private IEnumerable<RatesByTime> _searchRatesResultSymbol = new List<RatesByTime>();
        public IEnumerable<RatesByTime> SearchRatesResultSymbol
        {
            get
            {
                return _searchRatesResultSymbol;
            }
            set
            {
                _searchRatesResultSymbol = value;
                OnPropertyChanged(nameof(SearchRatesResultSymbol));
            }
        }

        public RatesByTime EditingRate;
        #endregion
    }
}
