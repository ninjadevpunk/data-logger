
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.App_Models
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
        public ACCOUNT User { get; set; } = null!;


        public string Name { get; set; } = string.Empty;


        public int appID { get; set; }
        public virtual ApplicationClass Application { get; set; } = null!;



        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public bool IsDefault { get; set; } = false;



        public ProjectClass()
        {
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
