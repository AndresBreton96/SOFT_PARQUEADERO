using Microsoft.Win32;
using SelectPdf;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using Transversales.Modelos.RegistrationEntries;
using Transversales.Modelos.VehiclesRegistration;
using Transversales.Utilitarios.Enums;
using Transversales.Utilitarios.Tools;

namespace Transversales.Utilitarios.Printing
{
    public static class PrintService
    {
        public static void GenerateDepartureTicket(Bills bill)
        {
            var styles = @"
    <style>
        body{          
            margin-top: 600px;  
            margin-left: -250px;
            width: 2000px;
            transform: rotate(90deg);
        }

        .bordes{
            border: solid 3px #000000;
            border-radius: 10px;
            padding: 0px;
            margin: 60px 0px;
            overflow: hidden;
        }
        .bordes table{
            width: 100%;
            padding: 0px;
            margin: 0px;
            border-collapse: collapse;
            text-align: center;
        }

        .encabezadoTabla{
            text-align: center;
            background-color: #c2c2c2;
            border-bottom: solid 3px #000000;
            font-size: 60px;
        }

        .bordeDerecho{
            border-right:  solid 3px #000000;
        }

        .contenidoTabla{
            text-align: center;
            font-size: 100px;
        }

    </style>";

            var document = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Ticket</title>

    {styles}
</head>
<body>
    <div style='text-align: center;'> <!--LOGO-->
        <table style='width: 100%; border-collapse: collapse;'>
            <tr>
                <td width='50%'>
                    <img src='{AppDomain.CurrentDomain.BaseDirectory}DAYAs.jpg' alt='' width='100%'></td>
                <td style='font-size: 50px;'>
                    Luz Yaneth Marin Franco<br>
                    Nit. 63.342.807 - 7<br>
                    NO responsable de Iva<br>
                    Carrera 6 No. 10-14<br>
                    Piedecuesta - Stder<br>
                    <b>Tel: 654 2389</b></td>
            </tr>
            <tr>
                <td colspan='2' style='text-align: left; font-size: 60px;'>Ticket No.: <b>{bill.Consecutive}</b></td>
            </tr> <!--INFORMACION-->
        </table>
    </div>
    <div class='bordes'>
        <table style='height: 140px;'>
            <thead class='encabezadoTabla'>
                <td style='width: 33%;' class='bordeDerecho'>Hora Entrada</td>
                <td style='width: 33%;' class='bordeDerecho'>Hora Salida</td>
                <td style='width: 33%;' >Fecha</td>
            </thead>
            <tr>
                <td class='bordeDerecho contenidoTabla'>{bill.EntryDate.ToShortTimeString()}</td>
                <td class='bordeDerecho contenidoTabla'>{bill.DepartureDate.ToShortTimeString()}</td>
                <td class='contenidoTabla'>{bill.BillDate.ToShortDateString()}</td>
            </tr>
        </table>
    </div>
    <div class='bordes'>
        <table style='height: 140px;'>
            <thead class='encabezadoTabla'>
                <td style='width: 50%;' class='bordeDerecho'>Placas del Vehículo</td>
                <td style='width: 50%;' >Valor Total</td>
            </thead>
            <tr>
                <td class='bordeDerecho contenidoTabla' style='font-size: 120px;'>{bill.LicensePlate}</td>
                <td class='contenidoTabla' style='font-size: 120px;'>${bill.Rate:#,###,##0}</td>
            </tr>
        </table>
    </div>
    <div class='bordes' style='background-color: #c2c2c2; padding: 0px; margin: 60px 0px; line-height: 2px; height: 150px;'>
        <p style='font-size: 50px; text-align: center;'><b>GRACIAS POR VISITARNOS</b></p>
        <p style='font-size: 48px; text-align: center;'>VER CONDICIONES DE ESTE CONTRATO AL RESPALDO</p>
    </div>
    
</body>
</html>
";

            var documentRoute = $"{AppDomain.CurrentDomain.BaseDirectory}" + bill.Consecutive + ".pdf";

            SaveDocument(document, documentRoute);

            PrintTicket(documentRoute, ResourcesReader.GetProperty("LocalParameters", LocalParameterEnum.PrinterName.ToString()));

            if (File.Exists(documentRoute))
                File.Delete(documentRoute);
        }

        public static void GenerateEntryTicket(Tickets ticket)
        {
            var styles = @"
    <style>
        body{          
            margin-top: 600px;  
            margin-left: -250px;
            width: 2000px;
            transform: rotate(90deg);
        }

        .bordes{
            border: solid 3px #000000;
            border-radius: 10px;
            padding: 0px;
            margin: 60px 0px;
            overflow: hidden;
        }
        .bordes table{
            width: 100%;
            padding: 0px;
            margin: 0px;
            border-collapse: collapse;
            text-align: center;
        }

        .encabezadoTabla{
            text-align: center;
            background-color: #c2c2c2;
            border-bottom: solid 3px #000000;
            font-size: 60px;
        }

        .bordeDerecho{
            border-right:  solid 3px #000000;
        }

        .contenidoTabla{
            text-align: center;
            font-size: 100px;
        }

        .barcode{
            font-family: 'Libre Barcode 39';
            padding: 0px;
            margin: 0px;
            font-size: 400px;
            margin-left: 200px;
            margin-top: 80px;
        }

    </style>";

            var document = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Ticket</title>

    {styles}
</head>
<body>
    <div style='text-align: center;'> <!--LOGO-->
        <table style='width: 100%; border-collapse: collapse;'>
            <tr>
                <td width='50%'>
                    <img src='{AppDomain.CurrentDomain.BaseDirectory}DAYAs.jpg' alt='' width='100%'></td>
                <td style='font-size: 50px;'>
                    Luz Yaneth Marin Franco<br>
                    Nit. 63.342.807 - 7<br>
                    NO responsable de Iva<br>
                    Carrera 6 No. 10-14<br>
                    Piedecuesta - Stder<br>
                    <b>Tel: 654 2389</b></td>
            </tr>
        </table>
    </div>
    <div class='bordes'>
        <table style='height: 140px;'>
            <thead class='encabezadoTabla'>
                <td style='width: 33%;' class='bordeDerecho'>Hora Entrada</td>
                <td style='width: 33%;' class='bordeDerecho'>Fecha Entrada</td>
                <td style='width: 33%;' >Placas del Vehículo</td>
            </thead>
            <tr>
                <td class='bordeDerecho contenidoTabla'>{ticket.EntryDate.ToShortTimeString()}</td>
                <td class='bordeDerecho contenidoTabla'>{ticket.EntryDate.ToShortTimeString()}</td>
                <td class='contenidoTabla'>{ticket.LicensePlate}</td>
            </tr>
        </table>
    </div>
    <div class='bordes' style='height: 450px;'>
        <p class='barcode'>*{ticket.LicensePlate}*</p>
    </div>
    <div class='bordes' style='background-color: #c2c2c2; padding: 0px; margin: 60px 0px; line-height: 2px; height: 150px;'>
        <p style='font-size: 50px; text-align: center;'><b>GRACIAS POR VISITARNOS</b></p>
        <p style='font-size: 48px; text-align: center;'>VER CONDICIONES DE ESTE CONTRATO AL RESPALDO</p>
    </div>
    
</body>
</html>
";

            var documentRoute = $"{AppDomain.CurrentDomain.BaseDirectory}" + ticket.LicensePlate + ".pdf";

            SaveDocument(document, documentRoute);

            PrintTicket(documentRoute, ResourcesReader.GetProperty("LocalParameters", LocalParameterEnum.PrinterName.ToString()));

            if (File.Exists(documentRoute))
                File.Delete(documentRoute);
        }

        private static void SaveDocument(string document, string documentRoute)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.Custom;
            SizeF sizef = new SizeF(227, 842);
            converter.Options.PdfPageCustomSize = sizef;

            int footerHeight = 0;
            converter.Options.DisplayFooter = false;
            converter.Options.DisplayHeader = false;
            converter.Footer.Height = footerHeight;
            converter.Options.MarginBottom = 0;
            converter.Options.MarginLeft = 2;
            converter.Options.MarginRight = 2;
            converter.Options.MarginTop = 0;

            PdfDocument doc = converter.ConvertHtmlString(document);

            doc.Save(documentRoute);
            doc.Close();
        }

