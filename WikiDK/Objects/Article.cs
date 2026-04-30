using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WikiDK.Objects
{
    public class Article
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [Column("content")]
        public string Content { get; set; } = string.Empty;
        [NotNull]
        [Column("created")]
        public DateTime Created { get; set; } = DateTime.MinValue;
        [NotNull]
        [Column("updated")]
        public DateTime Updated { get; set; } = DateTime.MinValue;
        [Column("author_id")]
        public int AuthorId { get; set; }
        public User Author { get; set; } = null!;

    }
}
