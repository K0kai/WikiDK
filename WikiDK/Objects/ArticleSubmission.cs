using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WikiDK.Objects
{
    public class ArticleSubmission
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("article_id")]
        public int? ArticleId { get; set; }
        [MaxLength(100)]
        [Required]
        [NotNull]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [MaxLength(100)]
        [Column("description")]
        public string? Description { get; set;  } = string.Empty;
        [Column("content")]
        public string? Content {  get; set; } = string.Empty;
        [Column("thumbnail_link")]
        public string? ArticleThumbnail { get; set; } = string.Empty;
        [Column("submitter_id")]
        public int? SubmitterId { get; set; }
        [NotNull]
        [Column("submitter_name")]
        public string SubmitterName { get; set; } = string.Empty;
        [Column("submitted_at")]
        public DateTimeOffset SubmittedAt { get; set; } = DateTimeOffset.UtcNow;
        [MaxLength(20)]
        [Column("type")]
        public string Type { get; set; } = string.Empty;
        [Column("groups")]
        public List<int>? Groups { get; set; }
        [Column("categories")]
        public List<int>? Categories { get; set; }

    }
}
