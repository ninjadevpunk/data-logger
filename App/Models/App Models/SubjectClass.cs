
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// The most important part of a LOG. Provides clear context of the problem at hand.
    /// </summary>
    [Table("Subject")]
    public class SubjectClass
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int subjectID { get; set; }

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;


        public ACCOUNT User { get; set; }

        public int accountID { get; set; }


        public string Subject { get; set; } = "";

        public int projectID { get; set; }
        public ProjectClass Project { get; set; }


        public int appID { get; set; }
        public ApplicationClass Application { get; set; }

        public SubjectClass()
        {
        }

        public SubjectClass(string subjectName)
        {
            Subject = subjectName;
        }

        public SubjectClass(LOG.CATEGORY category, ACCOUNT user, string subject)
        {
            Category = category;
            accountID = user.accountID;
            Subject = subject;
        }

        public SubjectClass(int subjectID, LOG.CATEGORY category, ACCOUNT user, string subject, ProjectClass project, ApplicationClass application)
        {
            this.subjectID = subjectID;
            Category = category;
            User = user;
            Subject = subject;
            Project = project;
            Application = application;
        }
        
        public SubjectClass(LOG.CATEGORY category, ACCOUNT user, string subject, ProjectClass project, ApplicationClass application)
        {
            Category = category;
            accountID = user.accountID;
            Subject = subject;
            Project = project;
            Application = application;
        }

        public override bool Equals(object? obj)
        {
            return obj is SubjectClass @class &&
                   subjectID == @class.subjectID &&
                   Category == @class.Category &&
                   User == @class.User &&
                   Subject == @class.Subject &&
                   Project.Equals(@class.Project) &&
                   Application.Equals(@class.Application);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(subjectID, Category, User, Subject, Project, Application);
        }

        public static bool operator ==(SubjectClass? left, SubjectClass? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(SubjectClass? left, SubjectClass? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }

    }
}
