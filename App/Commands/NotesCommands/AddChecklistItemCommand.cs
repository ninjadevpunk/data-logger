using Data.Interfaces;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    internal class AddChecklistItemCommand : AsyncCommandBase
    {
        private readonly CreateCheckListViewModel _createCheckListViewModel;
        private readonly IDataService _dataService;

        public AddChecklistItemCommand(IDataService dataService, CreateCheckListViewModel createCheckListViewModel)
        {
            _dataService = dataService;
            _createCheckListViewModel = createCheckListViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var list = _createCheckListViewModel.ChecklistItems;

                list.Add(new CreateChecklistItemViewModel(_createCheckListViewModel));

                _createCheckListViewModel.ChecklistItems = list;
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
            }
        }
    }
}
