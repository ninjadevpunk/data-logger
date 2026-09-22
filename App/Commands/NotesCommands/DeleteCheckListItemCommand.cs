using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    public class DeleteCheckListItemCommand : CommandBase
    {
        private readonly CreateCheckListViewModel _checklist;
        private readonly CreateChecklistItemViewModel _checklistItem;

        public DeleteCheckListItemCommand(CreateCheckListViewModel checkList, CreateChecklistItemViewModel createChecklistItemViewModel)
        {
            try
            {
                _checklist = checkList ?? throw new ArgumentNullException(nameof(checkList));
                _checklistItem = createChecklistItemViewModel ?? throw new ArgumentNullException(nameof(createChecklistItemViewModel));
            }
            catch (Exception ex)
            {
                //
            }
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
                //
            }

        }
    }
}
