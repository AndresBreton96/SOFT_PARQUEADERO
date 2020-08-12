using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using Transversales.Modelos;
using Transversales.Utilitarios.Enums;
using System.Windows.Input;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para ModifyPricesView.xaml
    /// </summary>
    public partial class ModifyPricesView : UserControl
    {
        #region Constructor
        public ModifyPricesView()
        {
            InitializeComponent();

            var rateTypes = Enum.GetValues(typeof(RateType)).Cast<RateType>();

            RateTypesCbo.ItemsSource = rateTypes;
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty SaveRateCommandProperty =
            DependencyProperty.Register("SaveRateCommand", typeof(ICommand), typeof(ModifyPricesView), new PropertyMetadata(null));

        public ICommand SaveRateCommand
        {
            get { return (ICommand)GetValue(SaveRateCommandProperty); }
            set { SetValue(SaveRateCommandProperty, value); }
        }

        private ProcessEnum _currentProcess;
        private ProcessEnum CurrentProcess
        {
            get
            {
                return _currentProcess;
            }
            set
            {
                _currentProcess = value;
                switch (_currentProcess)
                {
                    case ProcessEnum.Adding:
                    case ProcessEnum.Modifying:
                        RateDescription.IsEnabled = true;
                        RateTypesCbo.IsEnabled = true;
                        RateValue.IsEnabled = true;
                        RateTime.IsEnabled = true;
                        RateDescription.Focus();

                        AddBtn.Visibility = Visibility.Hidden;
                        ChangesPnl.Visibility = Visibility.Visible;
                        break;
                    case ProcessEnum.Nothing:
                    default:
                        RateDescription.IsEnabled = false;
                        RateTypesCbo.IsEnabled = false;
                        RateValue.IsEnabled = false;
                        RateTime.IsEnabled = false;

                        AddBtn.Visibility = Visibility.Visible;
                        ChangesPnl.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        #endregion

        #region Events
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            StartRate();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SaveRateCommand != null)
            {
                var rate = new Rates();
                try
                {
                    SaveRateCommand.Execute(rate);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelChanges();
        }

        #endregion

        #region Methods
        private void StartRate()
        {
            try
            {
                CurrentProcess = ProcessEnum.Adding;
            }
            catch (Exception ex)
            {
                CurrentProcess = ProcessEnum.Nothing;
            }
        }

        private void StartRate(Rates rate)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void CancelChanges()
        {
            try
            {
                CurrentProcess = ProcessEnum.Nothing;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
