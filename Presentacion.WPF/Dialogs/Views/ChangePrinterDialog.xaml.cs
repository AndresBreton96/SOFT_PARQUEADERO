using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Transversales.Utilitarios.Enums;
using Transversales.Utilitarios.Tools;

namespace Presentacion.WPF.Dialogs.Views
{
    /// <summary>
    /// Lógica de interacción para ChangePrinterDialog.xaml
    /// </summary>
    public partial class ChangePrinterDialog : UserControl
    {
        public ChangePrinterDialog()
        {
            InitializeComponent();
            LoadPrinters();
        }

        private void LoadPrinters()
        {
            try
            {
                var printers = new List<string>();
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    printers.Add(printer);
                }

                cboPrinters.ItemsSource = printers;

                var currentPrinter = ResourcesReader.GetProperty("LocalParameters", LocalParameterEnum.PrinterName.ToString());

                if (string.IsNullOrEmpty(currentPrinter))
                    return;

                cboPrinters.SelectedItem = currentPrinter;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
