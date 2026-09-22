using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class AndroidLOGViewModel : LOGViewModel
    {
        public readonly AndroidCodingLOG _AndroidCodingLOG;
        public override CacheContext LOGViewModelContext => CacheContext.AndroidStudio;





        #region Constructors



        public AndroidLOGViewModel(AndroidCodingLOG androidCodingLOG, LogCacheViewModel logCacheViewModel, 
            ObservableCollection<PostItViewModel> createPostItViewModels, IDataService dataService) :
                base(androidCodingLOG, logCacheViewModel, createPostItViewModels, dataService)

        {
            _AndroidCodingLOG = androidCodingLOG;
            EditCommand = new EditCommand(CacheContext.AndroidStudio, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, LOGViewModelContext);

            _cacheMaster.SaveASViewModel(this, LOGViewModelContext);
        }

        public AndroidLOGViewModel(AndroidCodingLOG androidCodingLOG, LogCacheViewModel logCacheViewModel, IDataService dataService) :
            base(androidCodingLOG, logCacheViewModel, dataService)
        {
            _AndroidCodingLOG = androidCodingLOG;
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
