using System;
using System.IO;

namespace Transversales.Utilitarios.Tools
{
    public static class ServerLog
    {
        private static string RootFolder
        {
            get
            {
                try
                {
                    var root = $"{AppDomain.CurrentDomain.BaseDirectory}";
                    var fechaActual = DateTime.Now.Date.ToString("dd-MMMM-yyyy");
                    var MesActual = DateTime.Now.ToString("MMMM");

                    var route = $"{root}\\ServerLogs\\{MesActual}";

                    if (!Directory.Exists(route)) Directory.CreateDirectory(route);

                    var r = $"{route}\\log_diario_{fechaActual}.txt";
                    return r;
                }
                catch (Exception ex)
                {
                    //ignored
                    throw new Exception(ex.Message);
                }
            }
        }

        public static void Add(Exception ex)
        {
            try
            {
                var innerEx = ex.InnerException?.InnerException?.InnerException?.Message ??
                                   ex.InnerException?.InnerException?.Message ??
                                   ex.InnerException?.Message ?? ex.Message;

                using (var w = File.AppendText(RootFolder))
                {
                    var logMsg = $"\n\n--------------------------------------------------------------------------------\r\n";
                    logMsg += DateTime.Now.ToString("dd-MM-yyyy - HH:mm:ss") + $" - EXCEPCION{Environment.NewLine}";
                    if (ex.Message != null) logMsg += $"Mensaje: {ex.Message } {Environment.NewLine}";
                    if (ex.InnerException != null) logMsg += $"Excepción Interna: {innerEx } {Environment.NewLine}";
                    if (ex.Source != null) logMsg += $"Fuente del error: {ex.Source} {Environment.NewLine}";
                    if (ex.StackTrace != null) logMsg += $"Seguimiento del error: { ex.StackTrace } {Environment.NewLine}";
                    logMsg += $"-------------------------------------------------------------------------------- " +
                        $"{Environment.NewLine}{Environment.NewLine}";

                    w.WriteLine(logMsg);
                }
            }
            catch (Exception exc)
            {
                //
            }
        }
    }
}
