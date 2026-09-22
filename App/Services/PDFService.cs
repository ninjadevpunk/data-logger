using Data_Logger_1._3.Models;
using Data_Logger_1._3.ViewModels.Dialogs;
using SkiaSharp;
using Svg.Skia;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows;
using static Data_Logger_1._3.Services.CacheMaster;

namespace Data_Logger_1._3.Services
{
    public class PDFService
    {

        /// <summary>
        /// Determines which side the PostIt list should go on.
        /// </summary>
        public enum Side { Left, Right }

        private string filePath = "";

        /// <summary>
        /// Formats a date with the correct suffix
        /// </summary>
        /// <param name="date">The unformatted date</param>
        /// <returns>Returns the date in its correct format</returns>
        public static string FormatDateWithSuffix(DateTime date)
        {
            string daySuffix = GetDaySuffix(date.Day);
            return $"{date:dddd} {date.Day}{daySuffix} {date:MMMM yyyy HH:mm}";
        }

        public static string GetDaySuffix(int day)
        {

            // Special case for 11th, 12th, 13th
            if (day >= 11 && day <= 13) 
            {
                return "th";
            }

            switch (day % 10)
            {
                case 1:
                    return "st";
                case 2:
                    return "nd";
                case 3:
                    return "rd";
                default:
                    return "th";
            }
        }

        private async Task ExportLogToPDF(LOG log, CacheContext context)
        {
            try
            {
                string defaultFileName = $"{log.Start.ToString("dMMMMyyyy.HHmmssfff")}.{log.Project.Name.Replace(" ", "")}.pdf";

                var dialog = new Microsoft.Win32.SaveFileDialog();
                dialog.DefaultExt = ".pdf";
                dialog.Filter = "PDF File (.pdf)|*.pdf";
                dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dialog.FileName = defaultFileName;

                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    filePath = dialog.FileName;
                }
                else
                    return;

                string syncfusionKey = Environment.GetEnvironmentVariable("SYNCFUSION_KEY") ?? throw new InvalidOperationException("SYNCFUSION_KEY is not set.");

                // Register Syncfusion license
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionKey);

                // PDF Document Setup
                PdfDocument document = new PdfDocument();
                document.PageSettings.Orientation = PdfPageOrientation.Landscape;
                document.PageSettings.Size = PdfPageSize.A4;
                PdfMargins margins = new PdfMargins
                {
                    All = 15
                };
                document.PageSettings.Margins = margins;

                // Fonts
                string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;

                // Construct correct font paths
                string baseFontPath = Path.Combine(projectRoot, "Fonts");
                string logoFontPath = Path.Combine(baseFontPath, "Zekton.ttf");
                string regularFontPath = Path.Combine(baseFontPath, "Red Hat Display.ttf");

                string pdfFontPath = Path.Combine(baseFontPath, "PDF");
                string mediumFontPath = Path.Combine(pdfFontPath, "Red Hat Display Medium.ttf");
                string semiBoldFontPath = Path.Combine(pdfFontPath, "Red Hat Display SemiBold.ttf");
                string boldFontPath = Path.Combine(pdfFontPath, "Red Hat Display Bold.ttf");


                // Resources
                string baseResourcePath = Path.Combine(projectRoot, "Resources");
                string baseSvgPath = Path.Combine(baseResourcePath, "SVGs");
                string AndroidSVG = Path.Combine(baseSvgPath, "AndroidSVG.svg");
                string LogoSVG = Path.Combine(baseSvgPath, "LogoSVG.svg");
                string CallSVG = Path.Combine(baseSvgPath, "CallSVG.svg");
                string EmailSVG = Path.Combine(baseSvgPath, "EmailSVG.svg");
                string ScanTextSVG = Path.Combine(baseSvgPath, "ScanTextSVG.svg");

