using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class Desk
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? DeskID { get; set; }
        public string? Title { get; set; }
        public string? UrlPhoto { get; set; }
        [ForeignKey("User")]
        public int CurrUserID { get; set; }
        public User? user { get; set; }
    }
}
