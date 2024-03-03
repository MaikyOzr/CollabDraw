using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class DocFiles
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocId { get; set; }
        public string? DocUrl { get; set; }
        [ForeignKey("User")]
        public string? CurrUserID { get; set; }
        public User? user { get; set; }

        public IEnumerator<DocFiles> GetEnumerator()
        {
            yield return this;
        }
    }
}
