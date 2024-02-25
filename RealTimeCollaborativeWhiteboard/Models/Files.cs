using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class Files
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeskID { get; set; }
        public string? UrlPhoto { get; set; }
        [ForeignKey("User")]
        public string? CurrUserID { get; set; }
        public User? user { get; set; }

        public IEnumerator<Files> GetEnumerator()
        {
            yield return this; 
        }
    }
}
