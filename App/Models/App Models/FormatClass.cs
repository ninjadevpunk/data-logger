using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// The format of a log.
    /// </summary>
    [Table("FORMAT")]
    public class FormatClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int formatID { get; set; }

        public ACCOUNT User { get; set; }

        public int accountID { get; set; }

        public string Format { get; set; } = "Digital Canvas";

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.GRAPHICS;

        public FormatClass()
        {
            
        }

        public FormatClass(string format)
        {
            Format = format;
        }

        public FormatClass(int formatID, ACCOUNT user, string format, LOG.CATEGORY category)
        {
            this.formatID = formatID;
            User = user;
            Format = format;
            Category = category;
        }

        public override bool Equals(object? obj)
        {
            return obj is FormatClass @class &&
                   formatID == @class.formatID &&
                   User.Equals(@class.User) &&
                   Format == @class.Format &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(formatID, User, Format, Category);
        }

        public static bool operator ==(FormatClass left, FormatClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FormatClass left, FormatClass right)
        {
            return !left.Equals(right);
        }
    }
}
