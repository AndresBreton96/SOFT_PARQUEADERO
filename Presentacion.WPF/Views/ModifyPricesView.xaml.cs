using Presentacion.WPF.ViewModels;
using System;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using Transversales.Modelos;
using Transversales.Utilitarios.Enums;
using System.Windows.Input;
using Negocio.Contratos.Rates;

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

        public static readonly DependencyProperty SearchRatesCommandProperty =
            DependencyProperty.Register("SearchRatesCommand", typeof(ICommand), typeof(ModifyPricesView), new PropertyMetadata(null));

        public static readonly DependencyProperty DropRateCommandProperty =
            DependencyProperty.Register("DropRateCommand", typeof(ICommand), typeof(ModifyPricesView), new PropertyMetadata(null));

        public static readonly DependencyProperty UpdateRateCommandProperty =
            DependencyProperty.Register("UpdateRateCommand", typeof(ICommand), typeof(ModifyPricesView), new PropertyMetadata(null));

        public ICommand SaveRateCommand
        {
            get { return (ICommand)GetValue(SaveRateCommandProperty); }
            set { SetValue(SaveRateCommandProperty, value); }
        }

        public ICommand SearchRatesCommand
        {
            get { return (ICommand)GetValue(SearchRatesCommandProperty); }
            set { SetValue(SearchRatesCommandProperty, value); }
        }

        public ICommand DropRateCommand
        {
            get { return (ICommand)GetValue(DropRateCommandProperty); }
            set { SetValue(DropRateCommandProperty, value); }
        }

        public ICommand UpdateRateCommand
        {
            get { return (ICommand)GetValue(UpdateRateCommandProperty); }
            set { SetValue(UpdateRateCommandProperty, value); }
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

        private RatesByTime _editingRate;

        #endregion

        #region Events
        private void RateTypesCbo_LostFocus(object sender, RoutedEventArgs e)
        {
            if(RateTypesCbo.SelectedIndex == -1)
                return;

            if((RateType)RateTypesCbo.SelectedItem == RateType.HoraCarro || (RateType)RateTypesCbo.SelectedItem == RateType.HoraMoto)
            {
                RateTime.Text = "60";
                RateTime.IsEnabled = false;
                return;
            }
            RateTime.Text = "";
            RateTime.IsEnabled = true;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            StartRate();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            CancelChanges();
        }

        private void ModifyRateButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            StartRate((RatesByTime)item);
        }

        private void DeleteRateButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext;
            DropRate((RatesByTime)item);
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
                MessageBox.Show(ex.Message);
            }
        }

        private void StartRate(RatesByTime rate)
        {
            try
            {
                SearchRatesCommand.Execute(rate.RateId);
                _editingRate = ((ModifyPricesViewModel)DataContext).EditingRate;
                if (_editingRate == null)
                    return;
                RateId.Text = _editingRate.RateId.ToString();
                RateDescription.Text = _editingRate.Description;
                RateTypesCbo.SelectedItem = _editingRate.RateType;
                RateValue.Text = _editingRate.Value.ToString();
                RateTime.Text = _editingRate.Time.ToString();
                CurrentProcess = ProcessEnum.Modifying;

            }
            catch (Exception ex)
            {
                CurrentProcess = ProcessEnum.Nothing;
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveChanges()
        {
            if (string.IsNullOrEmpty(RateDescription.Text))
            {
                MessageBox.Show("Por favor ingrese una descripcion.");
                RateDescription.Focus();
                return;
            }

            if (RateTypesCbo.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un tipo de tarifa.");
                RateTypesCbo.Focus();
                return;
            }

            if (string.IsNullOrEmpty(RateValue.Text))
            {
                MessageBox.Show("Por favor ingrese un valor de la tarifa.");
                RateValue.Focus();
                return;
            }

            if (string.IsNullOrEmpty(RateTime.Text))
            {
                MessageBox.Show("Por favor ingrese un tiempo para la tarifa.");
                RateValue.Focus();
                return;
            }

            var flag = false;
            switch (CurrentProcess)
            {
                case ProcessEnum.Adding:
                    if (SaveRateCommand != null)
                    {
                        var rate = new RatesByTime()
                        {
                            RateId = 0,
                            Description = RateDescription.Text,
                            RateType = (RateType)RateTypesCbo.SelectedItem,
                            Value = Convert.ToDouble(RateValue.Text),
                            Time = Convert.ToDouble(RateTime.Text)
                        };
                        SaveRateCommand.Execute(rate);
                        flag = true;
                    }
                    break;
                case ProcessEnum.Modifying:
                    if (UpdateRateCommand != null)
                    {
                        var rate = new RatesByTime()
                        {
                            RateId = _editingRate.RateId,
                            Description = RateDescription.Text,
                            RateType = (RateType)RateTypesCbo.SelectedItem,
                            Value = Convert.ToDouble(RateValue.Text),
                            Time = Convert.ToDouble(RateTime.Text)
                        };
                        UpdateRateCommand.Execute(rate);
                        flag = true;
                    }
                    break;
            }

            if (flag)
            {
                SearchRatesCommand.Execute(null);
                CancelChanges();
                return;
            }

            MessageBox.Show("No se la inicializado el comando de búsqueda, por favor reinicie la vista e intente nuevamente.");
        }

        private void DropRate(RatesByTime rate)
        {
            if (DropRateCommand != null)
            {
                DropRateCommand.Execute(rate);

                SearchRatesCommand.Execute(null);
                CancelChanges();
                return;
            }

            MessageBox.Show("No se la inicializado el comando de búsqueda, por favor reinicie la vista e intente nuevamente.");
        }

        private void CancelChanges()
        {
            try
            {
                RateId.Text = "0";
                RateDescription.Text = "";
                RateTypesCbo.SelectedIndex = -1;
                RateValue.Text = "";
                RateTime.Text = "";
                CurrentProcess = ProcessEnum.Nothing;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
    }
}
