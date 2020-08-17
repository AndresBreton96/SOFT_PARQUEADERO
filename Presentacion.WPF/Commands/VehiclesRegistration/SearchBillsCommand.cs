using Negocio.Contratos.VehiclesRegistration;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Transversales.Modelos.VehiclesRegistration;

namespace Presentacion.WPF.Commands.VehiclesRegistration
{
    public class SearchBillsCommand : ICommand
    {
        #region Constructor
        public SearchBillsCommand(SearchBillsViewModel searchBillsViewModel, IBillsAdministrator billsAdministrator)
        {
            _searchBillsViewModel = searchBillsViewModel;
            _billsAdministrator = billsAdministrator;
        }

        #endregion

        #region Variables
        private readonly SearchBillsViewModel _searchBillsViewModel;
        private readonly IBillsAdministrator _billsAdministrator;


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
                if (parameter == null)
                {
                    var billsFound = _billsAdministrator.GetAll(_searchBillsViewModel.InitialDate, _searchBillsViewModel.FinalDate, _searchBillsViewModel.LicensePlate);
                    if (billsFound == null || !billsFound.Any())
                        billsFound = new List<Bills>();
                    _searchBillsViewModel.SearchBillsResultSymbol = billsFound;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _searchBillsViewModel.SearchBillsResultSymbol = new List<Bills>();
            }
        }

        #endregion
    }
}
