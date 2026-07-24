using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class Policy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTimeOffset UpdatedAt { get; private set; } = DateTime.UtcNow;
        [JsonIgnore]
        public ICollection<PolicyPermissionRelation> PoliciesPermissions { get; set; } = [];
    }
}
