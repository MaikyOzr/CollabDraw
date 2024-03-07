using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeCollaborativeWhiteboard.Models
{
    public class PDF
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PDFId { get; set; }
        public string? PDFUrl { get; set; }
        [ForeignKey("User")]
        public string? CurrUserID { get; set; }
        public IEnumerator<PDF> GetEnumerator()
        {
            yield return this;
        }
    }
}
