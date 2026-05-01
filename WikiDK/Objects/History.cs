using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class History
    {
        [Column("id")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("article_id")]
        public int ArticleId { get; set; }
        [Column("editor_id")]
        public int EditorId { get; set; }
        [MaxLength(100)]
        [Column("previous_title")]
        public string PreviousTitle { get; set; } = string.Empty;
        [Column("previous_content")]
        public string PreviousContent { get; set; } = string.Empty;
        [Column("edit_date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public User Editor { get; set; } = null!;
        [JsonIgnore]
        public Article Article { get; set; } = null!;
    }
}
