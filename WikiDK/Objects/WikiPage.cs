using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiDK.Objects
{
    public class WikiPage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        [StringLength(255)]
        [Required]
        [Column("slug")]
        public string Slug { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        [Column("author_id")]
        [Required]
        public int AuthorId { get; set; }
        [StringLength(100)]
        [Required]
        [Column("author_name")]
        public string AuthorName { get; set; } = null!;
        [Column("editor_id")]
        [Required]
        public int EditorId { get; set; }
        [Column("view_count")]
        public int ViewCount { get; set; } = 0;

    }

    public class WikiPageCreateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
    }

    public class WikiPageUpdateRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; } = string.Empty;
        public int EditorId { get; set; }
    }
}