        private static bool PrintTicket(string documentRoute, string printerName)
        {
            try

            {
                string applicationPath = "";

                var printApplicationRegistryPaths = new[]
                {
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Acrobat.exe",
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\AcroRD32.exe"
                };

                foreach (var printApplicationRegistryPath in printApplicationRegistryPaths)
                {
                    using (var regKeyAppRoot = Registry.LocalMachine.OpenSubKey(printApplicationRegistryPath))
                    {
                        if (regKeyAppRoot == null)
                        {
                            continue;
                        }

                        applicationPath = (string)regKeyAppRoot.GetValue(null);

                        if (string.IsNullOrEmpty(applicationPath))
                        {
                            return false;
                        }
                    }
                }

                // Print to Acrobat
                const string flagNoSplashScreen = "/s";
                const string flagOpenMinimized = "/h";

                var flagPrintFileToPrinter = string.Format("/t \"{0}\" \"{1}\"", documentRoute, printerName);

                var args = string.Format("{0} {1} {2}", flagNoSplashScreen, flagOpenMinimized, flagPrintFileToPrinter);

                var startInfo = new ProcessStartInfo
                {
                    FileName = applicationPath,
                    Arguments = args,
                    CreateNoWindow = true,
                    ErrorDialog = false,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                var process = Process.Start(startInfo);

                // Close Acrobat regardless of version
                if (process != null)
                {
                    process.WaitForInputIdle();
                    process.CloseMainWindow();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
