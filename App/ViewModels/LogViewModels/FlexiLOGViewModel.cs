using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class FlexiLOGViewModel : LOGViewModel
    {
        public FlexiNotesLOG _FlexiLOG;
        public override CacheContext LOGViewModelContext => CacheContext.Flexi;



        #region Constructors



        public FlexiLOGViewModel(FlexiNotesLOG flexiLOG, LogCacheViewModel logCacheViewModel, 
            ObservableCollection<PostItViewModel> createPostItViewModels, IDataService dataService) :
            base(flexiLOG, logCacheViewModel, createPostItViewModels, dataService)
        {
            _FlexiLOG = flexiLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, LOGViewModelContext);

            _cacheMaster.SaveFlexiViewModel(this, LOGViewModelContext);
        }
        public FlexiLOGViewModel(FlexiNotesLOG flexiLOG, LogCacheViewModel logCacheViewModel, IDataService dataService) :
            base(flexiLOG, logCacheViewModel, dataService)
        {
            _FlexiLOG = flexiLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, LOGViewModelContext);
        }


        #endregion





        #region Properties








        #endregion



        #region Member Functions



        protected override void DeleteCacheItem()
        {
            DeleteCacheFile(StartAsID, LOGViewModelContext);
            DeleteCacheItemCommand.Execute(this);
        }





        #endregion
    }
}
