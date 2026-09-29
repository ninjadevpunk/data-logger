using Core.DTOs.Identity;
using Core.DTOs.Viewer;
using Data_Logger_1._3.Services;

namespace Data_Logger_1._3.ViewModels.ViewerViewModels
{
    internal class ViewViewModelFactory
    {
        private readonly NavigationService _navigationService;

        public ViewViewModelFactory(NavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "ViewViewModelFactory");
        }

        public codeViewerViewModel Create(UserDTO userDTO, CodingViewDTO codingViewDTO) =>
            new codeViewerViewModel(_navigationService, userDTO, codingViewDTO);
    }
}
