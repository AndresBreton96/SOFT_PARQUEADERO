using MaterialDesignThemes.Wpf;
using Presentacion.WPF.Dialogs.Views;
using Presentacion.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Transversales.Modelos.Exceptions;
using Transversales.Modelos.RegistrationEntries;
using Transversales.Utilitarios.Printing;

namespace Presentacion.WPF.Views
{
    /// <summary>
    /// Lógica de interacción para RegisterDepartureView.xaml
    /// </summary>
    public partial class RegisterDepartureView : UserControl
    {
        #region Constructor
        public RegisterDepartureView()
        {
            InitializeComponent();

            TxtBackground.ImageSource = new BitmapImage(new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\Ppar.jpg"));
        }

        #endregion

        #region Variables
        public static readonly DependencyProperty CalculatePriceCommandProperty =
            DependencyProperty.Register("CalculatePriceCommand", typeof(ICommand), typeof(RegisterDepartureView), new PropertyMetadata(null));

        public static readonly DependencyProperty SaveDepartureTicketCommandProperty =
            DependencyProperty.Register("SaveDepartureTicketCommand", typeof(ICommand), typeof(RegisterDepartureView), new PropertyMetadata(null));

        public static readonly DependencyProperty RunDialogCommandProperty =
            DependencyProperty.Register("RunDialogCommand", typeof(ICommand), typeof(RegisterDepartureView), new PropertyMetadata(null));

        public static readonly DependencyProperty CreateBillCommandProperty =
            DependencyProperty.Register("CreateBillCommand", typeof(ICommand), typeof(RegisterDepartureView), new PropertyMetadata(null));

        public ICommand CalculatePriceCommand
        {
            get { return (ICommand)GetValue(CalculatePriceCommandProperty); }
            set { SetValue(CalculatePriceCommandProperty, value); }
        }

        public ICommand SaveDepartureTicketCommand
        {
            get { return (ICommand)GetValue(SaveDepartureTicketCommandProperty); }
            set { SetValue(SaveDepartureTicketCommandProperty, value); }
        }

        public ICommand RunDialogCommand
        {
            get { return (ICommand)GetValue(RunDialogCommandProperty); }
            set { SetValue(RunDialogCommandProperty, value); }
        }

        public ICommand CreateBillCommand
        {
            get { return (ICommand)GetValue(CreateBillCommandProperty); }
            set { SetValue(CreateBillCommandProperty, value); }
        }


        #endregion

        #region Events
        private void PlatesTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    CreateBill();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Methods
        private void CreateBill()
        {
            try
            {

                if (CalculatePriceCommand != null)
                {
                    CalculatePriceCommand.Execute(PlatesTextBox.Text);
                    ((RegisterDepartureViewModel)DataContext).Plates = PlatesTextBox.Text;

                    //if ((((RegisterDepartureViewModel)DataContext).Fractions > 1 && ((RegisterDepartureViewModel)DataContext).Hours == 0) || (((RegisterDepartureViewModel)DataContext).Fractions != 0 && ((RegisterDepartureViewModel)DataContext).Hours != 0))                 
                    //{
                    //    if (RunDialogCommand != null)
                    //    {
                    //        RunDialogCommand.Execute(null);
                    //        PlatesTextBox.Text = string.Empty;
                    //        return;
                    //    }
                    //    MessageBox.Show("No se ha podido inicializar el comando para mostrar el diálogo.");
                    //    ((RegisterDepartureViewModel)DataContext).Plates = null;
                    //    PlatesTextBox.Text = string.Empty;
                    //    return;
                    //}

                    if (SaveDepartureTicketCommand != null)
                    {
                        var ticket = new Tickets()
                        {
                            TicketId = 0,
                            EntryTicketId = 0,
                            EntryType = EntryType.Salida,
                            LicensePlate = PlatesTextBox.Text,
                            EntryDate = ((RegisterDepartureViewModel)DataContext).DepartureTime
                        };

                        SaveDepartureTicketCommand.Execute(ticket);
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido inicializar el comando para crear guardar el ticket de salida.");
                        ((RegisterDepartureViewModel)DataContext).Plates = null;
                        PlatesTextBox.Text = string.Empty;
                        return;
                    }

                    if (CreateBillCommand != null)
                    {
                        CreateBillCommand.Execute(PlatesTextBox.Text);
                        ((RegisterDepartureViewModel)DataContext).Plates = null;
                        PlatesTextBox.Text = string.Empty;

                        var priceMessageDialog = new ChargedPriceDialog()
                        {
                            Message = { Text = ((RegisterDepartureViewModel)DataContext).Price.ToString("C") }
                        };

                        DialogHost.Show(priceMessageDialog, "RootDialog");
                        return;
                    }

                    MessageBox.Show("No se ha podido inicializar el comando para crear la factura.");
                    ((RegisterDepartureViewModel)DataContext).Plates = null;
                    PlatesTextBox.Text = string.Empty;

                    return;

                }
                MessageBox.Show("No se ha podido inicializar el comando para calcular el precio.");
                ((RegisterDepartureViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;

            }
            catch (TicketNotFoundException)
            {
                ((RegisterDepartureViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
            catch (EntryTicketNotFoundException)
            {
                ((RegisterDepartureViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
            catch (DepartureTicketAlreadyRegistered)
            {
                ((RegisterDepartureViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
            catch (Exception)
            {
                ((RegisterDepartureViewModel)DataContext).Plates = null;
                PlatesTextBox.Text = string.Empty;
                return;
            }
        }

        #endregion
    }
}
