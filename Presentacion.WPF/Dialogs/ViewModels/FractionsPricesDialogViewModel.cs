using System;
using System.ComponentModel;

namespace Presentacion.WPF.Dialogs.ViewModels
{
    public class FractionsPricesDialogViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
