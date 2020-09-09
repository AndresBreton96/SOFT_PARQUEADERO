using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Presentacion.WPF.Dialogs.ViewModels
{
    public class ChangePrinterViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
