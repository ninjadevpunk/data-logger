using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using System.Collections.ObjectModel;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class GraphicsLOGViewModel : LOGViewModel
    {
        public readonly GraphicsLOG _GraphicsLOG;
        public override CacheContext LOGViewModelContext => CacheContext.Graphics;



        #region Constructor


        public GraphicsLOGViewModel(GraphicsLOG graphicsLOG, LogCacheViewModel logCacheViewModel, 
            ObservableCollection<PostItViewModel> createPostItViewModels, IDataService dataService) :
            base(graphicsLOG, logCacheViewModel, createPostItViewModels, dataService)
        {
            _GraphicsLOG = graphicsLOG;
            EditCommand = new EditCommand(LOGViewModelContext, _vm._navigationService, _vm);
            ViewCommand = new ViewCommand(_vm._navigationService, LOGViewModelContext);


            _cacheMaster.SaveGraphicsViewModel(this, LOGViewModelContext);
        }

        public GraphicsLOGViewModel(GraphicsLOG graphicsLOG, LogCacheViewModel logCacheViewModel, IDataService dataService) :
            base(graphicsLOG, logCacheViewModel, dataService)
        {
            _GraphicsLOG = graphicsLOG;
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
