using Negocio.Contratos.Rates;
using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos;
using Transversales.Modelos.Exceptions;

namespace Presentacion.WPF.Commands
{
    public class SaveRateCommand : ICommand
    {
        #region Constructor
        public SaveRateCommand(ModifyPricesViewModel modifyPricesViewModel, IRatesAdministrator ratesAdministrator)
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
                    _ratesAdministrator.AddRate(rate);
                }
            }
            catch (ExistingRateException ex)
            {
                MessageBox.Show("Ya existe una tarifa del tipo seleccionado, por favor eliminela antes de agregar una nueva.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
