using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class PolicyPermissionRelation
    {
        [Column("policy_id")]
        public int PolicyId { get; set; }
        [Column("permission_id")]
        public int PermissionId { get; set; }
        [Column("is_active")]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public Policy Policy { get; set; } = null!;
        [JsonIgnore]
        public Permission Permission { get; set; } = null!;
    }
}
