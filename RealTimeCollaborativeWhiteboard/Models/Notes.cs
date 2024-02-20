using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class Notes 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotesId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
