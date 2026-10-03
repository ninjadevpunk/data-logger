using Core.Enums;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.Reporter.Logs
{
    internal class codeREPORTViewModel : REPORTViewModel
    {
        public override LogContext Context => LogContext.Coding;

        public codeREPORTViewModel(NavigationService navigationService)
            : base(navigationService)
        {
            
        }




        #region Methods






        #endregion

    }
}
