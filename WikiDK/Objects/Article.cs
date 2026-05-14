using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class Article
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [NotNull]
        [MaxLength(100)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [MaxLength(100)]
        [Column("description")]
        public string? Description { get; set; } = string.Empty;
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        [NotNull]
        [Column("created")]
        public DateTimeOffset Created { get; set; } = DateTime.UtcNow;
        [Column("updated")]
        public DateTimeOffset? Updated { get; set; } = null;
        [Column("author_id")]
        public int AuthorId { get; set; }
        [Column("last_editor_id")]
        public int? LastEditorId { get; set; } = int.MinValue;
        [Column("thumb_link")]
        public string? ThumbnailLink { get; set; } = null;
        [Column("categories")]
        public List<int> Categories { get; set; } = [];
        [JsonIgnore]
        public User Author { get; set; } = null!;

    }
}
