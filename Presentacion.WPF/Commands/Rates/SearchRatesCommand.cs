using Negocio.Contratos.Rates;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.Commands.Rates
{
    public class SearchRatesCommand : ICommand
    {
        #region Constructor
        public SearchRatesCommand(ModifyPricesViewModel viewModel, IRatesAdministrator ratesAdministrator)
        {
            _viewModel = viewModel;
            _ratesAdministrator = ratesAdministrator;
        }

        #endregion

        #region Variables
        private readonly ModifyPricesViewModel _viewModel;
        private readonly IRatesAdministrator _ratesAdministrator;


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
                    var ratesList = _ratesAdministrator.GetAll();
                    if (ratesList == null)
                        ratesList = new List<RatesByTime>();
                    _viewModel.SearchRatesResultSymbol = ratesList;
                }
                if(parameter is byte)
                {
                    var rate = _ratesAdministrator.GetAll((byte)parameter);
                    if (rate.Any())
                    {
                        _viewModel.EditingRate = rate.FirstOrDefault();
                        return;
                    }
                    MessageBox.Show("No se encontró la tarifa deseada.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #endregion
    }
}
