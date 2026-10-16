using System.Windows;
using Core.DTOs.Notes;
using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.LogViewModels;
using MVVMEssentials.Commands;

namespace Data_Logger_1._3.Commands.NotesCommands
{
    internal class SaveNoteCommand : AsyncCommandBase
    {
        protected readonly NavigationService _navigationService;
        protected readonly IAuthService _authService;
        protected readonly IDataService _dataService;
        protected readonly CreateNoteViewModel _createNoteViewModel;
        protected readonly NOTESDashboardViewModel _notesViewModel;


        public SaveNoteCommand(NavigationService navigationService, IAuthService authService, IDataService dataService, 
            CreateNoteViewModel createNoteViewModel, NOTESDashboardViewModel notesViewModel)
        {
            _navigationService = navigationService;
            _authService = authService;
            _dataService = dataService;
            _createNoteViewModel = createNoteViewModel;
            _notesViewModel = notesViewModel;
        }


        protected override async Task ExecuteAsync(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(_createNoteViewModel.NoteContent))
                {
                    MessageBox.Show("Your note is empty. It will NOT be stored.", "Note Empty", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var user = _authService.GetUser();
                    var noteDTO = new NoteDTO
                    {
                        UserId = user.Id,
                        Subject = _createNoteViewModel.NoteSubject,
                        NoteContent = _createNoteViewModel.NoteContent,
                        CreationDate = _createNoteViewModel.CreationDate,
                        ModifiedDate = _createNoteViewModel.ModifiedDate
                    };

                    // Send to database
                    var (success, noteDataDTO) = await _dataService.CreateNoteAsync(noteDTO);

                    if(success && noteDataDTO != null)
                    {
                        var notesViewItemDTO = new NotesViewItemDTO
                        {
                            NoteDTO = noteDataDTO,
                            Subject = noteDataDTO.Subject,
                            Content = noteDataDTO.NoteContent,
                            CreationDate = noteDataDTO.CreationDate
                        };

                        var list = _notesViewModel.NoteItems;
                        list.Add(new NoteLOGViewModel(_dataService, _notesViewModel, notesViewItemDTO));

                        _notesViewModel.NoteItems = list;
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while saving the note. Please try again later.", "Error", MessageBoxButton.OK, 
                            MessageBoxImage.Error);
                    }


                }

                await _navigationService.NavigateToNotesDashboard();
                _navigationService.SetGenericNotesChecked(false);
            }
            catch (Exception ex)
            {
                await _dataService.HandleExceptionAsync(ex);
                MessageBox.Show("An error occurred. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
