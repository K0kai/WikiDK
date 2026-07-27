using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class RolePolicyRelation
    {
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("policy_id")]
        public int PolicyId { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public Role Role { get; set; } = null!;
        [JsonIgnore]
        public Policy Policy { get; set; } = null!;
    }
}
