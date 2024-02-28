using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class Music
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MusicId { get; set; }
        public string? UrlMusic { get; set; }
        public string? CurrUserID { get; set; }
        public User? user { get; set; }
        public IEnumerator<Music> GetEnumerator()
        {
            yield return this;
        }
    }
}
