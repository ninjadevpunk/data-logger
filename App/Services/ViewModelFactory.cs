using Core.DTOs.Cache;
using Core.DTOs.Data;
using Core.DTOs.Identity;
using Core.DTOs.Viewer;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Data_Logger_1._3.ViewModels.Dialogs.Updater;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.ViewerViewModels;
using FileSys.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Data_Logger_1._3.Services
{
    internal class ViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly NavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly ICacheService _cacheService;


        internal ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _navigationService = _serviceProvider.GetRequiredService<NavigationService>();
            _authService = _serviceProvider.GetRequiredService<IAuthService>();
            _dataService = _serviceProvider.GetRequiredService<IDataService>();
            _cacheService = _serviceProvider.GetRequiredService<ICacheService>();
        }



        internal codeEditViewModel CreateCodeEditViewModel(CodingViewModel codingViewModel, CodingCacheDTO codingCacheDTO) => 
            new codeEditViewModel(_navigationService, codingViewModel, _dataService, codingCacheDTO, _authService, _cacheService);

        internal codeUpdateViewModel CreateCodeUpdateViewModel(CodeReportDeskViewModel codeReportDeskViewModel, CodingDataDTO codingDataDTO)
            => new(_navigationService, _authService, _dataService, codeReportDeskViewModel, codingDataDTO);

        internal codeViewerViewModel CreateViewerViewModel(UserDTO userDTO, CodingViewDTO codingViewDTO) => new(_navigationService, 
            userDTO, codingViewDTO);

        internal EditPostItViewModel CreateEditPostItViewModel(LoggerViewModel loggerViewModel, PostItCacheDTO postItCacheDTO) =>
            new(_navigationService, _dataService, loggerViewModel, postItCacheDTO);
    }

}
