using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Data_Logger_1._3.Models
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
        public ACCOUNT Author { get; set; }

        public int accountID { get; set; }

        /// <summary>
        /// The project that is associated with the log.
        /// </summary>

        [ForeignKey("projectID")]
        public ProjectClass Project { get; set; }

        public int projectID { get; set; }

        /// <summary>
        /// The application in which the project was created in.
        /// </summary>
        [ForeignKey("appID")]
        public ApplicationClass Application { get; set; }

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
        public OutputClass Output { get; set; }

        public int outputID { get; set; }

        /// <summary>
        /// What type of build is created in this project.
        /// </summary>
        [ForeignKey("typeID")]
        public TypeClass Type { get; set; }

        public int typeID { get; set; }

        /// <summary>
        /// The most important property of the log. All the thoughts, 
        /// suggestions and details of the project are described in PostIts.
        /// </summary>

        public List<PostIt> PostItList { get; set; } = new();


        /* ENTITY FRAMEWORK */





        public string Content { get; set; } = string.Empty;



        /** CONSTRUCTOR */
        public LOG()
        {
            // Blank
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

        protected LOG(ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, OutputClass output, TypeClass type, List<PostIt> postItList, string content)
        {
            Author = author;
            Project = projectName;
            Application = applicationName;
            Start = startTime;
            End = endTime;
            Output = output;
            Type = type;
            PostItList = postItList;
        }

        protected LOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime, OutputClass output, TypeClass type, List<PostIt> postItList, string content)
        {
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



        /* COUNT */

        /** Helper function **/
        public int helper(string type)
        {
            int count = 0;
            string pattern = "[A-Za-z]{5}[0-9]{0}";
            Regex xp = new Regex(pattern);

            foreach (PostIt n in PostItList)
            {
                switch (type)
                {
                    case "error":
                        {
                            if (xp.IsMatch(n.Error))
                                ++count;
                        }
                        break;
                    case "solution":
                        {
                            if (xp.IsMatch(n.Solution))
                                ++count;
                        }
                        break;
                    case "suggestion":
                        {
                            if (xp.IsMatch(n.Suggestion))
                                ++count;
                        }
                        break;
                    case "comment":
                        {
                            if (xp.IsMatch(n.Comment))
                                ++count;
                        }
                        break;
                }
            }

            return count;
        }


        /** Count how many errors there are
         */
        public int ErrorCount()
        {
            return helper("error");
        }

        /** Count how many solutions there are
         */
        public int SolutionCount()
        {
            return helper("solution");
        }

        /** Count how many suggestions there are
         */
        public int SuggestionCount()
        {
            return helper("suggestion");
        }

        /** Count how many comments there are
         */
        public int CommentCount()
        {
            return helper("comment");
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
                   EqualityComparer<List<PostIt>>.Default.Equals(PostItList, lOG.PostItList) &&
                   Content == lOG.Content;
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
            hash.Add(Content);
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
