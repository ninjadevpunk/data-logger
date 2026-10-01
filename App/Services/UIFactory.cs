using Data_Logger_1._3.Views.Dialogs;
using Data_Logger_1._3.Views.ReportPages;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;
using Data_Logger_1._3.ViewModels.Reporter.Updater;
using Data_Logger_1._3.ViewModels.Dialogs;

namespace Data_Logger_1._3.Services
{
    internal class UIFactory
    {
        private readonly IServiceProvider _serviceProvider;
        

        public UIFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public T CreatePage<T>() where T : Page, new()
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public ReporterDashboard CreateReporterPage(string vmKey)
        {
            var factory = _serviceProvider.GetRequiredService<Func<string, ReporterDashboard>>();
            return factory(vmKey);
        }

        public TPage CreatePage<TPage>(PostItViewModel postItViewModel)
            where TPage : Page, new()
        {
            var page = _serviceProvider.GetRequiredService<TPage>();
            page.DataContext = postItViewModel;

            return page;
        }

        public PostItPage CreatePostItPage(PostItViewModel postItViewModel)
        {
            var page = new PostItPage(postItViewModel);
            return page;
        }

        public PostItPage Create_EF_PostItPage(EF_EditPostItViewModel editPostItViewModel)
        {
            var page = new PostItPage(editPostItViewModel);
            return page;
        }

    }
}
