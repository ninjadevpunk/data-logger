using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models
{


    /// <summary>
    /// The class for creating checklists. 
    /// These items are stored within a NoteItem log.
    /// </summary>
    [Table("Item")]
    public class CheckListItem
    {
        /* PROPERTIES */
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int itemID { get; set; } = -1;



        public int logID { get; set; }

        public virtual CheckList Checklist { get; set; }




        [ForeignKey("accountID")]
        public ACCOUNT Author { get; set; }

        public int accountID { get; set; }




        public bool IsChecked { get; set; } = false;

        public string Item { get; set; } = "";




        /* CONSTRUCTORS */



        public CheckListItem() { }

        public CheckListItem(bool isChecked, string item)
        {
            IsChecked = isChecked;
            Item = item;
        }

        public CheckListItem(int id, bool isChecked, string item)
        {
            itemID = id;
            IsChecked = isChecked;
            Item = item;
        }



        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is CheckListItem item &&
                   itemID == item.itemID &&
                   IsChecked == item.IsChecked &&
                   Item == item.Item;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(itemID, IsChecked, Item);
        }

        public static bool operator ==(CheckListItem left, CheckListItem right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CheckListItem left, CheckListItem right)
        {
            return !left.Equals(right);
        }
    }
}
