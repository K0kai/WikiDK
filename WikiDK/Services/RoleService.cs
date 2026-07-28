using Microsoft.EntityFrameworkCore;
using WikiDK.Objects;
using WikiDK.Repositories;
using WikiDK.Services.Interfaces;

namespace WikiDK.Services
{
    public class RoleService(AppDbContext dbContext) : IRoleService 
    {
        public async Task<bool> CreateRole(RoleCreateRequest request)
        {
            var now = DateTimeOffset.UtcNow;
            var role = new Role
            {
                Name = request.Name,
                Description = request.Description ?? "",
                CreatedAt = now,
                CreatedBy = request.User!.Id,
                UpdatedBy = null
            };
            dbContext.Roles.Add(role);
            dbContext.SaveChanges();
            return true;
        }

        public async Task<Role?> GetRoleById(int id)
        {
            return await dbContext.Roles.FindAsync(id);
        }

        public async Task<Role?> GetRoleById(string id)
        {
            var tryParse = int.TryParse(id, out var intId);
            if (!tryParse)
                throw new Exception("Invalid numeric string");
            return await GetRoleById(intId);
        }

        public async Task<ICollection<Role>> GetRoles()
        {
            return await dbContext.Roles.ToListAsync();
        }

        public async Task<ICollection<Role>> GetRolesByUserId(int userId)
        {
            var userQuery = await dbContext.Users.Where(u => u.Id == userId).SelectMany(u => u.UserRoles.Select(ur => ur.Role)).ToListAsync();
            return userQuery;
        }
    }
}
