using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class Board 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BoardId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
