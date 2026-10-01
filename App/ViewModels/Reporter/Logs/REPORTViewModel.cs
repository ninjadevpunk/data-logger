using Core.Enums;
using Data_Logger_1._3.Services;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Reporter
{
    /// <summary>
    /// The base class for all REPORT items to be shown in a ReportDeskViewModel.
    /// </summary>
    internal abstract class REPORTViewModel : ViewModelBase
    {
        public abstract LogContext Context { get; }
        protected NavigationService _navigationService;

        public ICommand SingleExport { get; set; }

        public ICommand EditLogCommand { get; set; }

        public ICommand ViewLogCommand { get; set; }

        public ICommand DeleteLogCommand { get; set; }


        public REPORTViewModel()
        {
            
        }



        #region Properties




        




        







        #endregion






        #region Member Functions



        #endregion
    }
}
