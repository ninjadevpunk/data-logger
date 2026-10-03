using Core.DTOs.Data;
using Core.Enums;
using Data_Logger_1._3.Services;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.ReporterCommands
{

    /// <summary>
    /// Exports a single log to a PDF.
    /// </summary>
    public class SingleExportCommand : AsyncCommandBase
    {
        private readonly PDFService _pdfService;
        private readonly LogDataDTO _logDataDTO;
        private readonly LogContext _context;


        public SingleExportCommand(PDFService pdfService, LogDataDTO logDataDTO, LogContext context = LogContext.Coding)
        {
            _pdfService = pdfService;
            _logDataDTO = logDataDTO;
            _context = context;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                // TODO: Handle exporting using PDFService
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred near SingleExportCommand.Execute(): {ex.Message}");
            }
        }
    }

}
