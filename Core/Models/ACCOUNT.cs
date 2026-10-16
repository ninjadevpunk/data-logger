using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    /// <summary>
    /// The main authoring class. Represents Data Logger users.
    /// </summary>
    [Table("ACCOUNT")]
    public class ACCOUNT
    {
        /* PROPERTIES */


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int accountID { get; set; }

        public string ProfilePic { get; set; } = string.Empty;


        public string FirstName { get; set; } = "";


        public string LastName { get; set; } = "";


        public string Email { get; set; } = "";


        public string Password { get; set; } = "";


        public bool IsEmployee { get; set; } = false;


        public string? CompanyName { get; set; } = "";


        public string? CompanyAddress { get; set; } = "";


        public string? CompanyLogo { get; set; } = "";


        public bool IsOnline { get; set; } = false;


        /* ---CONSTRUCTORS--- */


        public ACCOUNT()
        {

        }




        /* OVERLOADS */


        public override bool Equals(object? obj)
        {
            return obj is ACCOUNT aCCOUNT &&
                   accountID == aCCOUNT.accountID &&
                   ProfilePic == aCCOUNT.ProfilePic &&
                   FirstName == aCCOUNT.FirstName &&
                   LastName == aCCOUNT.LastName &&
                   Email == aCCOUNT.Email &&
                   Password == aCCOUNT.Password &&
                   IsEmployee == aCCOUNT.IsEmployee &&
                   CompanyName == aCCOUNT.CompanyName &&
                   CompanyAddress == aCCOUNT.CompanyAddress &&
                   CompanyLogo == aCCOUNT.CompanyLogo &&
                   IsOnline == aCCOUNT.IsOnline;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(accountID);
            hash.Add(ProfilePic);
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(Email);
            hash.Add(Password);
            hash.Add(IsEmployee);
            hash.Add(CompanyName);
            hash.Add(CompanyAddress);
            hash.Add(CompanyLogo);
            hash.Add(IsOnline);
            return hash.ToHashCode();
        }

        public static bool operator ==(ACCOUNT? left, ACCOUNT? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(ACCOUNT? left, ACCOUNT? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }





    }
}
