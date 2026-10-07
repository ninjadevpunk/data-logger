using Core.DTOs.Cache;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    /// <summary>
    /// LOG ViewModel class for a log cached in the log cache. Specialised for Qt logs.
    /// </summary>
    internal sealed class QtLOGViewModel : LOGViewModel
    {
        public override ICommand EditCommand { get; }
        public override ICommand ViewCommand { get; }

        public QtLOGViewModel(NavigationService navigationService, QtCacheDTO qtCodingCacheDTO, CodingQtViewModel codingQtViewModel,
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true) :
            base(navigationService, qtCodingCacheDTO, codingQtViewModel, dataService, cacheService, isCacheSave)
        {
            EditCommand = new EditCommand(navigationService, qtCodingCacheDTO, _timer);
            ViewCommand = new ViewCommand(navigationService, qtCodingCacheDTO);

            if (_isCacheSave)
                _cacheMaster.SaveQtCodeCache(qtCodingCacheDTO);
        }


        protected override async void AutoDeleteLogCacheItem()
        {
            CleanUp();
            DeleteCacheItemCommand.Execute(this);
        }
    }
}
