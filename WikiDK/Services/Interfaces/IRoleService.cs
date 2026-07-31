using WikiDK.Objects;

namespace WikiDK.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<Role?> GetRoleById(int id);
        public Task<Role?> GetRoleById(string id);
        public Task<ICollection<Role>> GetRoles();
        public Task<ICollection<Role>> GetRolesByUserId(int userId);
        public Task<bool> CreateRole(RoleCreateRequest request);
        public Task<bool> UpdateRole(RoleUpdateRequest uRequest);

    }
}
