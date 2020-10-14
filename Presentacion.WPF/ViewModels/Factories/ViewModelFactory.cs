using Presentacion.WPF.State.Navigators;
using System;

namespace Presentacion.WPF.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        #region Constructor
        public ViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel,
                                CreateViewModel<LoginViewModel> createLoginViewModel,
                                CreateViewModel<ModifyPricesViewModel> createModifyPricesViewModel,
                                CreateViewModel<RegisterEntryViewModel> createRegisterEntryViewModel,
                                CreateViewModel<SearchTicketsViewModel> createSearchTicketsViewModel,
                                CreateViewModel<SearchBillsViewModel> createSearchBillsViewModel, CreateViewModel<UsersViewModel> createUsersViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createModifyPricesViewModel = createModifyPricesViewModel;
            _createRegisterEntryViewModel = createRegisterEntryViewModel;
            _createSearchTicketsViewModel = createSearchTicketsViewModel;
            _createSearchBillsViewModel = createSearchBillsViewModel;
            _createUsersViewModel = createUsersViewModel;
        }

        #endregion

        #region Variables
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<ModifyPricesViewModel> _createModifyPricesViewModel;
        private readonly CreateViewModel<RegisterEntryViewModel> _createRegisterEntryViewModel;
        private readonly CreateViewModel<SearchTicketsViewModel> _createSearchTicketsViewModel;
        private readonly CreateViewModel<SearchBillsViewModel> _createSearchBillsViewModel;
        private readonly CreateViewModel<UsersViewModel> _createUsersViewModel;

        #endregion

        #region Methods
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Login:
                    return _createLoginViewModel();
                case ViewType.Home:
                    return _createHomeViewModel();
                case ViewType.ModifyPrices:
                    return _createModifyPricesViewModel();
                case ViewType.RegisterEntry:
                    return _createRegisterEntryViewModel();
                case ViewType.SearchTickets:
                    return _createSearchTicketsViewModel();
                case ViewType.SearchBills:
                    return _createSearchBillsViewModel();
                case ViewType.Users:
                    return _createUsersViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }

        #endregion

    }
}