                PdfFont logoFont = new PdfTrueTypeFont(logoFontPath, 8);
                PdfFont defaultFont = new PdfTrueTypeFont(regularFontPath, 10);
                PdfFont mediumFont = new PdfTrueTypeFont(mediumFontPath, 14);
                PdfFont boldFont = new PdfTrueTypeFont(boldFontPath, 16);
                PdfFont semiBoldFont = new PdfTrueTypeFont(semiBoldFontPath, 12);
                PdfFont smallFont = new PdfTrueTypeFont(regularFontPath, 6);

                // Page & Layout Constants
                const float qrCodeSize = 100;
                const float columnSpacing = 30;
                const float midWaySpacing = 10;
                const float rectangleWidth = 8;
                const float postItSidePadding = 5;
                const float maxPostItHeight = 100;
                const float footerHeight = 70;
                const float logoSVGWidth = 25;
                const float logoSVGHeight = 18.5215f;
                const float CallSVGSize = 10;
                const float EmailSVGSize = CallSVGSize;
                const float profilePicSize = 50;

                float contentHeight = document.PageSettings.Size.Height - footerHeight - margins.Top * 2;
                float column1Width = qrCodeSize;
                float column2Width = 256;
                float column3Width = document.PageSettings.Size.Width - (column1Width + column2Width + margins.Left * 2 + columnSpacing + midWaySpacing);

                int pageNumber = 1;

                PdfPage currentPage;
                PdfGraphics graphics;

                void DrawSvgInPdf(PdfGraphics gfx, string svgContent, float x, float y, float width, float height)
                {
                    using (var svg = new SKSvg())
                    {

                        svg.Load(svgContent);

                        // Increase the scale factor for better quality rendering (2x, 3x, etc.)
                        float scaleFactor = 3.0f;

                        // Create a surface and SkiaSharp canvas at a higher resolution
                        SKImageInfo imageInfo = new SKImageInfo((int)(width * scaleFactor), (int)(height * scaleFactor));
                        using (SKSurface surface = SKSurface.Create(imageInfo))
                        {
                            SKCanvas canvas = surface.Canvas;
                            canvas.Clear(SKColors.Transparent);
                            canvas.Scale(scaleFactor);
                            canvas.DrawPicture(svg.Picture);


                            using (SKImage skImage = surface.Snapshot())
                            // 100 = Highest quality
                            using (SKData data = skImage.Encode(SKEncodedImageFormat.Png, 100))
                            using (MemoryStream imageStream = new MemoryStream(data.ToArray()))
                            {
                                // Load the PNG image into Syncfusion PDF
                                PdfImage image = PdfImage.FromStream(imageStream);
                                gfx.DrawImage(image, new PointF(x, y), new SizeF(width, height)); // Draw image at the given position and size
                            }
                        }

                    }
                }

                var footerYcoordinate = document.PageSettings.Size.Height - footerHeight + 20;

                // Calculate the vertical position for centered alignment
                float logoCenterY = footerYcoordinate + (logoSVGHeight / 2);

