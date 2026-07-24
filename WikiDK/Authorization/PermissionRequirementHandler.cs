using Microsoft.AspNetCore.Authorization;

namespace WikiDK.Authorization
{
    public class PermissionRequirementHandler(ILogger<PermissionRequirementHandler> logger) : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Checking if user has permissions: {permissions}", string.Join(" | ", requirement.Permissions));

            return Task.CompletedTask;
        }
    }
}
