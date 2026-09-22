using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
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
        public virtual ACCOUNT User { get; set; }



        public string Name { get; set; }

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public bool IsDefault { get; set; } = false;


        public ApplicationClass()
        {
        }

        public ApplicationClass(string applicationName)
        {
            Name = applicationName;
        }

        public ApplicationClass(int appID, string name)
        {
            this.appID = appID;
            Name = name;
        }

        public ApplicationClass(ACCOUNT user, string name)
        {
            accountID = user.accountID;
            Name = name;
        }

        public ApplicationClass(int appID, string name, LOG.CATEGORY category)
        {
            this.appID = appID;
            Name = name;
            Category = category;
        }

        public ApplicationClass(int appID, ACCOUNT user, string name, bool isDefault)
        {
            this.appID = appID;
            Name = name;
            IsDefault = isDefault;
        }

        public ApplicationClass(int appID, ACCOUNT user, string name, LOG.CATEGORY category)
        {
            this.appID = appID;
            User = user;
            Name = name;
            Category = category;
        }

        public ApplicationClass(int appID, ACCOUNT user, string name, LOG.CATEGORY category, bool isDefault)
        {
            this.appID = appID;
            User = user;
            Name = name;
            Category = category;
            IsDefault = isDefault;
        }

        public ApplicationClass(ACCOUNT user, string name, LOG.CATEGORY category, bool isDefault)
        {
            User = user;
            Name = name;
            Category = category;
            IsDefault = isDefault;
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
