
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// Represents a reason for the log being logged.
    /// </summary>
    [Table("TYPE")]
    public class TypeClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int typeID { get; set; }


        public ACCOUNT User { get; set; }

        public int accountID { get; set; }


        public string Name { get; set; } = "NONE";


        public ApplicationClass Application { get; set; }
        public int appID { get; set; } = 3;

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public TypeClass()
        {
        }

        public TypeClass(string typeName)
        {
            Name = typeName;
        }

        public TypeClass(int typeID, string typeName)
        {
            this.typeID = typeID;
            Name = typeName;
        }

        public TypeClass(int typeID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            this.typeID = typeID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }
        
        public TypeClass(ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }

        public override bool Equals(object? obj)
        {
            return obj is TypeClass @class &&
                   typeID == @class.typeID &&
                   User.Equals(@class.User) &&
                   Name == @class.Name &&
                   Application.Equals(@class.Application) &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(typeID, User, Name, Application, Category);
        }

        public static bool operator ==(TypeClass left, TypeClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeClass left, TypeClass right)
        {
            return !left.Equals(right);
        }
    }
}
