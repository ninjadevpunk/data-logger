using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    public class codeEditViewModel : codeCreateViewModel
    {
        private readonly QtLOGViewModel _qtLOGViewModel;
        private readonly CodeLOGViewModel _codeLOGViewModel;

        public codeEditViewModel()
        {

        }

        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService, 
            ViewModelBase viewModelBase)
            : base(navigationService, logCacheViewModel, dataService)
        {

            _codeLOGViewModel = (CodeLOGViewModel)viewModelBase;

            StartDate = _codeLOGViewModel._CodeLOG.Start;
            StartHours = _codeLOGViewModel._CodeLOG.Start.Hour;
            StartMinutes = _codeLOGViewModel._CodeLOG.Start.Minute;
            StartSeconds = _codeLOGViewModel._CodeLOG.Start.Second;
            StartMilliseconds = _codeLOGViewModel._CodeLOG.Start.Millisecond;

            EndDate = _codeLOGViewModel._CodeLOG.End;
            EndHours = _codeLOGViewModel._CodeLOG.End.Hour;
            EndMinutes = _codeLOGViewModel._CodeLOG.End.Minute;
            EndSeconds = _codeLOGViewModel._CodeLOG.End.Second;
            EndMilliseconds = _codeLOGViewModel._CodeLOG.End.Millisecond;


            AnnotateCommand = null;
            EditCommand = new AnnotateCommand(ActionType.Edit, Context, _navigationService, this, _logCacheViewModel, _dataService, 
                _codeLOGViewModel);

        }

        public codeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, string app, IDataService dataService, 
            ViewModelBase viewModelBase)
            : base(navigationService, logCacheViewModel, app, dataService)
        {
            if (app == "Qt")
            {
                _qtLOGViewModel = (QtLOGViewModel)viewModelBase;
                EditCommand = new AnnotateCommand(ActionType.Edit, CacheContext.Qt, _navigationService, this, _logCacheViewModel, 
                    _dataService, _qtLOGViewModel);
            }
            else
            {
                AnnotateCommand = null;
                EditCommand = new AnnotateCommand(ActionType.Edit, Context, _navigationService, this, _logCacheViewModel, _dataService, 
                    _codeLOGViewModel);
            }

        }


    }
}
