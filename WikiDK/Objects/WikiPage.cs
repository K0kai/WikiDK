using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiDK.Objects
{
    public class WikiPage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [StringLength(255)]
        [Required]
        public string Slug { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
        [Required]
        public int AuthorId { get; set; }
        [StringLength(100)]
        [Required]
        public string AuthorName { get; set; } = null!;
        [Required]
        public int EditorId { get; set; }
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
