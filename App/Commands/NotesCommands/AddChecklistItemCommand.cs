using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class AddChecklistItemCommand : CommandBase
    {
        private readonly CreateCheckListViewModel _checklist;
        private readonly ViewModelFactory _viewModelFactory;

        public AddChecklistItemCommand(CreateCheckListViewModel viewModel)
        {
            _checklist = viewModel;
        }

        public AddChecklistItemCommand(CreateCheckListViewModel viewModel, ViewModelFactory viewModelFactory)
        {
            _checklist = viewModel;
            _viewModelFactory = viewModelFactory;
        }

        public override void Execute(object parameter)
        {
            var list = _checklist.ChecklistItems;
            list.Add(_viewModelFactory.MakeCreateChecklistItemViewModel(new CheckListItem(false, "")));

            _checklist.ChecklistItems = list;
        }
    }
}
