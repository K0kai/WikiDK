using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class UserRoleRelation
    {
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;
        [JsonIgnore]
        public Role Role { get; set; } = null!;
    }
}
