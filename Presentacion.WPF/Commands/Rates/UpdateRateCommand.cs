using Negocio.Contratos.Rates;
using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;

namespace Presentacion.WPF.Commands.Rates
{
    public class UpdateRateCommand : ICommand
    {
        #region Constructor
        public UpdateRateCommand(ModifyPricesViewModel modifyPricesViewModel, IRatesAdministrator ratesAdministrator)
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
                    _ratesAdministrator.UpdateRate(rate);
                }
            }
            catch (ExistingRateException ex)
            {
                MessageBox.Show("No se ha encontrado la tarifa que se desea modificar, por favor intente nuevamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
