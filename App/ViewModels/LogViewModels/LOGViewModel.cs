using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Dashboard;
using Data_Logger_1._3.ViewModels.Dialogs;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.LogViewModels
{
    public abstract class LOGViewModel : ViewModelBase
    {
        protected readonly LOG _LOG;
        protected readonly LogCacheViewModel _vm;
        protected readonly CacheMaster _cacheMaster;
        public abstract CacheContext LOGViewModelContext { get; }



        public bool IsDisposed { get; set; } = false;

        public Timer _timer;

        public ICommand EditCommand { get; set; }

        public ICommand ViewCommand { get; set; }

        public ICommand DeleteCacheItemCommand { get; set; }

        public ICommand QuickDeleteCacheItemCommand { get; set; }





        #region Constructors



        public LOGViewModel(LOG log, LogCacheViewModel logCacheViewModel, ObservableCollection<PostItViewModel> createPostItViewModels, IDataService dataService)
        {
            _vm = logCacheViewModel;

            _LOG = log;
            NotaryContent = PostItContent(createPostItViewModels);
            _LOG.Content = NotaryContent;
            TimeRemaining = 1200;
            StartCountdown();

            
            Setup(dataService);

            _cacheMaster = dataService.GetCachemaster();
            _cacheMaster.IdentifiersChecked(_LOG.Start.ToString("yyyyMMdd_HHmmss"));

        }

        public LOGViewModel(LOG log, LogCacheViewModel logCacheViewModel, IDataService dataService)
        {
            _vm = logCacheViewModel;

            _LOG = log;
            NotaryContent = _LOG.Content;
            TimeRemaining = 600;
            StartCountdown();


            Setup(dataService);

            _cacheMaster = dataService.GetCachemaster();
        }

        private void Setup(IDataService dataService)
        {
            DeleteCacheItemCommand = new DeleteCacheItemCommand(_vm, dataService, true);
            QuickDeleteCacheItemCommand = new DeleteCacheItemCommand(_vm, dataService, false);
        }



        #endregion










        #region Properties





        public int ViewModelID => _LOG.ID;

        public string ProjectName => $"{_LOG.Project.Name} ({_LOG.Application.Name})";

        public string StartAsID => _LOG.Start.ToString("yyyyMMdd_HHmmss");

        public string ErrorCount => _LOG.ErrorCount().ToString();

        public string SolutionCount => _LOG.SolutionCount().ToString();

        public string SuggestionCount => _LOG.SuggestionCount().ToString();

        public string CommentCount => _LOG.CommentCount().ToString();


        public string StartEndDate => $"{_LOG.Start.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")} - " +
            $"{_LOG.End.ToString("dddd, d MMMM yyyy HH:mm:ss.fff")}";

        /** Store the first occurence of a note with acceptable input only. **/
        public string NotaryContent { get; set; }


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
                                _timer.Dispose();
                                IsDisposed = true;
                            }
                            else
                                TimeRemaining--;
                        }
                    });
                }
            }
            catch(TaskCanceledException tex)
            {
                _timer.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception found near TimerCallback(): {ex.Message}");

                // TODO


            }
        }

        protected abstract void DeleteCacheItem();

        public void DeleteCacheFile(string id, CacheContext cacheContext)
        {
            _cacheMaster.DeleteViewModel(id, cacheContext);
        }






        public static string PostItContent(ObservableCollection<PostItViewModel> createPostItViewModel)
        {
            string pattern = @"\S";
            Regex regex = new Regex(pattern);

            foreach (PostItViewModel postItViewModel in createPostItViewModel)
            {
                if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Error) && regex.IsMatch(postItViewModel.Display_Error))
                    return postItViewModel.Display_Error;
                else if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Solution) && regex.IsMatch(postItViewModel.Display_Solution))
                    return postItViewModel.Display_Solution;
                else if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Suggestion) && regex.IsMatch(postItViewModel.Display_Suggestion))
                    return postItViewModel.Display_Suggestion;
                else if (!string.IsNullOrWhiteSpace(postItViewModel.Display_Comment) && regex.IsMatch(postItViewModel.Display_Comment))
                    return postItViewModel.Display_Comment;
            }

            return "No Notes";
        }






        #endregion









    }
}
