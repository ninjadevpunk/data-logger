using Core.Models.App_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{

    /// <summary>
    /// The parent class. Defines all common log properties.
    /// </summary>
    [Table("LOG")]
    public abstract class LOG
    {
        /* ENUMS */


        /// <summary>
        /// Log categories. All LOGs can only be Coding, Graphics, 
        /// Film or Notes types.
        /// </summary>
        public enum CATEGORY { CODING, GRAPHICS, FILM, NOTES }





        /* PROPERTIES */


        public CATEGORY Category { get; protected set; }

        /// <summary>
        /// The log's identifier. Important for database and caching operations.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// The creator of a log.
        /// </summary>
        [ForeignKey("accountID")]
        public ACCOUNT Author { get; set; } = null!;

        public int accountID { get; set; }

        /// <summary>
        /// The project that is associated with the log.
        /// </summary>

        [ForeignKey("projectID")]
        public ProjectClass Project { get; set; } = null!;

        public int projectID { get; set; }

        /// <summary>
        /// The application in which the project was created in.
        /// </summary>
        [ForeignKey("appID")]
        public ApplicationClass Application { get; set; } = null!;

        public int appID { get; set; }



        /// <summary>
        /// The start date and time of the project.
        /// </summary>
        public DateTime Start { get; set; } = DateTime.Now;

        /// <summary>
        /// The end date and time of the project.
        /// </summary>
        public DateTime End { get; set; } = DateTime.Now;



        /// <summary>
        /// The deliverable for the project.
        /// </summary>
        [ForeignKey("outputID")]
        public OutputClass Output { get; set; } = null!;

        public int outputID { get; set; }

        /// <summary>
        /// What type of build is created in this project.
        /// </summary>
        [ForeignKey("typeID")]
        public TypeClass Type { get; set; } = null!;

        public int typeID { get; set; }

        /// <summary>
        /// The most important property of the log. All the thoughts, 
        /// suggestions and details of the project are described in PostIts.
        /// </summary>

        public List<PostIt> PostItList { get; set; } = new();


        /* ENTITY FRAMEWORK */




        /** CONSTRUCTOR */
        public LOG()
        {

        }

        protected LOG(CATEGORY category, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, OutputClass output, TypeClass type, List<PostIt> postItList)
        {
            Category = category;
            Author = author;
            Project = projectName;
            Application = applicationName;
            Start = startTime;
            End = endTime;
            Output = output;
            Type = type;
            PostItList = postItList;
        }

        protected LOG(CATEGORY category, int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, OutputClass output, TypeClass type, List<PostIt> postItList)
        {
            Category = category;
            ID = id;
            Author = author;
            Project = projectName;
            Application = applicationName;
            Start = startTime;
            End = endTime;
            Output = output;
            Type = type;
            PostItList = postItList;
        }




        public override bool Equals(object? obj)
        {
            return obj is LOG lOG &&
                   Category == lOG.Category &&
                   ID == lOG.ID &&
                   Author.Equals(lOG.Author) &&
                   Project == lOG.Project &&
                   Application == lOG.Application &&
                   Start == lOG.Start &&
                   End == lOG.End &&
                   Output == lOG.Output &&
                   Type == lOG.Type &&
                   EqualityComparer<List<PostIt>>.Default.Equals(PostItList, lOG.PostItList);
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
            return hash.ToHashCode();
        }

        public static bool operator ==(LOG? left, LOG? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(LOG? left, LOG? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }

    }
}
