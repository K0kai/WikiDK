using System.Text.Json.Serialization;

namespace WikiDK.Objects
{
    public class PolicyPermissionRelation
    {
        public int PolicyId { get; set; }
        public int PermissionId { get; set; }
        [JsonIgnore]
        public Policy Policy { get; set; } = null!;
        [JsonIgnore]
        public Permission Permission { get; set; } = null!;
    }
}