                void CreateNewPage()
                {
                    currentPage = document.Pages.Add();
                    graphics = currentPage.Graphics;

                    /*      **FOOTER**       */


                    // Logo

                    DrawSvgInPdf(graphics, LogoSVG, margins.Left, footerYcoordinate, logoSVGWidth, logoSVGHeight);

                    // Logo Text

                    // Measure the height of the text
                    const string logoText = "generated by DATA LOGGER";

                    float textHeight = logoFont.MeasureString(logoText).Height;

                    var logoTextXcoordinate = margins.Left + logoSVGWidth + 20;
                    float logoTextYcoordinate = logoCenterY - (textHeight / 2);  // Center the text with the logo

                    // Draw the text at the calculated position
                    graphics.DrawString(logoText, logoFont, PdfBrushes.Black, new PointF(logoTextXcoordinate, logoTextYcoordinate));



                    // Call Details


                    var callAndEmailFont = new PdfTrueTypeFont(regularFontPath, 8);
                    var callSvgXcoordinate = logoTextXcoordinate + logoFont.MeasureString(logoText).Width + 20;
                    var callSvgYcoordinate = logoCenterY - (CallSVGSize / 2);

                    DrawSvgInPdf(graphics, CallSVG, callSvgXcoordinate, callSvgYcoordinate, CallSVGSize, CallSVGSize);

                    // Calculate Y for call text (vertically aligned with the logo)
                    const string companyNumber = "078 771 2776";


                    var callTextHeight = callAndEmailFont.MeasureString(companyNumber).Height;

                    var callTextYcoordinate = logoCenterY - (callTextHeight / 2);
                    var callTextXcoordinate = callSvgXcoordinate + CallSVGSize + 10;  // 10 units gap between icon and text

                    graphics.DrawString(companyNumber, callAndEmailFont, PdfBrushes.Black, new PointF(callTextXcoordinate, callTextYcoordinate));

                    // Email Details
                    var emailSvgXcoordinate = callTextXcoordinate + callAndEmailFont.MeasureString(companyNumber).Width + 20;
                    var emailSvgYcoordinate = logoCenterY - (EmailSVGSize / 2);

                    DrawSvgInPdf(graphics, EmailSVG, emailSvgXcoordinate, emailSvgYcoordinate, EmailSVGSize, EmailSVGSize);

                    string emailAddress = log.Author.Email;

                    var emailTextHeight = callAndEmailFont.MeasureString(emailAddress).Height;

                    var emailTextYcoordinate = logoCenterY - (emailTextHeight / 2);
                    var emailTextXcoordinate = emailSvgXcoordinate + EmailSVGSize + 10;

                    graphics.DrawString(emailAddress, callAndEmailFont, PdfBrushes.Black, new PointF(emailTextXcoordinate, emailTextYcoordinate));



                }

                CreateNewPage();




                /*      **COLUMN 1**        */

                // **QR Code**
                PdfQRBarcode qrBarcode = new PdfQRBarcode
                {
                    Text = log.ID.ToString(),
                    Size = new SizeF(qrCodeSize, qrCodeSize)
                };
                qrBarcode.Draw(currentPage, new PointF(margins.Left, margins.Top));


                PdfPen thinPen = new PdfPen(PdfBrushes.Black, 0.5f);

                /// Define the width and height for the pill shape
                float pillWidth = qrCodeSize;
                float pillHeight = 20;
                float pillRadius = pillHeight / 2;

                // Define start position for the pill shape (10 pixels below QR code)
                float pillX = margins.Left;
                float pillYPosition = margins.Top + qrCodeSize + 10;

                // Define control points for the Bezier curves
                PointF leftStart = new PointF(pillX + pillRadius, pillYPosition);
                PointF rightEnd = new PointF(pillX + pillWidth - pillRadius, pillYPosition);
                PointF leftBottom = new PointF(pillX + pillRadius, pillYPosition + pillHeight);
                PointF rightBottom = new PointF(pillX + pillWidth - pillRadius, pillYPosition + pillHeight);

                // Top-left curve
                graphics.DrawBezier(thinPen,
                    new PointF(pillX, pillYPosition + pillRadius),
                    new PointF(pillX, pillYPosition),
                    new PointF(pillX + pillRadius, pillYPosition),
                    leftStart
                );

                // Top straight line
                graphics.DrawLine(thinPen, leftStart, rightEnd);

                // Top-right curve
                graphics.DrawBezier(thinPen,
                    rightEnd,
                    new PointF(pillX + pillWidth, pillYPosition),
                    new PointF(pillX + pillWidth, pillYPosition + pillRadius),
                    new PointF(pillX + pillWidth, pillYPosition + pillRadius)
                );

