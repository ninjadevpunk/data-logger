using Core.Models;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public sealed class CheckListItemViewModel : ViewModelBase
    {
		private readonly CheckListItem _CheckListItem;

        public CheckListItemViewModel(CheckListItem checkListItem)
        {
			_CheckListItem = checkListItem;
            
        }




        #region Properties



        public int ViewModelID => _CheckListItem.itemID;

        public string Item => _CheckListItem.Item;




        #endregion



    }
}
