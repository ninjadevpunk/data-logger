using Data_Logger_1._3.Commands.NotesCommands;
using MVVMEssentials.ViewModels;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    internal class CreateChecklistItemViewModel : ViewModelBase
    {
        public ICommand DeleteItem { get; set; } = null!;


        #region Constructors




        public CreateChecklistItemViewModel(CreateCheckListViewModel createCheckListViewModel)
        {
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

        private string item = string.Empty;
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
