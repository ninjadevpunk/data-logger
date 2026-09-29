using Core.Models;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;
using static FileSys.Services.CacheMaster;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Exports a single log to a PDF.
    /// </summary>
    public class SingleExportCommand : AsyncCommandBase
    {
        private readonly PDFService _pdfService;
        private readonly LOG? _log = null;

        public LogContext Context { get; set; }


        public SingleExportCommand()
        {
        }

        public SingleExportCommand(LogContext context, PDFService pdfService)
        {
            try
            {
                _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
                Context = context;
            }
            catch (Exception ex)
            {

            }
        }

        public SingleExportCommand(LOG log, LogContext context, PDFService pdfService)
        {
            try
            {
                _pdfService = pdfService ?? throw new ArgumentNullException(nameof(pdfService));
                Context = context;
                _log = log;
            }
            catch (Exception ex)
            {

            }
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (_log != null)
                {
                    switch (Context)
                    {
                        case LogContext.Qt:
                            await _pdfService.ExportQtLogToPDF(_log);

                            break;
                        case LogContext.AndroidStudio:
                            await _pdfService.ExportASLogToPDF(_log);

                            break;
                        case LogContext.Coding:
                            await _pdfService.ExportCodingLogToPDF(_log);

                            break;
                        case LogContext.Graphics:
                            await _pdfService.ExportGraphicsLogToPDF(_log);

                            break;
                        case LogContext.Film:
                            await _pdfService.ExportFilmLogToPDF(_log);

                            break;
                        case LogContext.Flexi:
                            await _pdfService.ExportFlexiLogToPDF(_log);

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near SingleExportCommand.Execute(): {ex.Message}");
            }
        }
    }

}
