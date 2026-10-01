using Core.DTOs.Notes;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;
using System.Diagnostics;
using System.Windows;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    internal class SaveChecklistCommand : AsyncCommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly IAuthService _authService;
        private readonly IDataService _dataService;
        private readonly CreateCheckListViewModel _createCheckListViewModel;
        private readonly NOTESDashboardViewModel _notesDashboardViewModel;


        public SaveChecklistCommand(NavigationService navigationService, IAuthService authService, IDataService dataService, 
            CreateCheckListViewModel createCheckListViewModel, NOTESDashboardViewModel notesViewModel)
        {
            _navigationService = navigationService;
            _authService = authService;
            _dataService = dataService;
            _createCheckListViewModel = createCheckListViewModel;
            _notesDashboardViewModel = notesViewModel;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                var user = _authService.GetUser();

                var checkListDTO = new CheckListDTO
                {
                    UserId = user.UserId,
                    ListSubject = _createCheckListViewModel.NoteSubject,
                    CreationDate = _createCheckListViewModel.CreationDate,
                    ModifiedDate = _createCheckListViewModel.ModifiedDate,
                    Items = _createCheckListViewModel.ChecklistItems.Select(item => new CheckListItemDTO
                    {
                        IsChecked = item.IsDone,
                        Item = item.Item
                    }).ToList()
                };



                // Send to database
                var (success, checkListDataDTO) = await _dataService.CreateCheckListAsync(checkListDTO);

                if(success && checkListDataDTO != null)
                {
                    var viewCheckListDTO = new NotesViewCheckListDTO
                    {
                        NoteDTO = checkListDataDTO,
                        Subject = checkListDTO.ListSubject,
                        CreationDate = checkListDTO.CreationDate,
                        Items = checkListDTO.Items
                    };

                    var list = _notesDashboardViewModel.NoteItems;
                    list.Add(new NoteLOGViewModel(_dataService, _notesDashboardViewModel, viewCheckListDTO));

                    _notesDashboardViewModel.NoteItems = list;
                }
                else
                {
                    MessageBox.Show("An unexpected error occurred while saving the checklist. Please try again later.", "Error", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }


                await _navigationService.NavigateToNotesDashboard();
                _navigationService.SetChecklistNotesChecked(false);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                MessageBox.Show("An unexpected error occurred. Please try again later.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.WriteLine($"Exception occurred in SaveChecklistCommand.ExecuteAsync: {ex.Message}");
            }
        }
    }
}
