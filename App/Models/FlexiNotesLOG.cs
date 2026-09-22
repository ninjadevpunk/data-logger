using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// Please use this flexible LOG class for logs that don't fit under coding, 
    /// graphics, or film.
    /// </summary>
    [Table("FlexiNotesLOG")]
    public class FlexiNotesLOG : NotesLOG
    {

        /* ENUMS */



        /// <summary>
        /// The type of Flexi Notes LOG. It's either a document type, music or gaming type.
        /// </summary>
        public enum FLEXINOTEType { Document, Music, Gaming }


        /// <summary>
        /// The type of log for the gaming Flexi Notes LOG.
        /// </summary>
        public enum GAMINGContext { Create, Play }


        /* MEMBER VARIABLES */



        /// <summary>
        /// This log is strictly a flexi notes log.
        /// </summary>

        public override NOTELOGType notelogtype => NOTELOGType.FLEXI;

        /// <summary>
        /// Specify the type of Flexi Notes LOG.
        /// </summary>

        public FLEXINOTEType flexinotetype { get; set; } = FLEXINOTEType.Document;

        /// <summary>
        /// Specify the type of context in which a game is being logged.
        /// </summary>

        public GAMINGContext gamingcontext { get; set; } = GAMINGContext.Create;

        /// <summary>
        /// The medium. Used for music logs.
        /// </summary>

        public MediumClass Medium { get; set; }
        public int mediumID { get; set; }

        /// <summary>
        /// The format. Used for music logs.
        /// </summary>

        public FormatClass Format { get; set; }
        public int formatID { get; set; }

        /// <summary>
        /// The bitrate. Used for music logs primarily.
        /// </summary>

        public int Bitrate { get; set; } = 0;

        /// <summary>
        /// The length of the song.
        /// </summary>

        public string Length { get; set; } = "0:00";

        /// <summary>
        /// Regardless of project type, specify if it is completed here.
        /// </summary>

        public bool IsCompleted { get; set; } = false;

        /// <summary>
        /// Store the source of the project regardless of its type.
        /// </summary>

        public string Source { get; set; } = @"C:\";



        /* CONSTRUCTORS */




        public FlexiNotesLOG()
        {
            Category = CATEGORY.NOTES;
        }

        public FlexiNotesLOG(FLEXINOTEType flexinotetype, GAMINGContext gamingcontext, MediumClass medium, FormatClass format, int bitRate,
            string length, bool isCompleted, string source)
        {
            Category = CATEGORY.NOTES;
            this.flexinotetype = flexinotetype;
            this.gamingcontext = gamingcontext;
            Medium = medium;
            Format = format;
            Bitrate = bitRate;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }

        public FlexiNotesLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList,
            FLEXINOTEType flexinotetype, GAMINGContext gamingcontext, MediumClass medium, FormatClass format, int bitRate,
            string length, bool isCompleted, string source) : base(id, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Category = CATEGORY.NOTES;
            this.flexinotetype = flexinotetype;
            this.gamingcontext = gamingcontext;
            Medium = medium;
            Format = format;
            Bitrate = bitRate;
            Length = length;
            IsCompleted = isCompleted;
            Source = source;
        }



        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is FlexiNotesLOG lOG &&
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
                   notelogtype == lOG.notelogtype &&
                   flexinotetype == lOG.flexinotetype &&
                   gamingcontext == lOG.gamingcontext &&
                   Medium == lOG.Medium &&
                   Format == lOG.Format &&
                   Bitrate == lOG.Bitrate &&
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
            hash.Add(notelogtype);
            hash.Add(flexinotetype);
            hash.Add(gamingcontext);
            hash.Add(Medium);
            hash.Add(Format);
            hash.Add(Bitrate);
            hash.Add(Length);
            hash.Add(IsCompleted);
            hash.Add(Source);
            return hash.ToHashCode();
        }

        public static bool operator ==(FlexiNotesLOG left, FlexiNotesLOG right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FlexiNotesLOG left, FlexiNotesLOG right)
        {
            return !left.Equals(right);
        }
    }
}
