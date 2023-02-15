using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsService.Data
{
    public class NewsArticle
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(256)")]
        public string Title { get; set; } = "";
        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "varchar(256)")]
        public string Source { get; set; } = "Undefined"; 
        
        [Column(TypeName = "varchar(256)")]
        public string ArticleLink { get; set; } = "Undefined";
    }
}
