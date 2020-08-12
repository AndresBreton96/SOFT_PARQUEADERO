using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.Commands
{
    public class SaveRateCommand : ICommand
    {
        #region Constructor
        public SaveRateCommand(ModifyPricesViewModel modifyPricesViewModel)
        {
            _modifyPricesViewModel = modifyPricesViewModel;
        }

        #endregion

        #region Variables
        private readonly ModifyPricesViewModel _modifyPricesViewModel;

        public event EventHandler CanExecuteChanged;

        #endregion

        #region Events
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter is Rates)
            {
                Rates rate = (Rates)parameter;
            }
        }

        #endregion
    }
}
