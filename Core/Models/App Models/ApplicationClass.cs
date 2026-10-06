using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models.App_Models
{
    /// <summary>
    /// The application context of a log. What application was the project created in shold stored here.
    /// </summary>
    [Table("APPLICATION")]
    public class ApplicationClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int appID { get; set; }


        public int accountID { get; set; }
        public virtual ACCOUNT? User { get; set; }



        public string Name { get; set; } = string.Empty;

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public bool IsDefault { get; set; }


        public ApplicationClass()
        {
        }


        public override bool Equals(object? obj)
        {
            return obj is ApplicationClass @class &&
                   appID == @class.appID &&
                   User == @class.User &&
                   Name == @class.Name &&
                   Category == @class.Category &&
                   IsDefault == @class.IsDefault;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(appID, User, Name, Category, IsDefault);
        }

        public static bool operator ==(ApplicationClass? left, ApplicationClass? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ApplicationClass? left, ApplicationClass? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }

    }
}
