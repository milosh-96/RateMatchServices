using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsService.Data
{
    public class ExternalContentLink
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Source { get; set; } = "undefined";

        public string Title { get; set; } = "";

        public string ExternalUrl { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public override string ToString()
        {
            return String.Format("{0}: {1}", this.Source, this.Title);
        }
    }
}
