using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Create;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using Microsoft.Extensions.DependencyInjection;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.Services
{
    public class ViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public CreateChecklistItemViewModel MakeCreateChecklistItemViewModel(CheckListItem checklistItem)
        {
            var createCheckListViewModel = _serviceProvider.GetRequiredService<CreateCheckListViewModel>();

            return new CreateChecklistItemViewModel(createCheckListViewModel, checklistItem);
        }

        public codeCreateViewModel CreateQtCodeCreateViewModel()
        {
            return new codeCreateViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<CodingQtViewModel>(),
                "Qt",
                _serviceProvider.GetRequiredService<IDataService>()
            );
        }

        public codeCreateViewModel CreateCodeCreateViewModel()
        {
            return new codeCreateViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<CodingViewModel>(), 
                _serviceProvider.GetRequiredService<IDataService>()
            );
        }

        public codeEditViewModel CreateCodeEditViewModel(ViewModelBase viewModelBase)
        {
            return new codeEditViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<CodingViewModel>(),
                _serviceProvider.GetRequiredService<IDataService>(),
                viewModelBase
            );
        }

        public codeUpdateViewModel CreateCodeUpdateViewModel(CodeReportDeskViewModel codeReportDeskViewModel, LOG log)
        {
            return new codeUpdateViewModel(_serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<IDataService>(),
                codeReportDeskViewModel,
                log
                );
        }

        public async Task<PostItViewModel> CreatePostItViewModel(LoggerCreateViewModel loggerCreateViewModel, LOG.CATEGORY category)
        {
            var postItVM = new PostItViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<IDataService>(),
                loggerCreateViewModel,
                category
                );

            await postItVM.LoadSubjectsAsync(category);


            return postItVM;
        }

        public async Task<EF_EditPostItViewModel> Create_EF_EditPostItViewModel(ReporterUpdaterViewModel reporterUpdaterViewModel, LOG.CATEGORY category)
        {
            var ef_editPostItViewModel = new EF_EditPostItViewModel(
                _serviceProvider.GetRequiredService<NavigationService>(),
                _serviceProvider.GetRequiredService<IDataService>(),
                reporterUpdaterViewModel
            );

            await ef_editPostItViewModel.LoadSubjectsAsync(category);


            return ef_editPostItViewModel;
        }
    }

}
