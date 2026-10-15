using Core.Interfaces;
using Data.Interfaces;
using Data_Logger_1._3.Commands.PostItCommands;
using Data_Logger_1._3.Services;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.Dialogs
{
    /// <summary>
    /// Represents a base view model for coding reporter logger dialogs, providing common functionality for updating logs in the database.
    /// </summary>
    internal abstract class CodingReporterLoggerViewModel : CodingLoggerViewModel
    {
        public override ICommand AddPostItCommand => new AddDataPostItCommand(_navigationService, this);

        /// <summary>
        /// Updates a log in the database.
        /// </summary>
        public abstract ICommand UpdateCommand { get; }

        public CodingReporterLoggerViewModel(NavigationService navigationService, IAuthService authService, 
            IDataService dataService) 
            : base(navigationService, authService, dataService)
        {
        }

    }
}
