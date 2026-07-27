using WikiDK.Objects;

namespace WikiDK.Services.Interfaces
{
    public interface IPoliciesPermissionsService
    {
        public Task<PolicyPermissionRelation> CreatePolicyPermissionRelation(int policyId, int permissionId);
        public bool DeletePolicyPermissonRelation(int policyId, int permissionId);
        public Task<ICollection<Policy>> GetPoliciesByPermission(int permissionId);
        public Task<ICollection<Permission>> GetPermissionsByPolicy(int policyId);
    }
}
