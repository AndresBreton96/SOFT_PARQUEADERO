using Negocio.Contratos.Rates;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.Commands.Rates
{
    public class DropRateCommand : ICommand
    {
        #region Constructor
        public DropRateCommand(ModifyPricesViewModel modifyPricesViewModel, IRatesAdministrator ratesAdministrator)
        {
            _modifyPricesViewModel = modifyPricesViewModel;
            _ratesAdministrator = ratesAdministrator;
        }

        #endregion

        #region Variables
        private readonly ModifyPricesViewModel _modifyPricesViewModel;
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
                if (parameter is RatesByTime)
                {
                    RatesByTime rate = (RatesByTime)parameter;
                    _ratesAdministrator.DropRate(rate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}
