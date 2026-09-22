using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    public class qt_SearchResultViewModel : SearchResultViewModel
    {
        private readonly CodingLOG _QtCodingLOG;
        public override CacheContext SearchResultContext => CacheContext.Qt;

        public qt_SearchResultViewModel(CodingLOG codingLOG, ReportDeskViewModel reportDeskViewModel, NavigationService navigationService) : 
            base(codingLOG, reportDeskViewModel, navigationService)
        {
            _QtCodingLOG = codingLOG;
            var style = TryParsePath("path_Qt_icon");
            IconStyle = style == null ? new Style() : style;

            ViewLogCommand = new ViewCommand(navigationService, SearchResultContext, ViewType.Log, codingLOG);
            EditLogCommand = new EditLogCommand();
            DeleteLogCommand = new DeleteLogCommand();
        }







        #region Methods




        /// <summary>
        /// Returns the Qt Coding log injected into the ViewModel.
        /// </summary>
        public CodingLOG GetCodingLog => _QtCodingLOG;


        









        #endregion

    }
}
