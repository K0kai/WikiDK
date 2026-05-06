using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiDK.Objects
{
    public class ArticleCategory
    {
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("article_id")]
        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