                // Bottom-right curve
                graphics.DrawBezier(thinPen,
                    new PointF(pillX + pillWidth, pillYPosition + pillRadius),
                    new PointF(pillX + pillWidth, pillYPosition + pillHeight),
                    new PointF(rightBottom.X, rightBottom.Y),
                    rightBottom
                );

                // Bottom straight line
                graphics.DrawLine(thinPen, rightBottom, leftBottom);

                // Bottom-left curve
                graphics.DrawBezier(thinPen,
                    leftBottom,
                    new PointF(pillX, pillYPosition + pillHeight),
                    new PointF(pillX, pillYPosition + pillRadius),
                    new PointF(pillX, pillYPosition + pillRadius)
                );




                // **SVG + Text as a Unit (Android SVG and text)**
                const string scanText = "scan with Android phone";

                // Define the SVG size
                float svgWidth = 10;  // Fixed size for Android SVG
                float svgHeight = 10; // Fixed size for Android SVG

                // Define the gap between the SVG and the text
                const float scanTextGap = 5;

                // Measure the size of the text
                var scanTextFont = new PdfTrueTypeFont(regularFontPath, 6);
                var textSize = scanTextFont.MeasureString(scanText);

                // Calculate the total width of the unit (SVG + gap + text)
                float totalWidth = svgWidth + scanTextGap + textSize.Width;

                // Define the center of the pill (for centering the whole unit)
                float pillCenterX = margins.Left + pillWidth / 2;
                float pillCenterY = pillYPosition + pillHeight / 2;

                // Position the unit (SVG + Text) so that it is centered within the pill
                float unitX = pillCenterX - totalWidth / 2;

                // **SVG Drawing (Android SVG)**
                float svgX = unitX;
                float svgY = pillCenterY - svgHeight / 2;

                // Draw the Android SVG inside the unit
                DrawSvgInPdf(currentPage.Graphics, AndroidSVG, svgX, svgY, svgWidth, svgHeight);

                // **Text Drawing (next to the SVG)**
                float textX = unitX + svgWidth + scanTextGap;
                float textY = pillCenterY - textSize.Height / 2;

                // Draw the text next to the SVG
                graphics.DrawString(scanText, scanTextFont, PdfBrushes.Black, new PointF(textX, textY));







                /*      **COLUMN 2**        */

                float projectDetailsX = margins.Left + qrCodeSize + columnSpacing;
                float Ycoordinate = margins.Top;

