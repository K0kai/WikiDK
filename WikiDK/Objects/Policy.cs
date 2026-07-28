using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    [Index(nameof(Name), IsUnique = true)]
    public class Policy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty;
        [Column("description")]
        [StringLength(255)]
        public string Description { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; private set; } = DateTime.UtcNow;
        [JsonIgnore]
        public ICollection<PolicyPermissionRelation> PoliciesPermissions { get; set; } = [];
        [JsonIgnore]
        public ICollection<RolePolicyRelation> RolePolicies { get; set; } = [];
    }
}
