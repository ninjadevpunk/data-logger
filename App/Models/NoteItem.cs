
using Data_Logger_1._3.Models.App_Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Logger_1._3.Models
{
    [Table("NoteItem")]
    public class NoteItem : NotesLOG
    {
        /* DOCUMENTATION
         * 
         * Please use this class for generic notes!
         * 
         */



        /* MEMBER VARIABLES */



        /// <summary>
        /// This log is strictly a generic notes log.
        /// </summary>
        public override NOTELOGType notelogtype => NOTELOGType.GENERIC;

        public string Subject { get; set; } = "No Subject";

        /// <summary>
        /// The note's PostIt content.
        /// </summary>
        public string NoteContent { get; set; } = "";


        /// <summary>
        /// If the note is meant as a checklist note, this property 
        /// will be used to store CheckListItems. In the event that a 
        /// checklist is not created it will remain null.
        /// </summary>
        public int? CheckListID { get; set; }

        [ForeignKey("CheckListID")]
        public virtual CheckList? Checklist { get; set; }




        /* CONSTRUCTORS */



        public NoteItem()
        {
        }

        // For Generic Notes
        public NoteItem(string subject, string genericNote)
        {
            Subject = subject;
            Content = genericNote;
        }

        // For Checklists
        public NoteItem(string subject, string genericNote, CheckList items)
        {
            Subject = subject;
            Content = genericNote;
            Checklist = items;
        }

        public NoteItem(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList,
            string subject, string genericNote) : base(id, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Subject = subject;
            Content = genericNote;
        }

        public NoteItem(int id, ACCOUNT author, ProjectClass projectName, ApplicationClass applicationName, DateTime startTime, DateTime endTime,
            OutputClass output, TypeClass type, List<PostIt> postItList,
            string subject, string genericNote, CheckList items) : base(id, author, projectName, applicationName, startTime, endTime, output, type, postItList)
        {
            Subject = subject;
            Content = genericNote;
            Checklist = items;
        }


        /* OVERLOADS */



        public override bool Equals(object? obj)
        {
            return obj is NoteItem item &&
                   base.Equals(obj) &&
                   Category == item.Category &&
                   Author == item.Author &&
                   Project == item.Project &&
                   Application == item.Application &&
                   Start == item.Start &&
                   End == item.End &&
                   Output == item.Output &&
                   Type == item.Type &&
                   PostItList.Equals(item.PostItList) &&
                   notelogtype == item.notelogtype &&
                   Subject == item.Subject &&
                   Content == item.Content &&
                   Checklist == item.Checklist;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(base.GetHashCode());
            hash.Add(Category);
            hash.Add(Author);
            hash.Add(Project);
            hash.Add(Application);
            hash.Add(Start);
            hash.Add(End);
            hash.Add(Output);
            hash.Add(Type);
            hash.Add(PostItList);
            hash.Add(notelogtype);
            hash.Add(Subject);
            hash.Add(Content);
            hash.Add(Checklist);
            return hash.ToHashCode();
        }

        public static bool operator ==(NoteItem? left, NoteItem? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }

        public static bool operator !=(NoteItem? left, NoteItem? right)
        {
            if (ReferenceEquals(left, right)) return false;
            if (left is null || right is null) return true;
            return !left.Equals(right);
        }

    }
}
