using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiDK.Objects
{
    public class Category
    {
        [Key]
        [Required]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column("description")]
        public string? Description { get; set; } = string.Empty;
        [Column("slug")]
        public string? Slug { get; set; } = string.Empty;
        [Column("icon")]
        public string? Icon { get; set; } = null;
    }
}
