using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SchoolManagement.Middleware.Authorizations.Handlers
{
    public abstract class BaseAuthorizationHandler<T>: AuthorizationHandler<T> where T : IAuthorizationRequirement   
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        protected BaseAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        protected HttpContext? GetHttpContext() => httpContextAccessor.HttpContext;

        //Get userID via JWT claims "sub" or NameIdentifier
        protected bool TryGetCurrentUserId(AuthorizationHandlerContext context, out int userId)
        {
            userId = 0;
            var claim = context.User.FindFirst(ClaimTypes.NameIdentifier) ?? context.User.FindFirst("sub");
            return claim is not null && int.TryParse(claim.Value, out userId);
        }
        //Get RoleName via JWT claims
        protected string? GetCurrentRole(AuthorizationHandlerContext context) => context.User.FindFirst(ClaimTypes.Role)?.Value;
        protected bool IsAdmin(AuthorizationHandlerContext context) => GetCurrentRole(context) == RoleConstants.Admin;
        protected bool IsTeacher(AuthorizationHandlerContext context) => GetCurrentRole(context) == RoleConstants.Teacher;
        protected bool IsStudent(AuthorizationHandlerContext context) => GetCurrentRole(context) == RoleConstants.Student;

        protected bool TryGetRouteId(string key, out int id)
        {
            id = 0;
            var httpContext = GetHttpContext();
            if (httpContext is null) return false;

            return httpContext.Request.RouteValues.TryGetValue(key, out var value)
                   && int.TryParse(value?.ToString(), out id);
        }
    }
}
