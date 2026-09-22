using Data_Logger_1._3.Commands.NotesCommands;
using Data_Logger_1._3.Models;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    public class CreateChecklistItemViewModel : ViewModelBase
    {
        public readonly CheckListItem _CheckListItem;

        public ICommand DeleteItem {  get; set; }


        #region Constructors



        public CreateChecklistItemViewModel()
        {
            //
        }

        public CreateChecklistItemViewModel(CreateCheckListViewModel createCheckListViewModel, CheckListItem checkListItem)
        {
            _CheckListItem = checkListItem;
            DeleteItem = new DeleteCheckListItemCommand(createCheckListViewModel, this);

            Item = $"Item {createCheckListViewModel.ChecklistItems.Count+1}";
        }




        #endregion




        #region Properties



        private bool isDone;
        public bool IsDone
        {
            get
            {
                return isDone;
            }
            set
            {
                isDone = value;
                OnPropertyChanged(nameof(IsDone));
            }
        }

        private string item;
        public string Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                OnPropertyChanged(nameof(Item));
            }
        }





        #endregion




        #region Member Functions























        #endregion

    }
}
