using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;
using WikiDK.Repositories;
using WikiDK.Services.Interfaces;

namespace WikiDK.Services
{
    public class PoliciesPermissionsService (AppDbContext appDbContext) : IPoliciesPermissionsService
    {
        public async Task<PolicyPermissionRelation> CreatePolicyPermissionRelation(int policyId, int permissionId)
        {
            var policyPermissionRelation = new PolicyPermissionRelation();
            appDbContext.PolicyPermissions.Add(policyPermissionRelation);
            appDbContext.SaveChanges();
            return policyPermissionRelation;
        }

        public bool DeletePolicyPermissonRelation(int policyId, int permissionId)
        {
            var policyPermissionRelation = appDbContext.PolicyPermissions.Find(policyId, permissionId);
            if (policyPermissionRelation != null)
            {
                appDbContext.PolicyPermissions.Remove(policyPermissionRelation);
                appDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<ICollection<Permission>> GetPermissionsByPolicy(int policyId)
        {
            
            var query = appDbContext.PolicyPermissions.Where(pp => pp.PolicyId == policyId);
            var permissions = await query.Include(pp => pp.Permission).Select(pp => pp.Permission).ToListAsync();
            return permissions;
        }

        public async Task<ICollection<Policy>> GetPoliciesByPermission(int permissionId)
        {
            var query = appDbContext.PolicyPermissions.Where(pp => pp.PermissionId == permissionId);
            var policies = await query.Include(pp => pp.Policy).Select(pp => pp.Policy).ToListAsync();
            return policies;
        }
    }
}
