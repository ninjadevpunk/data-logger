using Core.DTOs.Data;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    internal class androidREPORTViewModel : REPORTViewModel
    {
        public override ICommand SingleExport { get; } = null!;

        public androidREPORTViewModel(IDataService dataService, ASReportDeskViewModel androidReportDeskViewModel, AndroidDataDTO androidDataDTO)
            : base(dataService, androidReportDeskViewModel, androidDataDTO)
        {
            // TODO: SingleExport
        }


    }
}
