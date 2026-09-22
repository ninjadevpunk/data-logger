using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models
{



    /// <summary>
    /// Class for feedback submissions.
    /// </summary>
    [Table("FEEDBACK")]
    public class FeedbackMessage
    {
        public enum FeedbackType { Bug, Exception, Crash  }



        public FeedbackType Category { get; set; } = FeedbackType.Bug;

        [Key]
        public int feedbackID { get; set; }


        /// <summary>
        /// The one sendng the feedback.
        /// </summary>
        [ForeignKey("accountID")]
        public ACCOUNT User { get; set; }

        public int accountID { get; set; }

        /// <summary>
        /// The date the feedback was sent.
        /// </summary>
        public DateTime DateReported { get; set; } = DateTime.Now;

        /// <summary>
        /// Description of the problem reported.
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// Whether the user can be contacted or not.
        /// </summary>
        public bool CanContact { get; set; } = false;

        /// <summary>
        /// Specifies if the app automatically created the feedback message.
        /// </summary>
        public bool IsAutoFeed { get; set; } = true;
    }
}
