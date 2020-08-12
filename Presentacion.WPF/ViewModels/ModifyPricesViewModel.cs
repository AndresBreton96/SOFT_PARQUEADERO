using Presentacion.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Transversales.Modelos;

namespace Presentacion.WPF.ViewModels
{
    public class ModifyPricesViewModel : ViewModelBase
    {
        #region Constructor
        public ModifyPricesViewModel()
        {
            SaveRateCommand = new SaveRateCommand(this);
        }

        #endregion

        #region Variables
        private bool ShowItem = true;

        private ObservableCollection<Rates> _rates;

        public ObservableCollection<Rates> Rates
        {
            get => _rates;
            set
            {
                _rates = value;
                OnPropertyChanged(nameof(Rates));
            }
        }

        public ICommand SaveRateCommand { get; }

        #endregion
    }
}
