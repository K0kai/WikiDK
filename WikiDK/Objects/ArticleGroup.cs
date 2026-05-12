using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiDK.Objects
{
    public class ArticleGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [MaxLength(20)]
        [Column("title")]
        public string Title { get; set; } = string.Empty;
        [Column("description")]
        [MaxLength(50)]
        public string Description { get; set; } = string.Empty;
        public ICollection<ArticleGroupItem> Items { get; set; } = [];
        [Column("locked")]
        public bool Locked { get; set; } = false;
        [Column("display_on_home")]
        public bool DisplayOnHome { get; set; } = false;
        [Column("display_on_sidebar")]
        public bool DisplayOnSidebar { get; set; } = false;
    }
}
