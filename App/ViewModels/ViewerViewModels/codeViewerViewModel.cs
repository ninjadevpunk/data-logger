using Data_Logger_1._3.Commands.LogCacheCommands;
using Core.Models;
using Data_Logger_1._3.Services;
using Core.Enums;
using Core.DTOs.Identity;
using Core.DTOs.Viewer;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    internal class codeViewerViewModel : ViewViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.CODING;
        public override LogContext Context => LogContext.Coding;

        public codeViewerViewModel(NavigationService navigationService, UserDTO userDTO, CodingViewDTO codingViewDTO) :
            base(navigationService, userDTO, codingViewDTO)
        {
            OKCommand = new ViewerOKCommand(navigationService, Context);

            BugsFound = codingViewDTO.Bugs;
            ApplicationOpened = codingViewDTO.ApplicationOpened;
        }


        public string BugsFound { get; init; } = "0 Bugs Found";
        public string ApplicationOpened { get; init; } = "Unsuccessful Launch";
    }
}
