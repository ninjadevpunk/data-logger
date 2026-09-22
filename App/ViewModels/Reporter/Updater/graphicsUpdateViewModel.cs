using Data_Logger_1._3.Commands.ReporterCommands.UpdateCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Models.App_Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Collections.ObjectModel;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Updater
{
    public class graphicsUpdateViewModel : ReporterUpdaterViewModel
    {
        public override LOG.CATEGORY Category => LOG.CATEGORY.GRAPHICS;
        public override CacheContext Context { get => CacheContext.Graphics; }
        public override string LogType => "GRAPHICS LOG";


        private readonly GraphicsReportDeskViewModel _graphicsViewModel;
        private readonly ObservableCollection<string> _mediums;
        private readonly ObservableCollection<string> _formats;
        private readonly ObservableCollection<string> _units;
        private readonly ObservableCollection<string> _sizes;

        public IEnumerable<string> Mediums => _mediums;
        public IEnumerable<string> Formats => _formats;
        public IEnumerable<string> Units => _units;
        public IEnumerable<string> Sizes => _sizes;

        public graphicsUpdateViewModel(NavigationService navigationService, ReportDeskViewModel reportDeskViewModel, IDataService dataService, LOG log) : 
            base(navigationService, reportDeskViewModel, dataService, log)
        {
            AppFieldEnabled = true;
            ApplicationName = "Krita";

            _graphicsViewModel = (GraphicsReportDeskViewModel)reportDeskViewModel;
            InitializeFields();

            _mediums =
            [
                "Pencil", "Paint", "Pen", "Pencil Crayon", "Kokie", "Crayon", "Oil Pastel", "Chalk"
            ];

            _formats =
            [
                "Paper", "Digital Canvas", "Cardboard", "Wall/Street"
            ];

            _units =
            [
                "cm", "mm", "px", "in", "m"
            ];

            _sizes =
            [
                "A4 (29,7 cm x 21 cm)", "A5 (21 cm x 14,8 cm)", "Letter (27,9 cm x 21,6 cm)"
            ];

            LoadDefaultOutputs();
            LoadDefaultTypes();


            BrowseCommand = new EF_BrowseCommand(Context, this);
            //UpdateCommand = new UpdateCommand(Context, this, _graphicsViewModel, _navigationService, _dataService, reportViewModel, pdfService);
            ClearLoggerCommand = new EF_ResetLoggerCommand(this, Category);
        }







        public override async Task AutoStartAsync()
        {
            await LoadProjectsAndApplications();
            await UpdateLogCount();
        }

        private void InitializeFields()
        {
            Medium = "Pencil";
            Format = "Paper";
            MeasuringUnit = "cm";
            Size = "A4 (29,7 cm x 21 cm)";
            Height = string.Empty;
            Width = string.Empty;
            DPI = string.Empty;
            ColourDepth = string.Empty;
            IsCompleted = false;
            Source = string.Empty;


        }

        private async Task LoadProjectsAndApplications()
        {
            await _dataService.InitialiseProjectsLISTAsync(Category);
            var items = _dataService.SQLITE_PROJECTS;
            foreach (ProjectClass project in items)
            {
                _projects.Add(project.Name);
            }

            await _dataService.InitialiseApplicationsLISTAsync(Category);
            var apps = _dataService.SQLITE_APPLICATIONS;
            foreach (ApplicationClass app in apps)
            {
                _applications.Add(app.Name);
            }
        }

        private void LoadDefaultOutputs()
        {
            Output = "PNG";
            _outputs.Add("PNG");
            _outputs.Add("JPG");
            _outputs.Add("SVG");
            _outputs.Add("GIF");
            _outputs.Add("PDF");
            _outputs.Add("TIFF");
            _outputs.Add("PSD");
            _outputs.Add("WEBP");
        }

        private void LoadDefaultTypes()
        {
            Type = "NONE";
            _types.Add("NONE");
            _types.Add("Artwork");
            _types.Add("Doodle");
            _types.Add("Graphic Design Project");
            _types.Add("Resource");
            _types.Add("Portfolio");
        }






        #region Properties



        private string medium;
        public string Medium
        {
            get => medium;
            set
            {
                if (medium != value)
                {
                    medium = value;
                    OnPropertyChanged(nameof(Medium));
                }
            }
        }

        private string format;
        public string Format
        {
            get => format;
            set
            {
                if (format != value)
                {
                    format = value;
                    OnPropertyChanged(nameof(Format));
                }
            }
        }

        private string height;
        public string Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        private string width;
        public string Width
        {
            get => width;
            set
            {
                if (width != value)
                {
                    width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        private string brush;
        public string Brush
        {
            get
            {
                return brush;
            }
            set
            {
                brush = value;
                OnPropertyChanged(nameof(Brush));
            }
        }

        private string measuringUnit;
        public string MeasuringUnit
        {
            get => measuringUnit;
            set
            {
                if (measuringUnit != value)
                {
                    measuringUnit = value;
                    OnPropertyChanged(nameof(MeasuringUnit));
                }
            }
        }

        private string size;
        public string Size
        {
            get => size;
            set
            {
                if (size != value)
                {
                    size = value;
                    OnPropertyChanged(nameof(Size));
                }
            }
        }

        private string dpi;
        public string DPI
        {
            get => dpi;
            set
            {
                if (dpi != value)
                {
                    dpi = value;
                    OnPropertyChanged(nameof(DPI));
                }
            }
        }

        private string colourDepth;
        public string ColourDepth
        {
            get => colourDepth;
            set
            {
                if (colourDepth != value)
                {
                    colourDepth = value;
                    OnPropertyChanged(nameof(ColourDepth));
                }
            }
        }

        private bool isCompleted;
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        private string source;
        public string Source
        {
            get => source;
            set
            {
                if (source != value)
                {
                    source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }
        }



        #endregion






        public async Task UpdateLogCount()
        {
            if (_graphicsViewModel != null)
            {
                var count = await _dataService.LogCount(Category);
                LogCount = $"{_graphicsViewModel.Logs.Count} Logs";
            }
        }




    }
}
