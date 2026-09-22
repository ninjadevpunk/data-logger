using Data_Logger_1._3.Commands.LoggerCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    public class graphicsEditViewModel : graphicCreateViewModel
    {
        private readonly GraphicsLOGViewModel _graphicsLOGViewModel;
        public graphicsEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService, 
            ViewModelBase viewModelBase) : base(navigationService, logCacheViewModel, dataService)
        {
            _graphicsLOGViewModel = (GraphicsLOGViewModel)viewModelBase;

            AnnotateCommand = null;
            EditCommand = new AnnotateCommand(ActionType.Edit, Context, _navigationService, this, _logCacheViewModel, _dataService, 
                _graphicsLOGViewModel);
        }
    }
}
