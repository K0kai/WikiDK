using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikiDK.Objects
{
    public class Rank
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [MaxLength(100)]
        [Column("description")]
        public string? Description { get; set; } = string.Empty;
        [Column("icon")]
        public string Icon { get; set; } = string.Empty;
    }
}
