using Microsoft.AspNetCore.Authorization;

namespace WikiDK.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string[] Permissions { get; set; } = [];

        public PermissionRequirement(string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
