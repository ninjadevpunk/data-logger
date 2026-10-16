using Data_Logger_1._3.Commands.LogCacheCommands;
using Core.Models;
using Data_Logger_1._3.Services;
using Core.Enums;
using Core.DTOs.Identity;
using Core.DTOs.Viewer;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    internal sealed class codeViewerViewModel : ViewerViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override LogContext Context => LogContext.Coding;

        public override ICommand OKCommand { get; }

        public codeViewerViewModel(NavigationService navigationService, UserDTO userDTO, CodingViewDTO codingViewDTO) :
            base(navigationService, userDTO, codingViewDTO)
        {
            BugsFound = codingViewDTO.Bugs;
            ApplicationOpened = codingViewDTO.ApplicationOpened;

            OKCommand = new ViewerOKCommand(navigationService);
        }


        public string BugsFound { get; init; } = "0 Bugs Found";
        public string ApplicationOpened { get; init; } = "Unsuccessful Launch";
    }
}
