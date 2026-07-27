using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class Role
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
        public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public ICollection<UserRoleRelation> UserRoles { get; set; } = [];
        public ICollection<RolePolicyRelation> RolePolicies { get; set; } = [];
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class RoleCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public User? User { get; set; }
    }
}
