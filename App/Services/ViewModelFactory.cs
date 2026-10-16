using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Identity;
using Core.DTOs.Viewer;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt;
using Data_Logger_1._3.ViewModels.LogViewModels;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    internal class ViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;


        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        // CODING

        internal CodeLOGViewModel Create(CodingCacheDTO codingCacheDTO, CodingViewModel codingViewModel, bool isCacheSave = true) 
            => ActivatorUtilities.CreateInstance<CodeLOGViewModel>(_serviceProvider, codingCacheDTO, codingViewModel, isCacheSave);

        internal codeEditViewModel CreateCodeEditViewModel(CodingViewModel codingViewModel, CodingCacheDTO codingCacheDTO,
            LOGViewModel logViewModel, int timeRemaining) =>
            ActivatorUtilities.CreateInstance<codeEditViewModel>(_serviceProvider, codingViewModel, codingCacheDTO, logViewModel, timeRemaining);

        internal codeUpdateViewModel CreateCodeUpdateViewModel(CodeReportDeskViewModel codeReportDeskViewModel, CodingDataDTO codingDataDTO)
            => ActivatorUtilities.CreateInstance<codeUpdateViewModel>(_serviceProvider, codeReportDeskViewModel, codingDataDTO);

        internal codeViewerViewModel CreateViewerViewModel(UserDTO userDTO, CodingViewDTO codingViewDTO) 
            => ActivatorUtilities.CreateInstance<codeViewerViewModel>(_serviceProvider, userDTO, codingViewDTO);



        // PostIt

        internal CreatePostItViewModel CreatePostItViewModel(LoggerViewModel loggerViewModel)
            => ActivatorUtilities.CreateInstance<CreatePostItViewModel>(_serviceProvider, loggerViewModel);

        internal EditPostItViewModel CreateEditPostItViewModel(LoggerViewModel loggerViewModel, CacheStickyPostItViewModel cacheStickyPostItViewModel) 
            => ActivatorUtilities.CreateInstance<EditPostItViewModel>(_serviceProvider, loggerViewModel, cacheStickyPostItViewModel);

        internal UpdatePostItViewModel CreateUpdatePostItViewModel(LoggerViewModel loggerViewModel, DataStickyPostItViewModel dataStickyPostItViewModel)
            => ActivatorUtilities.CreateInstance<UpdatePostItViewModel>(_serviceProvider, loggerViewModel, dataStickyPostItViewModel);



        // Qt
        internal QtLOGViewModel Create(QtCacheDTO qtCacheDto, CodingQtViewModel codingQtViewModel, bool isCacheSave = true) 
            => ActivatorUtilities.CreateInstance<QtLOGViewModel>(_serviceProvider, qtCacheDto, codingQtViewModel, isCacheSave);



        // Android

        internal AndroidLOGViewModel Create(AndroidCacheDTO androidCacheDTO, CodingAndroidViewModel codingAndroidViewModel, bool isCacheSave = true)
            => ActivatorUtilities.CreateInstance<AndroidLOGViewModel>(_serviceProvider, androidCacheDTO, codingAndroidViewModel, isCacheSave);

    }

}
