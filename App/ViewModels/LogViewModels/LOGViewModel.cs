using Core.DTOs.Cache;
using Core.Enums;
using Core.Utilities;
using Data.Interfaces;
using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using FileSys.Interfaces;
using MVVMEssentials.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    /// <summary>
    /// Represents a log DTO in the UI.
    /// </summary>
    internal abstract class LOGViewModel : ViewModelBase
    {
        protected readonly LogCacheDTO _logCacheDTO;
        protected readonly LogCacheViewModel _logCacheViewModel;
        protected readonly NavigationService _navigationService;
        protected readonly IDataService _dataService;
        protected readonly ICacheService _cacheMaster;
        protected LogContext LOGViewModelContext => _logCacheDTO.Context;
        protected readonly bool _isCacheSave;



        public bool IsDisposed { get; set; } = false;

        public Timer? _timer;

        public ICommand EditCommand { get; set; } = null!;

        public ICommand ViewCommand { get; set; } = null!;

        public ICommand DeleteCacheItemCommand { get; set; } = null!;

        public ICommand QuickDeleteCacheItemCommand { get; set; } = null!;





        #region Constructors



        public LOGViewModel(NavigationService navigationService, LogCacheDTO logCacheDTO, LogCacheViewModel logCacheViewModel, 
            IDataService dataService, ICacheService cacheService, bool isCacheSave = true)
        {
            _isCacheSave = isCacheSave;
            _logCacheViewModel = logCacheViewModel;
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService), "LOGViewModel");
            _dataService = dataService;

            _logCacheDTO = logCacheDTO;

            TimeRemaining = 1200;


            if (isCacheSave)
            {
                // TODO: Do not store content on model - UI concern only. See if Core.Utilities Rtf converter can do this for UI directly
                // and use DTO to obtain UI PostIt content.
                //NotaryContent = PostItContent(createPostItViewModels);
                //_logCacheDTO.Content = NotaryContent;

                _cacheMaster = cacheService ?? throw new ArgumentNullException(nameof(cacheService), "LOGViewModel");
                _cacheMaster.AddLogCacheIdentifier(_logCacheDTO.Start.ToString("yyyyMMdd_HHmmss"));
            }
            else
            { 
                // TODO: Same as above
                //NotaryContent = _logCacheDTO.Content;
                TimeRemaining /= 2;

                _cacheMaster = cacheService ?? throw new ArgumentNullException(nameof(cacheService), "LOGViewModel");
            }

            StartCountdown();


            Setup(dataService, cacheService, logCacheDTO);

        }


        private void Setup(IDataService dataService, ICacheService cacheService, LogCacheDTO logCacheDTO)
        {
            DeleteCacheItemCommand = new DeleteCacheItemCommand(_logCacheViewModel, dataService, cacheService, logCacheDTO, true, _timer);
            QuickDeleteCacheItemCommand = new DeleteCacheItemCommand(_logCacheViewModel, dataService, cacheService, logCacheDTO, false, _timer);
        }



        #endregion










        #region Properties





        /// <summary>
        /// The project name of the log
        /// </summary>
        public string ProjectName => $"{_logCacheDTO.ProjectName} ({_logCacheDTO.ApplicationName})";

        /// <summary>
        /// ID for log cache item
        /// </summary>
        public string StartAsCacheID => _logCacheDTO.Start.ToString("yyyyMMdd_HHmmss");

        /// <summary>
        /// How many errors are in the log
        /// </summary>
        public int ErrorCount => _logCacheDTO.PostIts.Count(p => RtfConverter.HasContent(p.Error));
        /// <summary>
        /// How many solutions are in the log.
        /// </summary>
        public int SolutionCount => _logCacheDTO.PostIts.Count(p => RtfConverter.HasContent(p.Solution));
        /// <summary>
        /// How many suggestions are in the log.
        /// </summary>
        public int SuggestionCount => _logCacheDTO.PostIts.Count(p => RtfConverter.HasContent(p.Suggestion));
        /// <summary>
        /// How many comments are in the log.
        /// </summary>
        public int CommentCount => _logCacheDTO.PostIts.Count(p => RtfConverter.HasContent(p.Comment));

        /// <summary>
        /// Formatted start and end date.
        /// </summary>
        public string StartEndDate => $"{_logCacheDTO.Start.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_logCacheDTO.End.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

        /// <summary>
        /// Store the first occurence of a note with acceptable input only.
        /// </summary>
        public string NotaryContent
        {
            get
            {
                foreach (var postIt in _logCacheDTO.PostIts)
                {
                    var rtfFields = new[] { postIt.Error, postIt.Solution, postIt.Suggestion, postIt.Comment };

                    foreach (var rtf in rtfFields)
                    {
                        var (text, exception) = RtfConverter.ToPlainText(rtf);

                        if (exception != null)
                        {
                            _dataService.HandleExceptionAsync(exception);
                            Debug.WriteLine($"{Constants.ExceptionPrefix} LOGViewModel NotaryContent getter: {exception.Message}");
                            continue;
                        }

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            return text;
                        }
                    }
                }

                return "No Notes";
            }
        }




        private double timeRemaining;

        public double TimeRemaining
        {
            get
            {
                return timeRemaining;
            }
            set
            {
                timeRemaining = value;
                OnPropertyChanged(nameof(TimeRemaining));
            }
        }









        #endregion







        #region Member Functions








        private void StartCountdown()
        {
            _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private void TimerCallback(object? state)
        {
            try
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (!IsDisposed)
                        {
                            if (TimeRemaining == 0)
                            {
                                DeleteCacheItem();
                                if(_timer != null)
                                    _timer.Dispose();
                                IsDisposed = true;
                            }
                            else
                                TimeRemaining--;
                        }
                    });
                }
            }
            catch (TaskCanceledException tex)
            {
                if(_timer != null)
                    _timer.Dispose();

                _dataService.HandleExceptionAsync(tex);
            }
            catch (Exception ex)
            {
                _dataService.HandleExceptionAsync(ex);
                Debug.WriteLine($"Exception found near TimerCallback(): {ex.Message}");

            }
        }


        protected abstract void DeleteCacheItem();

        public void DeleteCacheFile(string id, LogContext logContext)
        {
            _cacheMaster.DeleteLogCache(_logCacheDTO.UserId, id, logContext);
        }







        #endregion









    }
}
