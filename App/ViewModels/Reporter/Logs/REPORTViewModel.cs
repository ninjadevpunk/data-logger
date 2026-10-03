using Core.Enums;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    /// <summary>
    /// The base class for all REPORT items to be shown in a ReportDeskViewModel.
    /// </summary>
    internal abstract class REPORTViewModel : ViewModelBase
    {
        public abstract LogContext Context { get; }
        protected NavigationService _navigationService;

        public ICommand SingleExport { get; set; } = null!;
        public ICommand EditLogCommand { get; set; } = null!;
        public ICommand ViewLogCommand { get; set; } = null!;
        public ICommand DeleteLogCommand { get; set; } = null!;


        public REPORTViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }



        #region Properties




        




        







        #endregion






        #region Member Functions



        #endregion
    }
}
