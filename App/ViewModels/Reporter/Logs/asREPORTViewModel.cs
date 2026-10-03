using Core.Enums;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    internal class asREPORTViewModel : REPORTViewModel
    {
        public override LogContext Context => LogContext.Coding;


        public asREPORTViewModel(NavigationService navigationService)
            : base(navigationService)
        {
            
        }


        #region Methods







        #endregion


    }
}
