
using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data_Logger_1._3.Models
{

    /// <summary>
    /// The primary LOG sub class. The Coding class has the most Data Logger support 
    /// and is used for coding projects in any coding app - 
    /// especially Qt Creator and Android Studio.
    /// </summary>
    [Table("CodingLOG")]
    public class CodingLOG : LOG
    {

        /* MEMBER VARIABLES */


        /// <summary>
        /// Store the number of bugs found.
        /// </summary>

        public int Bugs { get; set; } = 0;

        /// <summary>
        /// Store the application launch details here like whether the app opened or not.
        /// </summary>

        public bool Success { get; set; } = false;


        /* CONSTRUCTORS */

        public CodingLOG()
        {
            // Blank
        }

        public CodingLOG(int bugs, bool success)
        {
            Category = CATEGORY.CODING;
            Bugs = bugs;
            Success = success;
        }

        public CodingLOG(ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList, int bugs, bool success) : base(LOG.CATEGORY.CODING, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Category = CATEGORY.CODING;
            Bugs = bugs;
            Success = success;
        }

        public CodingLOG(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList, int bugs, bool success) : base(LOG.CATEGORY.CODING, id, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Category = CATEGORY.CODING;
            Bugs = bugs;
            Success = success;
        }

        

        public override bool Equals(object? obj)
        {
            if (obj is not CodingLOG log)
                return false;

            return base.Equals(log) &&
                   Bugs == log.Bugs &&
                   Success == log.Success;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Bugs, Success);
        }

        public static bool operator ==(CodingLOG cLOG1, CodingLOG cLOG2)
        {
            return cLOG1.Equals(cLOG2);
        }

        public static bool operator !=(CodingLOG cLOG1, CodingLOG cLOG2)
        {
            return !cLOG1.Equals(cLOG2);
        }


        /* OVERLOADS */




    }
}
