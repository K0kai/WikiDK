using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class Role
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
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
        [Column ("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; } = DateTime.UtcNow;
        [Column("icon")]
        public string? Icon { get; set; } = string.Empty;
        [Column("color")]
        [StringLength(100)]
        public string? Color { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<UserRoleRelation> UserRoles { get; set; } = [];
        [JsonIgnore]
        public ICollection<RolePolicyRelation> RolePolicies { get; set; } = [];
        [Column("created_by")]
        public int CreatedBy { get; set; }
        [Column("updated_by")]
        public int? UpdatedBy { get; set; }
    }

    public class RoleCreateRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; } = null!;
        public User? User { get; set; }
    }

    public class RoleUpdateRequest
    {        
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public User? User { get; set; }

    }
}
