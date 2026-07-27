using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;
using WikiDK.Repositories;
using WikiDK.Services.Interfaces;

namespace WikiDK.Services
{
    public class RolePoliciesService (AppDbContext appDbContext) : IRolePoliciesService
    {
        public Task<RolePolicyRelation> CreateRolePolicyRelation(int roleId, int policyId)
        {
            var rolePolicyRelation = new RolePolicyRelation
            {
                RoleId = roleId,
                PolicyId = policyId
            };
            appDbContext.RolePolicies.Add(rolePolicyRelation);
            appDbContext.SaveChanges();
            return Task.FromResult(rolePolicyRelation);
        }

        public bool DeleteRolePolicyRelation(int roleId, int policyId)
        {
            var rolePolicyRelation = appDbContext.RolePolicies.Find(roleId, policyId);
            if (rolePolicyRelation != null)
            {
                appDbContext.RolePolicies.Remove(rolePolicyRelation);
                appDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<ICollection<Policy>> GetPoliciesByRoleId(int roleId, bool activeOnly = false)
        {
            var query = appDbContext.RolePolicies.Where(rp => rp.RoleId == roleId);

            if (activeOnly)
                query = query.Where(rp => rp.IsActive);

            var policies = await query.Include(rp => rp.Role).Select(rp => rp.Policy).ToListAsync();
            return policies;
        }       

        public async Task<ICollection<Role>> GetRolesByPolicy(int policyId, bool activeOnly = false)
        {
            var query = appDbContext.RolePolicies.Where(rp => rp.PolicyId == policyId);

            if (activeOnly)
                query = query.Where(rp => rp.IsActive);

            var roles = await query.Include(rp => rp.Role).Select(rp => rp.Role).ToListAsync();
            return roles;
        }
    }
}
