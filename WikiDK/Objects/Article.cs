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
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        [NotNull]
        [Column("created")]
        public DateTime Created { get; set; } = DateTime.UtcNow;
        [Column("updated")]
        public DateTime? Updated { get; set; } = null;
        [Column("author_id")]
        public int AuthorId { get; set; }
        [JsonIgnore]
        public User Author { get; set; } = null!;

    }
}
