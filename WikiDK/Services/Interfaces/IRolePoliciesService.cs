using WikiDK.Objects;

namespace WikiDK.Services.Interfaces
{
    public interface IRolePoliciesService
    {
        Task<RolePolicyRelation> CreateRolePolicyRelation(int roleId, int policyId);
        bool DeleteRolePolicyRelation(int roleId, int policyId);
        Task<ICollection<Role>> GetRolesByPolicy(int policyId, bool activeOnly = false);
        Task<ICollection<Policy>> GetPoliciesByRoleId(int roleId, bool activeOnly = false);
    }
}
