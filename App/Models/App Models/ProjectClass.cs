
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// Represents a project in which a log is based in.
    /// </summary>
    [Table("PROJECT")]
    public class ProjectClass
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int projectID { get; set; }

        public int accountID { get; set; }
        public ACCOUNT User { get; set; }


        public string Name { get; set; }


        public int appID { get; set; }
        public virtual ApplicationClass Application { get; set; }



        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public bool IsDefault { get; set; } = false;




        public ProjectClass()
        {
        }

        public ProjectClass(string projectName)
        {
            Name = projectName;
        }

        public ProjectClass(string projectName, ApplicationClass app)
        {
            Name = projectName;
            Application = app;
        }

        public ProjectClass(ACCOUNT user, string name, ApplicationClass application)
        {
            accountID = user.accountID;
            Name = name;
            Application = application;
        }

        public ProjectClass(int projectID, string name, ApplicationClass application)
        {
            this.projectID = projectID;
            Name = name;
            Application = application;
            Category = LOG.CATEGORY.CODING;
        }



        public ProjectClass(int projectID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            this.projectID = projectID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }

        public ProjectClass(int projectID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category, bool isDefault)
        {
            this.projectID = projectID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
            IsDefault = isDefault;
        }
        
        public ProjectClass(ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category, bool isDefault)
        {
            User = user;
            Name = name;
            Application = application;
            Category = category;
            IsDefault = isDefault;
        }

        public override bool Equals(object? obj)
        {
            return obj is ProjectClass @class &&
                   projectID == @class.projectID &&
                   User == @class.User &&
                   IsDefault == @class.IsDefault &&
                   Name == @class.Name &&
                   Application == @class.Application &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(projectID, User, Name, Application, Category, IsDefault);
        }

        public static bool operator ==(ProjectClass? left, ProjectClass? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ProjectClass? left, ProjectClass? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }

    }
}
