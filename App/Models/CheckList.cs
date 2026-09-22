using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models
{


    /// <summary>
    /// Represents a collection of checklist items associated with a NoteItem log.
    /// </summary>
    [Table("CHECKLIST")]
    public class CheckList
    {
        /* PROPERTIES */



        [ForeignKey("accountID")]
        public ACCOUNT Author { get; set; }

        public int accountID { get; set; }

        /// <summary>
        /// Navigation property for the associated NoteItem.
        /// </summary>
        [Key]
        public int logID { get; set; }

        [Required]
        public virtual NoteItem NoteItem { get; set; }

        /// <summary>
        /// Collection of items in the checklist.
        /// </summary>
        public virtual List<CheckListItem> Items { get; set; } = new List<CheckListItem>();


        /* CONSTRUCTORS */

        /// <summary>
        /// Default constructor for EF.
        /// </summary>
        public CheckList() { }

        /// <summary>
        /// Initializes a new checklist with a single item.
        /// </summary>
        public CheckList(bool isChecked, string item)
        {
            Items.Add(new CheckListItem(isChecked, item));
        }

        /// <summary>
        /// Initializes a new checklist with an existing checklist item.
        /// </summary>
        public CheckList(CheckListItem item)
        {
            Items.Add(item);
        }


        /* MEMBER FUNCTIONS */

        /// <summary>
        /// Adds a new item to the checklist.
        /// </summary>
        public void Add(bool isChecked, string item)
        {
            Items.Add(new CheckListItem(isChecked, item));
        }


        /* OVERRIDES */

        public override bool Equals(object? obj)
        {
            return obj is CheckList list &&
                   Items.SequenceEqual(list.Items);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Items);
        }

        public static bool operator !=(CheckList? left, CheckList? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }

        public static bool operator ==(CheckList? left, CheckList? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }
    }
}
