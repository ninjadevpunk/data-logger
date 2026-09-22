using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The LOG class for video projects.
    /// </summary>
    [Table("FilmLOG")]
    public class FilmLOG : LOG
    {


        /* MEMBER VARIABLES */


        /// <summary>
        /// The height of the footage.
        /// </summary>

        public double Height { get; set; } = 0.0;

        /// <summary>
        /// The width of the footage.
        /// </summary>

        public double Width { get; set; } = 0.0;

        /// <summary>
        /// The length of the footage. Please use video time format. e.g. 00:00
        /// </summary>

        public string Length { get; set; } = "0:00";

        /// <summary>
        /// Whether the video has been completed or filmed.
        /// </summary>

        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// The location of the video regardless of it being online or not.
        /// </summary>

        public string Source { get; set; } = @"C:\";


        /* CONSTRUCTORS */




        public FilmLOG()
        {
            Category = CATEGORY.FILM;
        }

        public FilmLOG(double height, double width, string length, bool isCompleted, string source)
        {
            Category = CATEGORY.FILM;
            Height = height;
            Width = width;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }

        public FilmLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList,
            double height, double width, string length, bool isCompleted, string source) : base(LOG.CATEGORY.FILM, id, author, projectName, applicationName,
                                                                                                            startTime, endTime, output, type, postItList)
        {
            Category = CATEGORY.FILM;
            Height = height;
            Width = width;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }



        /* OVERLOADS */




        public override bool Equals(object? obj)
        {
            return obj is FilmLOG lOG &&
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
                   Height == lOG.Height &&
                   Width == lOG.Width &&
                   Length == lOG.Length &&
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
            hash.Add(Height);
            hash.Add(Width);
            hash.Add(Length);
            hash.Add(IsCompleted);
            hash.Add(Source);
            return hash.ToHashCode();
        }

        public static bool operator ==(FilmLOG left, FilmLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FilmLOG left, FilmLOG right)
        {
            return !left.Equals(right);
        }
    }
}
