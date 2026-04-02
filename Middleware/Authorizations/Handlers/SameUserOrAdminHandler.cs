using Microsoft.AspNetCore.Authorization;
using SchoolManagement.Middleware.Authorizations.Requirements;

namespace SchoolManagement.Middleware.Authorizations.Handlers
{
    public class SameUserOrAdminHandler : BaseAuthorizationHandler<SameUserOrAdminRequirement>
    {
        public SameUserOrAdminHandler(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor) { }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserOrAdminRequirement requirement)
        {
            if (IsAdmin(context))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            if (!TryGetCurrentUserId(context, out int userId)) return Task.CompletedTask;
            if (TryGetRouteId("id", out int targetId) && userId == targetId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
