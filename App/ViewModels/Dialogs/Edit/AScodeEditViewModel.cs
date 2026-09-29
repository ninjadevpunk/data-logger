using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.LogViewModels;
using FileSys.Interfaces;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.Dialogs.Edit
{
    internal class AScodeEditViewModel : AScodeCreateViewModel
    {
        private readonly AndroidLOGViewModel _androidLOGViewModel;

        public AScodeEditViewModel(NavigationService navigationService, LogCacheViewModel logCacheViewModel, IDataService dataService, 
            ViewModelBase viewModelBase, ICacheService cacheService, CodingAnnotateService codingAnnotateService) : 
                base(navigationService, logCacheViewModel, dataService, cacheService, codingAnnotateService)
        {
            _androidLOGViewModel = (AndroidLOGViewModel)viewModelBase;

            AnnotateCommand = null;
            //EditCommand = new EditAnnotateCommand();
        }
    }
}