                // **START & END DATE**
                string startAndEndDate = $"{FormatDateWithSuffix(log.Start)} - {FormatDateWithSuffix(log.End)}";
                graphics.DrawString(startAndEndDate, smallFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                Ycoordinate += smallFont.MeasureString(startAndEndDate).Height + 5;

                // **PROJECT**
                graphics.DrawString(log.Project.Name, boldFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                Ycoordinate += boldFont.MeasureString(log.Project.Name).Height;

                // **APPLICATION**
                graphics.DrawString(log.Application.Name, semiBoldFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                Ycoordinate += semiBoldFont.MeasureString(log.Application.Name).Height + 60;

                // **AUTHOR**

                // Define Profile Picture Position
                var profilePicX = projectDetailsX;
                var profilePicY = Ycoordinate;
                var profilePicRectangle = new RectangleF(profilePicX, profilePicY, profilePicSize, profilePicSize);

                // Load or Upload Profile Picture
                string profilePicPath = log.Author.ProfilePic;
                string uploadedImageUrl;
                string fileId = null; // Store ImageKit fileId

                if (string.IsNullOrWhiteSpace(profilePicPath) || !File.Exists(profilePicPath))
                {
                    // No local file → Use a default avatar
                    uploadedImageUrl = "https://ik.imagekit.io/ninjadevpunk/default_avatar.png";
                }
                else
                {
                    // Upload to ImageKit & get fileId
                    (uploadedImageUrl, fileId) = await ImageKitService.UploadImageAsync(profilePicPath);
                }

                // Process Image URL for 50x50 profile picture
                string processedProfilePicUrl = ImageKitService.GetProcessedImageUrl(uploadedImageUrl);

                // Download Processed Image
                string tempProfilePicPath = Path.Combine(Path.GetTempPath(), "profile_pic.png");
                await ImageKitService.DownloadImageAsync(processedProfilePicUrl, tempProfilePicPath);

                // Draw Profile Picture in PDF
                using (FileStream imageStream = new FileStream(tempProfilePicPath, FileMode.Open, FileAccess.Read))
                {
                    PdfImage profileImage = PdfImage.FromStream(imageStream);
                    graphics.DrawImage(profileImage, profilePicRectangle);
                }

                // Delete temporary file
                if (File.Exists(tempProfilePicPath))
                {
                    File.Delete(tempProfilePicPath);
                }

                // Delete image from ImageKit (only if it was uploaded and not a default avatar)
                if (!string.IsNullOrEmpty(fileId))
                {
                    await ImageKitService.DeleteImageAsync(fileId);
                }





                // Author Details


                var authorXcoordinate = projectDetailsX + profilePicSize + 15;
                var authorPretextFont = new PdfTrueTypeFont(regularFontPath, 8);
                graphics.DrawString("created by", authorPretextFont, PdfBrushes.Black, new PointF(authorXcoordinate, Ycoordinate + 10));
                Ycoordinate += boldFont.MeasureString("created by").Height;



                string ToSentenceCase(string input)
                {
                    if (string.IsNullOrWhiteSpace(input)) return input;
                    input = input.Trim();
                    return char.ToUpper(input[0]) + input.Substring(1).ToLower();
                }


                // Name and Surname
                var fullName = $"{ToSentenceCase(log.Author.FirstName)} {ToSentenceCase(log.Author.LastName)}";
                graphics.DrawString(fullName, mediumFont, PdfBrushes.Black, new PointF(authorXcoordinate, Ycoordinate));
                Ycoordinate += mediumFont.MeasureString(fullName).Height + 30;



                // **OUTPUT**
                var output = $"{log.Output.Name}";
                graphics.DrawString(output, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                Ycoordinate += defaultFont.MeasureString(output).Height + 5;



                // **TYPE**
                var type = $"{log.Type.Name}";
                graphics.DrawString(type, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                Ycoordinate += defaultFont.MeasureString(type).Height + 5;


                switch (context)
                {
                    case CacheContext.AndroidStudio:
                        {

                            if (log is CodingLOG clog)
                            {


                                // **BUGS**

                                var bugs = $"{clog.Bugs}";
                                graphics.DrawString(bugs, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(bugs).Height + 5;


                                // **APP OPENED**

                                var opened = clog.Success ? "Application Launched Successfully" : "No Sucessful Launch";
                                graphics.DrawString(opened, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(opened).Height + 5;

                                if (clog is AndroidCodingLOG asLog)
                                {



                                    // **SYNC**

                                    var sync = $"{asLog.Sync.ToString("HH:mm:ss:fff")}";
                                    graphics.DrawString(sync, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                    Ycoordinate += defaultFont.MeasureString(sync).Height + 5;


                                    // **STARTING GRADLE DAEMON

                                    var gradleDaemon = $"{asLog.StartingGradleDaemon.ToString("HH:mm:ss:fff")}";
                                    graphics.DrawString(gradleDaemon, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                    Ycoordinate += defaultFont.MeasureString(gradleDaemon).Height + 5;


                                    // **RUN BUILD**

                                    var runBuild = $"{asLog.RunBuild.ToString("HH:mm:ss:fff")}";
                                    graphics.DrawString(runBuild, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                    Ycoordinate += defaultFont.MeasureString(runBuild).Height + 5;

                                    // **LOAD BUILD**

                                    var loadBuild = $"{asLog.LoadBuild.ToString("HH:mm:ss:fff")}";
                                    graphics.DrawString(loadBuild, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                    Ycoordinate += defaultFont.MeasureString(loadBuild).Height + 5;

                                    // **CONFIGURE BUILD**

                                    var configureBuild = $"{asLog.ConfigureBuild.ToString("HH:mm:ss:fff")}";
                                    graphics.DrawString(configureBuild, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                    Ycoordinate += defaultFont.MeasureString(configureBuild).Height + 5;

                                    // **ALL PROJECTS**

                                    var allProjects = $"{asLog.AllProjects.ToString("HH:mm:ss:fff")}";
                                    graphics.DrawString(allProjects, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));

                                }

                            }

                            

                            break;
                        }
                    case CacheContext.Graphics:
                        {
                            if(log is GraphicsLOG glog)
                            {
                                // **MEDIUM**

                                var medium = $"{glog.Medium}";
                                graphics.DrawString(medium, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(medium).Height + 5;


                                // **FORMAT**

                                var format = $"{glog.Format}";
                                graphics.DrawString(format, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(format).Height + 5;


                                // **BRUSH**

                                var brush = $"{glog.Brush}";
                                graphics.DrawString(brush, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(brush).Height + 5;


                                // **HEIGHT**

                                var height = $"{glog.Height}";
                                graphics.DrawString(height, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(height).Height + 5;


                                // **WIDTH**

                                var width = $"{glog.Width}";
                                graphics.DrawString(width, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(width).Height + 5;


                                // **UNIT**

                                var unit = $"{glog.Unit}";
                                graphics.DrawString(unit, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(unit).Height + 5;


                                // **SIZE**
                                var size = $"{glog.Size}";
                                graphics.DrawString(size, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(size).Height + 5;


                                // **DPI**

                                var dpi = $"{glog.DPI}";
                                graphics.DrawString(dpi, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(dpi).Height + 5;


                                // **DEPTH**

                                var depth = $"{glog.Depth}";
                                graphics.DrawString(depth, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(depth).Height + 5;


                                // **ISCOMPLETED**

                                var isCompleted = glog.IsCompleted ? "Project Completed" : "Project In Progress...";
                                graphics.DrawString(isCompleted, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(isCompleted).Height + 5;


                                // **SOURCE**

                                var source = $"{glog.Source}";
                                graphics.DrawString(source, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));


                            }

                            break;
                        }
                    case CacheContext.Film:
                        {
                            if(log is FilmLOG flog)
                            {
                                // **HEIGHT**

                                var height = $"{flog.Height}";
                                graphics.DrawString(height, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(height).Height + 5;


                                // **WIDTH**

                                var width = $"{flog.Width}";
                                graphics.DrawString(width, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(width).Height + 5;


                                // **LENGTH**

                                var length = $"{flog.Length}";
                                graphics.DrawString(length, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(length).Height + 5;


                                // **ISCOMPLETED**

                                var isCompleted = flog.IsCompleted ? "Project Completed" : "Project In Progress...";
                                graphics.DrawString(isCompleted, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(isCompleted).Height + 5;


                                // **SOURCE**

                                var source = $"{flog.Source}";
                                graphics.DrawString(source, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));

                            }


                            break;
                        }
                    case CacheContext.Flexi:
                        {

                            if(log is FlexiNotesLOG flxlog)
                            {
                                // **FLEXIBLE LOG TYPE**

                                var flextLogType = $"Log Type: {flxlog.flexinotetype.ToString()}";
                                graphics.DrawString(flextLogType, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(flextLogType).Height + 5;


                                // **MEDIUM**

                                var medium = $"{flxlog.Medium}";
                                graphics.DrawString(medium, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(medium).Height + 5;


                                // **FORMAT**

                                var format = $"{flxlog.Format}";
                                graphics.DrawString(format, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(format).Height + 5;


                                // **BITRATE**

                                var bitRate = $"{flxlog.Bitrate}";
                                graphics.DrawString(bitRate, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(bitRate).Height + 5;


                                // **LENGTH**

                                var length = $"{flxlog.Length}";
                                graphics.DrawString(length, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(length).Height + 5;


                                // **ISCOMPLETED**

                                var isCompleted = flxlog.IsCompleted ? "Project Completed" : "Project In Progress...";
                                graphics.DrawString(isCompleted, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(isCompleted).Height + 5;


                                // **SOURCE**

                                var source = $"{flxlog.Source}";
                                graphics.DrawString(source, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));



                            }







                            break;
                        }
                    default:
                        {
                            if (log is CodingLOG clog)
                            {


                                // **BUGS**

                                var bugs = $"{clog.Bugs} bug(s)";
                                graphics.DrawString(bugs, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));
                                Ycoordinate += defaultFont.MeasureString(bugs).Height + 5;


                                // **APP OPENED**

                                var opened = clog.Success ? $"Application Launched Successfully" : $"No Sucessful Launch";
                                graphics.DrawString(opened, defaultFont, PdfBrushes.Black, new PointF(projectDetailsX, Ycoordinate));

                            }

                            break;
                        }
                }










                /*      **COLUMN 3**        */

                // **POST ITS**
                float staticPostItColumnX = projectDetailsX + column2Width + columnSpacing + midWaySpacing;
                float postItColumnX = staticPostItColumnX;
                Side side = Side.Right;
                float postItListY = margins.Top;
                float lineWidth = document.PageSettings.Size.Width / 2 - margins.Right - postItSidePadding * 2 - 120;

                var titleFont = new PdfTrueTypeFont(boldFontPath, 12);
                graphics.DrawString("STICKY NOTES", titleFont, PdfBrushes.Black, new PointF(postItColumnX + postItSidePadding, postItListY));
                Ycoordinate = postItListY + boldFont.MeasureString("STICKY NOTES").Height + 5;

                void AddPostIts(List<PostIt> postIts)
                {
                    foreach (var postIt in postIts)
                    {
                        if (Ycoordinate + 150 > contentHeight + maxPostItHeight)
                        {
                            Ycoordinate = margins.Top;
                            if (side == Side.Right)
                            {
                                CreateNewPage();
                                postItColumnX = margins.Left + 20;
                                side = Side.Left;
                            }
                            else
                            {
                                postItColumnX = projectDetailsX + column2Width + columnSpacing + midWaySpacing;
                                side = Side.Right;
                            }
                        }


                        // **Subject**
                        var subjectFont = new PdfTrueTypeFont(boldFontPath, 11);
                        graphics.DrawString(postIt.Subject.Subject, subjectFont, PdfBrushes.Black, new PointF(postItColumnX + postItSidePadding, Ycoordinate));
                        Ycoordinate += boldFont.MeasureString(postIt.Subject.Subject).Height + 5;

                        PdfStringFormat stringFormat = new PdfStringFormat { MeasureTrailingSpaces = true };






                        void DrawPostItSection(string text, PdfBrush colorBrush)
                        {
                            string stringContainer = string.Empty;

                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                float markerY = Ycoordinate + ((defaultFont.Height / 2) - (rectangleWidth / 2));

                                graphics.DrawEllipse(colorBrush, postItColumnX + postItSidePadding, markerY, rectangleWidth, rectangleWidth);

                                float staticXcoordinate = postItColumnX + postItSidePadding + rectangleWidth + 10;
                                float Xcoordinate = staticXcoordinate;

                                foreach (string word in text.Split(' '))
                                {
                                    if (defaultFont.MeasureString(stringContainer, stringFormat).Width + 20 > lineWidth)
                                    {
                                        Xcoordinate = staticXcoordinate;
                                        Ycoordinate += defaultFont.MeasureString(stringContainer, stringFormat).Height;
                                        stringContainer = string.Empty;
                                    }
                                    stringContainer += $"{word} ";
                                    graphics.DrawString($"{word} ", defaultFont, PdfBrushes.Black, new PointF(Xcoordinate, Ycoordinate), stringFormat);
                                    Xcoordinate = staticXcoordinate + defaultFont.MeasureString(stringContainer, stringFormat).Width;
                                }

                                Ycoordinate += defaultFont.MeasureString(stringContainer, stringFormat).Height + 15;
                            }
                        }

                        DrawPostItSection(PostItViewModel.ConvertRtfToPlainText(postIt.Error), PdfBrushes.Red);
                        DrawPostItSection(PostItViewModel.ConvertRtfToPlainText(postIt.Solution), new PdfSolidBrush(Color.FromArgb(0, 211, 34)));
                        DrawPostItSection(PostItViewModel.ConvertRtfToPlainText(postIt.Suggestion), new PdfSolidBrush(Color.FromArgb(255, 0, 245)));
                        DrawPostItSection(PostItViewModel.ConvertRtfToPlainText(postIt.Comment), new PdfSolidBrush(Color.FromArgb(255, 245, 0)));

                        Ycoordinate += 10;

                        graphics.DrawLine(PdfPens.Black, postItColumnX - 20, Ycoordinate, postItColumnX + postItSidePadding * 2 + lineWidth + 50, Ycoordinate);

                        Ycoordinate += 10;
                    }
                }

                AddPostIts(log.PostItList);












                /*      **FOOTER Continued        */

                var pageCount = document.Pages.Count;
                var pageFont = new PdfTrueTypeFont(regularFontPath, 8);
                var pageTextXcoordinate = document.PageSettings.Size.Width - margins.Right - 100;


                for (int i = 0; i < pageCount; i++)
                {
                    graphics = document.Pages[i].Graphics;
                    var page = $"page {i + 1} of {pageCount}";
                    var pageTextHeight = pageFont.MeasureString(page).Height;
                    var pageTextYcoordinate = logoCenterY - (pageTextHeight / 2);

                    graphics.DrawString(page, pageFont, PdfBrushes.Black, new PointF(pageTextXcoordinate, pageTextYcoordinate));
                }
















                










                // Save the document
                using (FileStream outputFileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    document.Save(outputFileStream);
                }

                document.Close(true);

                MessageBox.Show($"Log exported successfully. Find it in this location: {filePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch(HttpRequestException hex)
            {
                Debug.WriteLine($"HttpRequestException found in ExportLogToPDF(): {hex.Message}");
                MessageBox.Show("An error occurred on our end. We apologise for any inconvenience casued.");
            }
            catch (IOException iox)
            {
                Debug.WriteLine($"IOException found in ExportLogToPDF(): {iox.Message}");
                MessageBox.Show("An error occurred. In the event that you are overwriting a file that's open, please close it first before replacing it.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occurred in ExportLogToPDF(): {ex.Message}");
                MessageBox.Show("An error occurred on our end. We apologise for any inconvenience casued.");
            }

        }











        public async Task ExportQtLogToPDF(LOG log)
        {

            await ExportLogToPDF((CodingLOG)log, CacheContext.Qt);
        }






        public async Task ExportASLogToPDF(LOG log)
        {
            await ExportLogToPDF((AndroidCodingLOG)log, CacheContext.AndroidStudio);
        }






        public async Task ExportCodingLogToPDF(LOG log)
        {
            await ExportLogToPDF((CodingLOG)log, CacheContext.Coding);

        }






        public async Task ExportGraphicsLogToPDF(LOG log)
        {
            await ExportLogToPDF((GraphicsLOG)log, CacheContext.Graphics);
        }






        public async Task ExportFilmLogToPDF(LOG log)
        {
            await ExportLogToPDF((FilmLOG)log, CacheContext.Film);
        }






        public async Task ExportFlexiLogToPDF(LOG log)
        {
            await ExportLogToPDF((FlexiNotesLOG)log, CacheContext.Flexi);
        }
    }
}
