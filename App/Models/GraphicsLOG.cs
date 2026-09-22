
using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data_Logger_1._3.Models
{
    /// <summary>
    /// This class is reserved for graphics such as drawing, painting and logging for those kinds of projects.
    /// </summary>
    [Table("GraphicsLOG")]
    public class GraphicsLOG : LOG
    {

        /* MEMBER VARIABLES */

        public MediumClass Medium { get; set; }
        public int mediumID { get; set; }


        public FormatClass Format { get; set; }
        public int formatID { get; set; }


        public string Brush { get; set; } = "";


        public double Height { get; set; } = 0.0;


        public double Width { get; set; } = 0.0;


        public MeasuringUnitClass Unit { get; set; }
        public int unitID { get; set; }


        public string Size { get; set; } = "A4";


        public double DPI { get; set; } = 0.0;


        public string Depth { get; set; } = "8-bit";


        public bool IsCompleted { get; set; } = false;


        public string Source { get; set; } = @"C:\";



        /* CONSTRUCTORS */




        public GraphicsLOG() { }

        public GraphicsLOG(MediumClass medium, FormatClass format, string brush, double height, double width, MeasuringUnitClass unit, string size, double dPI, string depth, bool isCompleted, string source)
        {
            Category = CATEGORY.GRAPHICS;
            Medium = medium;
            Format = format;
            Brush = brush;
            Height = height;
            Width = width;
            Unit = unit;
            Size = size;
            DPI = dPI;
            Depth = depth;
            IsCompleted = isCompleted;
            Source = source;
        }

        public GraphicsLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList,
            MediumClass medium, FormatClass format, string brush, double height, double width,
            MeasuringUnitClass unit, string size, double dPI, string depth, bool isCompleted, string source) : base(LOG.CATEGORY.GRAPHICS, id, author, projectName, applicationName,
                                                                                                            startTime, endTime, output, type, postItList)
        {
            Category = CATEGORY.GRAPHICS;
            Medium = medium;
            Format = format;
            Brush = brush;
            Height = height;
            Width = width;
            Unit = unit;
            Size = size;
            DPI = dPI;
            Depth = depth;
            IsCompleted = isCompleted;
            Source = source;
        }


        /* OVERLOADS */




        public override bool Equals(object? obj)
        {
            return obj is GraphicsLOG lOG &&
                   Category == lOG.Category &&
                   ID == lOG.ID &&
                   Author == lOG.Author &&
                   Project == lOG.Project &&
                   Application == lOG.Application &&
                   Start == lOG.Start &&
                   End == lOG.End &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   PostItList.Equals(lOG.PostItList) &&
                   Medium == lOG.Medium &&
                   Format == lOG.Format &&
                   Brush == lOG.Brush &&
                   Height == lOG.Height &&
                   Width == lOG.Width &&
                   Unit == lOG.Unit &&
                   Size == lOG.Size &&
                   DPI == lOG.DPI &&
                   Depth == lOG.Depth &&
                   IsCompleted == lOG.IsCompleted &&
                   Source == lOG.Source;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Category);
            hash.Add(ID);
            hash.Add(Author);
            hash.Add(Project);
            hash.Add(Application);
            hash.Add(Start);
            hash.Add(End);
            hash.Add(Output);
            hash.Add(Type);
            hash.Add(PostItList);
            hash.Add(Category);
            hash.Add(Medium);
            hash.Add(Format);
            hash.Add(Brush);
            hash.Add(Height);
            hash.Add(Width);
            hash.Add(Unit);
            hash.Add(Size);
            hash.Add(DPI);
            hash.Add(Depth);
            hash.Add(IsCompleted);
            hash.Add(Source);
            return hash.ToHashCode();
        }

        public static bool operator ==(GraphicsLOG left, GraphicsLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(GraphicsLOG left, GraphicsLOG right)
        {
            return !left.Equals(right);
        }
    }
}
