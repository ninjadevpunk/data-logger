using Data_Logger_1._3.Models;
using MVVMEssentials.ViewModels;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public class ChecklistLOGViewModel : ViewModelBase
    {
        private readonly NoteItem _checklist;



        #region Constructors


        public ChecklistLOGViewModel(NoteItem checklist)
        {
            _checklist = checklist;
        }









        #endregion




        #region Properties



        public string Subject => _checklist.Subject;

        public CheckList Items => _checklist.Checklist;

        public string Date => $"Created {_checklist.Start.ToString("d/M/yyyy HH:mm:ss")}";




        #endregion




        #region Member Functions



















        #endregion
    }
}
