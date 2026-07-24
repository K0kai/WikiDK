using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class UserRoleRelation
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; } = null!;
        [JsonIgnore]
        public Role Role { get; set; } = null!;
    }
}
