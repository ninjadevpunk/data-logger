using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// The medium for a GraphicsLOG.
    /// </summary>
    [Table("MEDIUM")]
    public class MediumClass
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int mediumID { get; set; }


        public ACCOUNT User { get; set; }

        public int accountID { get; set; }


        public string Medium { get; set; } = "Pencil";

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.GRAPHICS;




        public MediumClass()
        {
            
        }

        public MediumClass(string medium)
        {
            Medium = medium;
        }

        public MediumClass(int mediumID, ACCOUNT user, string medium, LOG.CATEGORY category)
        {
            this.mediumID = mediumID;
            User = user;
            Medium = medium;
            Category = category;
        }

        public override bool Equals(object? obj)
        {
            return obj is MediumClass @class &&
                   mediumID == @class.mediumID &&
                   User.Equals(@class.User) &&
                   Medium == @class.Medium &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(mediumID, User, Medium, Category);
        }

        public static bool operator ==(MediumClass left, MediumClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MediumClass left, MediumClass right)
        {
            return !left.Equals(right);
        }
    }
}
