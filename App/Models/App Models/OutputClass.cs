
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models.App_Models
{
    /// <summary>
    /// Represents the output or deliverable of the project in which a log is based in.
    /// </summary>
    [Table("OUTPUT")]
    public class OutputClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int outputID { get; set; }


        public ACCOUNT User { get; set; }

        public int accountID { get; set; }


        public string Name { get; set; } = "Console Application";


        public int appID { get; set; } = 3;
        public virtual ApplicationClass Application { get; set; }


        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public OutputClass()
        {
        }

        public OutputClass(string outputName)
        {
            Name = outputName;
        }

        public OutputClass(int outputID, string outputName)
        {
            this.outputID = outputID;
            Name = outputName;
        }

        public OutputClass(int outputID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            this.outputID = outputID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }
        
        public OutputClass(ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }

        public override bool Equals(object? obj)
        {
            return obj is OutputClass @class &&
                   outputID == @class.outputID &&
                   User.Equals(@class.User) &&
                   Name == @class.Name &&
                   Application.Equals(@class.Application) &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(outputID, User, Name, Application, Category);
        }

        public static bool operator ==(OutputClass left, OutputClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OutputClass left, OutputClass right)
        {
            return !left.Equals(right);
        }
    }
}
