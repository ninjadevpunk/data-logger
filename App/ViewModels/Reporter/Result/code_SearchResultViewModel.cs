using Data_Logger_1._3.Commands.LogCacheCommands;
using Data_Logger_1._3.Commands.ReporterCommands;
using Data_Logger_1._3.Models;
using Data_Logger_1._3.Services;
using Data_Logger_1._3.ViewModels.Reporter.Desk;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.ViewModels.Reporter.Result
{
    public class code_SearchResultViewModel : SearchResultViewModel
    {
        public override CacheContext SearchResultContext => CacheContext.Coding;

        public code_SearchResultViewModel(NavigationService navigationService, IDataService dataService, CodingLOG codingLOG, 
            CodeReportDeskViewModel codeReportDeskViewModel)
    : base(codingLOG, codeReportDeskViewModel, navigationService)
        {
            ID = codingLOG.ID;

            // Default
            IconStyle = TryParsePath("path_DefaultApp");

            switch (codingLOG.Application.Name)
            {
                case "Visual Studio Community 2022":
                    IconStyle = TryParsePath("path_VS_SearchResult");
                        IconPath = null;
                    break;
                case "Visual Studio Code":
                    IconStyle = TryParsePath("path_VSC");
                        IconPath = null;
                    break;
                case "IntelliJ":
                    {
                        var iconGridIntelliJ = new Grid
                        {
                            Width = 30,
                            Height = 30
                        };

                        // Top-left polygon (flat orange #F97A12)
                        iconGridIntelliJ.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M7.59375 23.3906L0.328125 17.6719L3.9375 10.9688L14.2969 15L7.59375 23.3906Z"),
                            Fill = new SolidColorBrush(Color.FromRgb(0xF9, 0x7A, 0x12))
                        });

                        // Big right-side polygon (flat orange #F97A12)
                        iconGridIntelliJ.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M30 8.01563L29.4375 25.3594L17.9063 30L10.9688 25.5L21.0938 15L16.6875 5.25L20.6719 0.46875L30 8.01563Z"),
                            Fill = new SolidColorBrush(Color.FromRgb(0xF9, 0x7A, 0x12))
                        });

                        // Small inner/top-right wedge (flat blue #087CFA)
                        iconGridIntelliJ.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M30 8.01563L20.8594 18.7969L16.6875 5.25L20.6719 0.46875L30 8.01563Z"),
                            Fill = new SolidColorBrush(Color.FromRgb(0x08, 0x7C, 0xFA))
                        });

                        // Central polygon / bottom-left-to-center (flat pink #FE315D)
                        iconGridIntelliJ.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M14.4375 24.8906L2.4375 29.25L4.35938 22.5L6.84375 14.1562L0 11.8594L4.35938 0L13.7344 1.125L23.0156 11.7188L14.4375 24.8906Z"),
                            Fill = new SolidColorBrush(Color.FromRgb(0xFE, 0x31, 0x5D))
                        });

                        // Black square
                        iconGridIntelliJ.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M24.375 5.625H5.625V24.375H24.375V5.625Z"),
                            Fill = Brushes.Black
                        });

                        // White details / lettering
                        iconGridIntelliJ.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M7.40625 20.8594H14.4375V22.0312H7.40625V20.8594ZM12.4219 9.46875V8.0625H8.57812V9.46875H9.65625V14.3906H8.57812V15.8437H12.4219V14.3906H11.3438V9.46875H12.4219ZM16.1719 15.9375C15.6367 15.9616 15.1043 15.8487 14.625 15.6094C14.2329 15.393 13.8834 15.1071 13.5937 14.7656L14.6719 13.5469C14.8666 13.7608 15.087 13.9497 15.3281 14.1094C15.555 14.2424 15.8153 14.3075 16.0781 14.2969C16.3671 14.319 16.6482 14.196 16.8281 13.9687C17.0452 13.6891 17.146 13.3365 17.1094 12.9844V7.96875H18.8438V13.0312C18.8615 13.4459 18.7978 13.8599 18.6562 14.25C18.5326 14.6457 18.3061 15.0016 18 15.2812C17.4688 15.6757 16.8327 15.904 16.1719 15.9375Z"),
                            Fill = Brushes.White
                        });

                        IconContainer = iconGridIntelliJ;
                        IconPath = null;
                        IconStyle = null;
                        break;
                    }
                case "PyCharm":
                    {
                        var iconGridPyCharm = new Grid
                        {
                            Width = 30,
                            Height = 30
                        };

                        // Gradient Path 0 (top-left triangle)
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M21.0469 4.73438L29.8125 12L26.625 18.4219L21.375 16.9688H16.7812L21.0469 4.73438Z"),
                            Fill = new LinearGradientBrush(
                                new GradientStopCollection
                                {
            new GradientStop(Color.FromRgb(0x21, 0xD7, 0x89), 0),
            new GradientStop(Color.FromRgb(0x07, 0xC3, 0xF2), 1)
                                },
                                new Point(0, 0),
                                new Point(1, 1)
                            )
                        });

                        // Gradient Path 1
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M12.1875 9.46875L10.5 18.4219L10.3594 21.5156L6.04687 23.3437L0 24L1.82813 4.59375L12.8437 0L19.5937 4.45312L12.1875 9.46875Z"),
                            Fill = new LinearGradientBrush(
                                new GradientStopCollection
                                {
            new GradientStop(Color.FromRgb(0xFC, 0xF8, 0x4A), 0.01),
            new GradientStop(Color.FromRgb(0xA7, 0xEB, 0x62), 0.11),
            new GradientStop(Color.FromRgb(0x5F, 0xE0, 0x77), 0.21),
            new GradientStop(Color.FromRgb(0x32, 0xDA, 0x84), 0.27),
            new GradientStop(Color.FromRgb(0x21, 0xD7, 0x89), 0.31),
            new GradientStop(Color.FromRgb(0x21, 0xD7, 0x89), 0.58),
            new GradientStop(Color.FromRgb(0x21, 0xD7, 0x89), 0.6),
            new GradientStop(Color.FromRgb(0x20, 0xD6, 0x8C), 0.69),
            new GradientStop(Color.FromRgb(0x1E, 0xD4, 0x97), 0.76),
            new GradientStop(Color.FromRgb(0x19, 0xD1, 0xA9), 0.83),
            new GradientStop(Color.FromRgb(0x13, 0xCC, 0xC2), 0.9),
            new GradientStop(Color.FromRgb(0x0B, 0xC6, 0xE1), 0.97),
            new GradientStop(Color.FromRgb(0x07, 0xC3, 0xF2), 1)
                                },
                                new Point(0, 0),
                                new Point(1, 1)
                            )
                        });

                        // Gradient Path 2
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M12.1875 9.46875L13.0312 26.7656L10.2656 30L0 24L8.4375 11.3906L12.1875 9.46875Z"),
                            Fill = new LinearGradientBrush(
                                new GradientStopCollection
                                {
            new GradientStop(Color.FromRgb(0x21, 0xD7, 0x89), 0),
            new GradientStop(Color.FromRgb(0x24, 0xD7, 0x88), 0.16),
            new GradientStop(Color.FromRgb(0x2F, 0xD8, 0x86), 0.3),
            new GradientStop(Color.FromRgb(0x41, 0xDA, 0x82), 0.44),
            new GradientStop(Color.FromRgb(0x5A, 0xDC, 0x7D), 0.56),
            new GradientStop(Color.FromRgb(0x7A, 0xE0, 0x77), 0.69),
            new GradientStop(Color.FromRgb(0xA1, 0xE3, 0x6E), 0.81),
            new GradientStop(Color.FromRgb(0xCF, 0xE8, 0x65), 0.93),
            new GradientStop(Color.FromRgb(0xF1, 0xEB, 0x5E), 1)
                                },
                                new Point(0, 0),
                                new Point(1, 1)
                            )
                        });

                        // Gradient Path 3 (top-right trapezoid)
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M23.5312 8.20312H13.125L22.3125 0L23.5312 8.20312Z"),
                            Fill = new LinearGradientBrush(
                                new GradientStopCollection
                                {
            new GradientStop(Color.FromRgb(0x21, 0xD7, 0x89), 0),
            new GradientStop(Color.FromRgb(0x24, 0xD7, 0x88), 0.06),
            new GradientStop(Color.FromRgb(0x2F, 0xD8, 0x86), 0.11),
            new GradientStop(Color.FromRgb(0x41, 0xDA, 0x82), 0.16),
            new GradientStop(Color.FromRgb(0x5A, 0xDD, 0x7D), 0.21),
            new GradientStop(Color.FromRgb(0x79, 0xE0, 0x77), 0.25),
            new GradientStop(Color.FromRgb(0x7C, 0xE0, 0x76), 0.26),
            new GradientStop(Color.FromRgb(0x8C, 0xE1, 0x73), 0.5),
            new GradientStop(Color.FromRgb(0xB2, 0xE5, 0x6B), 0.92)
                                },
                                new Point(0, 0),
                                new Point(1, 1)
                            )
                        });

                        // Gradient Path 4 (bottom-right polygon)
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M30 26.8125L20.8594 29.9531L8.67188 26.5312L12.1875 9.46875L13.5938 8.20312L21.0469 7.5L20.3438 14.9531L26.25 12.6562L30 26.8125Z"),
                            Fill = new LinearGradientBrush(
                                new GradientStopCollection
                                {
            new GradientStop(Color.FromRgb(0xFC, 0xF8, 0x4A), 0.39),
            new GradientStop(Color.FromRgb(0xEC, 0xF4, 0x51), 0.54),
            new GradientStop(Color.FromRgb(0xC2, 0xE9, 0x64), 0.83),
            new GradientStop(Color.FromRgb(0xB2, 0xE5, 0x6B), 0.92)
                                },
                                new Point(0, 0),
                                new Point(1, 1)
                            )
                        });

                        // Black square
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M24.375 5.625H5.625V24.375H24.375V5.625Z"),
                            Fill = Brushes.Black
                        });

                        // White details / letters
                        iconGridPyCharm.Children.Add(new Path
                        {
                            Data = Geometry.Parse("M7.40625 20.8594H14.4375V22.0312H7.40625V20.8594ZM7.35938 8.0625H10.5469C12.4219 8.0625 13.5 9.14062 13.5 10.7344C13.5 12.5156 12.0938 13.4531 10.3594 13.4531H9.04687V15.7969H7.35938V8.0625ZM11.7656 10.8281C11.7656 10.0781 11.25 9.65625 10.3594 9.65625H9.04687V12.0469H10.4063C11.3273 12.0469 11.7656 11.4844 11.7656 10.8281ZM14.1563 12C14.1323 10.9237 14.5493 9.88436 15.3106 9.1231C16.0719 8.36184 17.1112 7.94482 18.1875 7.96875C19.6406 7.96875 20.5312 8.4375 21.2344 9.1875L20.1562 10.4531C19.5469 9.89062 18.9375 9.5625 18.1875 9.5625C16.875 9.5625 15.9375 10.6406 15.9375 12C15.9375 13.3125 16.875 14.4375 18.1875 14.4375C19.0781 14.4375 19.6406 14.0625 20.25 13.5469L21.3281 14.625C20.5312 15.4688 19.6406 16.0313 18.1406 16.0313C17.0757 16.0313 16.0551 15.6051 15.3065 14.8477C14.558 14.0903 14.1437 13.0648 14.1563 12Z"),
                            Fill = Brushes.White
                        });

                        IconContainer = iconGridPyCharm;
                        IconPath = null;
                        IconStyle = null;

                        break;
                    }
                case "WebStorm":
                    IconPath = new Path
                    {
                        Width = 20,
                        Height = 20.48,
                        Data = Geometry.Parse("M4.03125 27.0938L0 3.14062L7.5 0.046875L12.2344 2.85938L16.6406 0.515625L25.7344 4.03125L20.625 30L4.03125 27.0938ZM30 10.1719L26.1094 0.609375L19.125 0L8.25 10.4063L11.1562 23.8594L16.6406 27.6562L30 19.7344L26.7187 13.5938L30 10.1719ZM24 8.71875L26.7187 13.5938L30 10.1719L27.6094 4.21875L24 8.71875ZM24.375 5.625H5.625V24.375H24.375V5.625ZM7.40625 20.8594H14.4375V22.0313H7.40625V20.8594ZM16.5938 14.7188L17.5781 13.5C18.2813 14.0625 18.9844 14.4375 19.875 14.4375C20.5781 14.4375 21 14.1563 21 13.6875C21 13.2656 20.7188 13.0313 19.4531 12.7031C17.9063 12.2813 16.9219 11.8594 16.9219 10.3125V10.2656C16.9219 8.85939 18.0469 7.92189 19.5937 7.92189C20.6307 7.91752 21.6386 8.2645 22.4531 8.90626L21.5625 10.2188C20.9966 9.77651 20.3105 9.51514 19.5937 9.46876C18.9844 9.46876 18.6094 9.75001 18.6094 10.1719C18.6094 10.6875 18.9375 10.875 20.25 11.2031C21.7969 11.625 22.6875 12.1875 22.6875 13.5469C22.6875 15.0938 21.5156 15.9844 19.875 15.9844C18.6707 15.9401 17.5158 15.4946 16.5938 14.7188ZM15.0938 8.06251L13.9219 12.5625L12.6094 8.06251H11.2969L9.98437 12.5625L8.8125 8.06251H6.98438L9.23438 15.8438H10.6875L11.9531 11.3438L13.2188 15.8438H14.6719L16.9219 8.06251H15.0938Z"),
                        Fill = new LinearGradientBrush
                        {
                            StartPoint = new Point(0, 0),
                            EndPoint = new Point(1, 1),
                            GradientStops = new GradientStopCollection
                            {
                                new GradientStop(Color.FromRgb(0x00,0xCD,0xD7), 0.28),
                                new GradientStop(Color.FromRgb(0x20,0x86,0xD7), 0.94)
                            }
                        }
                    };
                    IconStyle = null;

                    break;

            }

            ViewLogCommand = new ViewCommand(_navigationService, SearchResultContext, ViewType.Log, codingLOG);
            EditLogCommand = new EditLogCommand(codingLOG, codeReportDeskViewModel, _navigationService);
            DeleteLogCommand = new DeleteLogCommand(dataService, codeReportDeskViewModel, SearchResultContext, codingLOG);
        }

        public int ID { get; set; }

    }
}
