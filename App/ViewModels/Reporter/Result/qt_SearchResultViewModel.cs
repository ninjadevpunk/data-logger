using Core.Enums;
using Core.Models;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.Utilities;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    /// <summary>
    /// TODO
    /// </summary>
    internal class qt_SearchResultViewModel : SearchResultViewModel
    {
        private readonly CodingLOG _QtCodingLOG;
        public override LogContext SearchResultContext => LogContext.Qt;

        public qt_SearchResultViewModel(CodingLOG codingLOG, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : 
            base(codingLOG, reportDeskViewModel, navigationService)
        {
            _QtCodingLOG = codingLOG;
            var style = Extensions.TryParsePath("path_Qt_icon");
            IconStyle = style == null ? new Style() : style;

            
        }







        #region Methods




        /// <summary>
        /// Returns the Qt Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetCodingLog => _QtCodingLOG;


        









        #endregion

    }
}
