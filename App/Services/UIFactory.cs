using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.ReportPages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit.PostIt;
using Core.DTOs.Cache;
using Data_Logger_1._3.ViewModels.Dialogs.Updater.PostIt;

namespace Data_Logger_1._3.Services
{
    internal class UIFactory
    {
        private readonly IServiceProvider _serviceProvider;


        public UIFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TDLSWindow CreateDLSWindow<TDLSWindow>() where TDLSWindow : DLSWindow
        {
            return _serviceProvider.GetRequiredService<TDLSWindow>();
        }

        public T CreatePage<T>() where T : Page
        {
            return _serviceProvider.GetRequiredService<T>();
        }


        public ReporterDashboard CreateReporterPage(string vmKey)
        {
            var factory = _serviceProvider.GetRequiredService<Func<string, ReporterDashboard>>();
            return factory(vmKey);
        }

        public TPage CreatePage<TPage>(PostItViewModel postItViewModel)
            where TPage : Page
        {
            var page = _serviceProvider.GetRequiredService<TPage>();
            page.DataContext = postItViewModel;

            return page;
        }

        public TPage CreatePage<TPage, TViewModel>()
            where TPage : Page
            where TViewModel : class
        {
            var page = _serviceProvider.GetRequiredService<TPage>();
            page.DataContext = _serviceProvider.GetRequiredService<TViewModel>();

            return page;
        }

        public async Task<PostItPage> CreatePostItPage(LoggerViewModel loggerViewModel, string appName, string projectName)
        {
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
            var postItViewModel = viewModelFactory.CreatePostItViewModel(loggerViewModel);
            await postItViewModel.AutoStartAsync(appName, projectName);

            return ActivatorUtilities.CreateInstance<PostItPage>(_serviceProvider, postItViewModel); ;
        }

        public async Task<PostItPage> CreateEditPostItPage(LoggerViewModel loggerViewModel, CacheStickyPostItViewModel cacheStickyPostItViewModel,
            string appName, string projectName)
        {
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
            var postItViewModel = viewModelFactory.CreateEditPostItViewModel(loggerViewModel, cacheStickyPostItViewModel);
            await postItViewModel.AutoStartAsync(appName, projectName);
            
            return ActivatorUtilities.CreateInstance<PostItPage>(_serviceProvider, postItViewModel);
        }

        public async Task<PostItPage> CreateUpdatePostItPage(LoggerViewModel loggerViewModel, DataStickyPostItViewModel dataStickyPostItViewModel,
            string appName, string projectName)
        {
            var viewModelFactory = _serviceProvider.GetRequiredService<ViewModelFactory>();
            var postItViewModel = viewModelFactory.CreateUpdatePostItViewModel(loggerViewModel, dataStickyPostItViewModel);
            await postItViewModel.AutoStartAsync(appName, projectName);

            return ActivatorUtilities.CreateInstance<PostItPage>(_serviceProvider, postItViewModel);
        }

    }
}
