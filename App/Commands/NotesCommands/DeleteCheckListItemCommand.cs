using Core.Utilities;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;
using System.Diagnostics;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    internal class DeleteCheckListItemCommand : CommandBase
    {
        private readonly CreateCheckListViewModel _checklist;
        private readonly CreateChecklistItemViewModel _checklistItem;

        public DeleteCheckListItemCommand(CreateCheckListViewModel checkList, CreateChecklistItemViewModel createChecklistItemViewModel)
        {
            _checklist = checkList;
            _checklistItem = createChecklistItemViewModel;
        }

        public override void Execute(object parameter)
        {

            try
            {
                if (parameter is CreateChecklistItemViewModel item)
                {
                    var list = _checklist.ChecklistItems;

                    // Delete from Checklist
                    list.Remove(item);

                    _checklist.ChecklistItems = list;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{Constants.ExceptionPrefix} DeleteCheckListItemCommand.Execute: {ex.Message}");
            }

        }
    }
}
