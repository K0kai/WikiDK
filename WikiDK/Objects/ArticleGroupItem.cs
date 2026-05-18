using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class ArticleGroupItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("article_group_id")]
        public int ArticleGroupId { get; set; }
        [JsonIgnore]
        public ArticleGroup ArticleGroup { get; set; } = null!;
        [JsonIgnore]
        public Article Article { get; set; } = null!;
        [Column("article_id")]
        public int ArticleId { get; set; }
        [Column("position")]
        public int Position { get; set; }
    }
}
